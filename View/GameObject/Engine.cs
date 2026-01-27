using OpenTK.Mathematics;
using Rexar.Toolbox.DataFlow;
using Rexar.Toolbox.Services;
using System;
using System.Collections.Generic;
using System.Xml;

namespace ZooArchitect.View
{
    public sealed class Engine : IInitable, ITickable, IService
    {
        public bool IsPersistance => true;


        List<GameObject> gameObjects;
        List<GameObject> gameObjectsToAdd;
        List<GameObject> gameObjectsToRemove;

        public Engine()
        {
            gameObjects = new List<GameObject>();
            gameObjectsToAdd = new List<GameObject>();
            gameObjectsToRemove = new List<GameObject>();
        }

        public void Init()
        {
            foreach (GameObject go in gameObjectsToAdd)
            {
                gameObjects.Add(go);
            }
            gameObjectsToAdd.Clear();


            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Init();
            }
        }

        public void LateInit()
        {
            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.LateInit();
            }
        }

        public void Tick(float deltaTime)
        {
            RemoveDeletedGameObjects();
            InitNewGameObjects();
            LateInitNewGameObjects();

            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Tick(deltaTime);
            }
        }

        private void InitNewGameObjects()
        {
            foreach (GameObject go in gameObjectsToAdd)
            {
                go.Init();
            }
        }

        private void LateInitNewGameObjects()
        {
            foreach (GameObject go in gameObjectsToAdd)
            {
                go.LateInit();
                gameObjects.Add(go);
            }
            gameObjectsToAdd.Clear();
        }

        private void RemoveDeletedGameObjects()
        {
            foreach (GameObject go in gameObjectsToRemove)
            {
                gameObjects.Remove(go);
            }
            gameObjectsToRemove.Clear();
        }

        public GameObject Instantiate(GameObject prefab, Vector3 position)
        {
            if (prefab != null)
            {
                GameObject gameObject = new GameObject();
                foreach (Component component in prefab.Components)
                {
                    Type componentType = component.GetType();
                    Component newComponent = gameObject.AddComponent(componentType);
                    newComponent.SetOwner(gameObject);
                    newComponent.Copy(component);
                }
                gameObjectsToAdd.Add(gameObject);

                Transform transform = gameObject.GetComponent<Transform>();
                transform.position = position;
                return gameObject;
            }
            else
            {
                GameObject gameObject = new GameObject();
                gameObject.AddComponent<Transform>(new Transform(gameObject, position, Vector3.One, 0.0f));
                gameObjectsToAdd.Add(gameObject);
                return gameObject;
            }
        }
        public GameObject LoadPrefab(string prefabPath)
        {
            GameObject gameObject = new GameObject();

            XmlDocument doc = new XmlDocument();
            doc.Load(prefabPath);

            for (int i = 0; i < doc.DocumentElement.ChildNodes.Count; i++)
            {
                var node = doc.DocumentElement.ChildNodes[i];
                if (string.Equals(node.Name, "Transform"))
                {
                    var childs = node.ChildNodes;
                    Vector3 position = Vector3.Zero;
                    position.X = float.Parse(childs[0].Attributes["x"].Value);
                    position.Y = float.Parse(childs[0].Attributes["y"].Value);
                    position.Z = float.Parse(childs[0].Attributes["z"].Value);
                    Vector3 size = Vector3.One;
                    size.X = float.Parse(childs[1].Attributes["x"].Value);
                    size.Y = float.Parse(childs[1].Attributes["y"].Value);
                    size.Z = float.Parse(childs[1].Attributes["z"].Value);
                    float rotation = float.Parse(childs[2].Attributes["r"].Value);
                    gameObject.AddComponent<Transform>(new Transform(gameObject, position, size, rotation));
                }
                else if (string.Equals(node.Name, "Sprite"))
                {
                    var childs = node.ChildNodes;
                    Vector3 color = Vector3.Zero;
                    color.X = float.Parse(childs[0].Attributes["x"].Value);
                    color.Y = float.Parse(childs[0].Attributes["y"].Value);
                    color.Z = float.Parse(childs[0].Attributes["z"].Value);
                    gameObject.AddComponent<Sprite>(new Sprite(gameObject, color));
                }
            }
            return gameObject;
        }
        public void Destroy(GameObject gameObject)
        {
            gameObjectsToRemove.Add(gameObject);
        }

    }
}
