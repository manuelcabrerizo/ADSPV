using ZooArchitect.Architecture.GameLogic.Math;

namespace Architecture.GameLogic.Entities
{
    public sealed class Visitor : Human
    {
        private Visitor(uint ID, Coordinate coordinate) : base(ID, coordinate)
        {
        }
    }
}