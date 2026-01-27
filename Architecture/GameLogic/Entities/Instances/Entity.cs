using Rexar.Toolbox.DataFlow;
using ZooArchitect.Architecture.GameLogic.Math;

namespace ZooArchitect.Architecture.GameLogic.Entities
{
    public abstract class Entity : IInitable, ITickable
    {
        public const uint UNASSIGNED_ENTITY_ID = 0;

        public uint ID;
        public Coordinate coordinate;

        protected Entity(uint ID, Coordinate coordinate) 
        {
            this.ID = ID;
            this.coordinate = coordinate;
        }

        public virtual void Init() { }
        public virtual void LateInit() { }
        public virtual void Tick(float deltaTime) { }
    }
}