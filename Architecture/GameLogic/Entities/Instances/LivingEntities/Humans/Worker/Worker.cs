using ZooArchitect.Architecture.GameLogic.Math;

namespace Architecture.GameLogic.Entities
{
    public sealed class Worker : Human
    {
        private Worker(uint ID, Coordinate coordinate) : base(ID, coordinate)
        {
        }
    }
}