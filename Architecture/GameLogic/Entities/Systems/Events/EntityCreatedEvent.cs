using Rexar.Toolbox.Events;
using ZooArchitect.Architecture.GameLogic.Entities;
using ZooArchitect.Architecture.GameLogic.Math;

namespace Architecture.GameLogic.Entities.Systems.Events
{
    public struct EntityCreatedEvent<EntityType> : IEvent where EntityType : Entity
    {
        public string blueprintId;
        public uint entityCreatedId;
        public Coordinate coordinate;

        public void Assign(params object[] parameters)
        {
            blueprintId = (string)parameters[0];
            entityCreatedId = (uint)parameters[1];
            coordinate = (Coordinate)parameters[2];
        }

        public void Reset()
        {
            blueprintId = string.Empty;
            entityCreatedId = Entity.UNASSIGNED_ENTITY_ID;
            coordinate = default(Coordinate);
        }
    }
}