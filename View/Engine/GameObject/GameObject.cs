using Rexar.Toolbox.DataFlow;
using System;
using System.Collections.Generic;

namespace ZooArchitect.View
{
    public class GameObject : IInitable, ITickable, IDisposable
    {
        private Dictionary<Type, Component> components;
        public Dictionary<Type, Component>.ValueCollection Components => components.Values;

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
            if (!components.ContainsKey(componentType))
            {
                Component component = Activator.CreateInstance(componentType) as Component;
                components.Add(componentType, component);
                component.SetOwner(this);
                return component;
            }
            throw new InvalidOperationException(); // TODO: custom exception
        }

        public void AddComponent<ComponentType>(ComponentType component) where ComponentType : Component
        {
            if (!components.ContainsKey(typeof(ComponentType)))
            {
                component.SetOwner(this);
                components.Add(typeof(ComponentType), component);
            }
            else
            {
                throw new InvalidOperationException(); // TODO: custom exception
            }
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