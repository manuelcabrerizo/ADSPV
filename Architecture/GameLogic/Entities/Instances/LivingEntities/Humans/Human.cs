using ZooArchitect.Architecture.GameLogic.Math;

namespace Architecture.GameLogic.Entities
{
    public abstract class Human : LivingEntity
    {
        protected Human(uint ID, Coordinate coordinate) : base(ID, coordinate)
        {
        }
    }
}