using ZooArchitect.Architecture.GameLogic.Entities;
using ZooArchitect.Architecture.GameLogic.Math;

namespace Architecture.GameLogic.Entities
{
    public abstract class Structure : Entity
    {
        protected Structure(uint ID, Coordinate coordinate) : base(ID, coordinate)
        {
        }
    }
}