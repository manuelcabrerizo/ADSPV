using Rexar.Toolbox.Blueprint;
using ZooArchitect.Architecture.GameLogic.Math;

namespace ZooArchitect.Architecture.GameLogic.Entities.Animals
{
    public sealed class Animal : Entity
    {
        [BlueprintParameter("Life")] private int[] life;

        protected Animal(uint ID, Coordinate coordinate) : base(ID, coordinate)
        {
        }
    }
}
