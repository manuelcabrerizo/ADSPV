using System;
using System.Collections.Generic;
using View.Exceptions;

namespace ZooArchitect.View
{
    public class GameObject : IDisposable
    {
        private Dictionary<Type, Component> components;
        public Dictionary<Type, Component>.ValueCollection Components => components.Values;
        public Transform transform { get; set; }
        public string name { get; set; }

        public GameObject() 
        {
            components = new Dictionary<Type, Component>();
        }

        public void Dispose()
        {
            foreach (Component component in components.Values)
            {
                component.OnDisable();
            }
        }

        public void Awake()
        {
            foreach (Component component in components.Values)
            {
                component.Awake();
            }
        }

        public void Start()
        {
            foreach (Component component in components.Values)
            {
                component.Start();
            }
        }

        public void Update(float deltaTime)
        {
            foreach (Component component in components.Values)
            {
                component.Update(deltaTime);
            }
        }

        public Component AddComponent(Type componentType)
        {
            if (!components.ContainsKey(componentType))
            {
                Component component = Activator.CreateInstance(componentType) as Component;
                components.Add(componentType, component);
                component.SetOwner(this);

                Transform transformComponent = GetComponent<Transform>();
                transform = transformComponent;
                component.transform = transformComponent;
                return component;
            }
            throw new ParameterlessConstructorNotFoundException("Parameterless Constructor Not Found in: " + componentType.Name);
        }

        public ComponentType AddComponent<ComponentType>() where ComponentType : Component
        {
            return AddComponent(typeof(ComponentType)) as ComponentType;
        }

        public void RemoveComponent<ComponentType>() where ComponentType : Component
        {
            if (components.ContainsKey(typeof(ComponentType)))
            {
                components.Remove(typeof(ComponentType));
            }
        }

        public ComponentType GetComponent<ComponentType>() where ComponentType : Component
        {
            if (components.ContainsKey(typeof(ComponentType)))
            { 
                return components[typeof(ComponentType)] as ComponentType;
            }
            return null;
        }
    }
}