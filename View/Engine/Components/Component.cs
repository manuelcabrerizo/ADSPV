using Rexar.Toolbox.DataFlow;

namespace ZooArchitect.View
{
    public abstract class Component : IInitable, ITickable
    {
        protected GameObject owner;
        public void SetOwner(GameObject owner)
        { 
            this.owner = owner;
        }
        public virtual void Copy(Component component) { }
        public virtual void Init() { }
        public virtual void LateInit() { }
        public virtual void Tick(float deltaTime) { }
        public abstract void OnDisable();

        public ComponentType GetComponent<ComponentType>() where ComponentType : Component
        { 
            return owner.GetComponent<ComponentType>();
        }
    }
}