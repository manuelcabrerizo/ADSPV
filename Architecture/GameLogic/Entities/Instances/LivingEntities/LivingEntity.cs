using ZooArchitect.Architecture.GameLogic.Entities;
using ZooArchitect.Architecture.GameLogic.Math;

namespace Architecture.GameLogic.Entities
{
    public abstract class LivingEntity : Entity
    {
        protected LivingEntity(uint ID, Coordinate coordinate) : base(ID, coordinate)
        {
        }
    }
}