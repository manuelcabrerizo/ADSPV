using Rexar.Toolbox.Events;
using ZooArchitect.Architecture.GameLogic.Math;

namespace ZooArchitect.Architecture.Controllers.Events
{
    public struct SpawnEntityRequestAceptedEvent : IEvent
    {
        public string blueprintToSpawn;
        public Coordinate coordinateToSpawn;
        public void Assign(params object[] parameters)
        {
            blueprintToSpawn = parameters[0] as string;
            coordinateToSpawn = (Coordinate)parameters[1];
        }
        public void Reset()
        {
            blueprintToSpawn = string.Empty;
            coordinateToSpawn = default(Coordinate);
        }
    }
}
