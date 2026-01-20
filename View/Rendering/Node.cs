using System.Collections.Generic;

namespace ZooArchitect.View.Rendering
{
    class Node : Spatial
    {
        protected List<Spatial> childs;

        public Node()
        { 
            childs = new List<Spatial>();
        }

        public int GetQuantity()
        {
            return childs.Count;
        }

        public int AttachChild(Spatial child)
        {
            child.SetParent(this);
            childs.Add(child);
            return childs.Count;
        }

        public int DetachChild(Spatial child)
        {
            if (childs.Contains(child))
            { 
                childs.Remove(child);
                return 1;
            }
            return -1;
        }

        protected override void UpdateWorldData(float deltaTime)
        {
            base.UpdateWorldData(deltaTime);
            foreach (Spatial child in childs)
            {
                child.UpdateGS(deltaTime);
            }
        }

    }
}
