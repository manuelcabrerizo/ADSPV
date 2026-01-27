using Rexar.Toolbox.DataFlow;
using System;
using System.Collections.Generic;

namespace ZooArchitect.View
{
    public class GameObject : IInitable, ITickable
    {
        private Dictionary<Type, Component> components;

        public Dictionary<Type, Component>.ValueCollection Components => components.Values;

        public GameObject() 
        {
            components = new Dictionary<Type, Component>();
        }
        public void Init()
        {
            foreach (Component component in components.Values)
            {
                component.Init();
            }
        }

        public void LateInit()
        {
            foreach (Component component in components.Values)
            {
                component.LateInit();
            }
        }

        public void Tick(float deltaTime)
        {
            foreach (Component component in components.Values)
            {
                component.Tick(deltaTime);
            }
        }

        public Component AddComponent(Type componentType)
        {
            Component component = Activator.CreateInstance(componentType) as Component;
            components.Add(componentType, component); 
            return component;
        }

        public void AddComponent<ComponentType>(ComponentType component) where ComponentType : Component
        {
            components.Add(typeof(ComponentType), component);
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