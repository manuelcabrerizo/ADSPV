using OpenTK.Mathematics;
using Rexar.Toolbox.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace ZooArchitect.View
{
    public sealed class Engine : IService, IDisposable
    {
        public bool IsPersistance => true;

        List<GameObject> gameObjects;
        List<GameObject> gameObjectsToInit;
        List<GameObject> gameObjectsToLateInit;
        List<GameObject> gameObjectsToRemove;

        public Engine()
        {
            gameObjects = new List<GameObject>();
            gameObjectsToInit = new List<GameObject>();
            gameObjectsToLateInit = new List<GameObject>();
            gameObjectsToRemove = new List<GameObject>();
        }

        public void Dispose()
        {
            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Dispose();
            }
            gameObjects.Clear();
        }

        public void Awake()
        {
            AwakeNewGameObjects();
        }

        public void Start()
        {
            StartNewGameObjects();
        }

        public void Update(float deltaTime)
        {
            RemoveDeletedGameObjects();
            AwakeNewGameObjects();
            StartNewGameObjects();

            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Update(deltaTime);
            }
        }

        private void AwakeNewGameObjects()
        {
            for (int i = gameObjectsToInit.Count - 1; i >= 0; i--)
            { 
                gameObjectsToInit[i].Awake();
                gameObjectsToLateInit.Add(gameObjectsToInit[i]);
                gameObjectsToInit.RemoveAt(i);
            }
        }

        private void StartNewGameObjects()
        {
            for (int i = gameObjectsToLateInit.Count - 1; i >= 0; i--)
            {
                gameObjectsToLateInit[i].Start();
                gameObjects.Add(gameObjectsToLateInit[i]);
                gameObjectsToLateInit.RemoveAt(i);
            }
        }

        private void RemoveDeletedGameObjects()
        {
            foreach (GameObject go in gameObjectsToRemove)
            {
                go.Dispose();
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
                    newComponent.Copy(component);
                }
                gameObjectsToInit.Add(gameObject);

                Transform transform = gameObject.GetComponent<Transform>();
                transform.position = position;
                return gameObject;
            }
            else
            {
                GameObject gameObject = new GameObject();
                Transform transform = gameObject.AddComponent<Transform>();
                transform.position = position;
                transform.size = Vector3.One;
                transform.rotation = 0.0f;
                gameObjectsToInit.Add(gameObject);
                return gameObject;
            }
        }

        // TODO: use reflection to make this function better
        public GameObject LoadPrefab(string prefabPath)
        {
            try
            {
                GameObject gameObject = new GameObject();
                XmlDocument doc = new XmlDocument();
                doc.Load(prefabPath);
                XmlElement root = doc.DocumentElement;
                gameObject.name = root.Attributes["tag"].Value;
                for (int i = 0; i < root.ChildNodes.Count; i++)
                {
                    var node = root.ChildNodes[i];
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
                        Transform transform = gameObject.AddComponent<Transform>();
                        transform.position = position;
                        transform.size = size;
                        transform.rotation = rotation;
                    }
                    else if (string.Equals(node.Name, "Sprite"))
                    {
                        var childs = node.ChildNodes;
                        Vector3 color = Vector3.Zero;
                        color.X = float.Parse(childs[0].Attributes["x"].Value);
                        color.Y = float.Parse(childs[0].Attributes["y"].Value);
                        color.Z = float.Parse(childs[0].Attributes["z"].Value);
                        Sprite sprite = gameObject.AddComponent<Sprite>();
                        sprite.color = color;
                    }
                }
                return gameObject;
            }
            catch (FileNotFoundException e)
            { 
                return null;
            }
        }
        public void Destroy(GameObject gameObject)
        {
            gameObjectsToRemove.Add(gameObject);
        }
    }
}
