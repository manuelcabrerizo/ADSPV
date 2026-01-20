namespace ZooArchitect.View.Rendering
{
    class Spatial : Object
    {
        public Transformation Local;
        public Transformation World;
        public bool WorldIsCurrent;
        protected Spatial parent;

        public Spatial()
        {
            Local = new Transformation();
            World = new Transformation();
            WorldIsCurrent = false;
            parent = null;
        }

        public void UpdateGS(float deltaTime, bool isInitiator = true) 
        {
            UpdateWorldData(deltaTime);
        }

        protected virtual void UpdateWorldData(float deltaTime) 
        {
            if (!WorldIsCurrent)
            {
                if (parent != null)
                {
                    World.Product(parent.World, Local); // = parent.World * Local; // TODO: ...
                }
                else
                {
                    World = Local;
                }
            }
        }

        public void SetParent(Spatial parent)
        {
            this.parent = parent;
        }

    }
}
