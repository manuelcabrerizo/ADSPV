using ZooArchitect.Architecture.GameLogic.Math;

namespace Architecture.GameLogic.Entities
{
    public sealed class Animal : LivingEntity
    {
        private Animal(uint ID, Coordinate coordinate) : base(ID, coordinate)
        {
        }
    }
}
