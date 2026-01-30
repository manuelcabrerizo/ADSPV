namespace ZooArchitect.View
{
    public abstract class Component
    {
        public GameObject gameObject;
        public Transform transform { get; set; }
        public void SetOwner(GameObject gameObject)
        { 
            this.gameObject = gameObject;
        }
        public virtual void Awake() { }
        public virtual void Start() { }
        public virtual void Update(float deltaTime) { }
        public virtual void OnDisable() { }
        public virtual void Copy(Component component) { }

        public ComponentType GetComponent<ComponentType>() where ComponentType : Component
        { 
            return gameObject.GetComponent<ComponentType>();
        }
    }
}