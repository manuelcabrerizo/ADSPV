using Rexar.Toolbox.Services;
using System;
using System.Collections.Generic;
using ZooArchitect.Architecture.GameLogic.Entities;
using ZooArchitect.View.GameLogic.Entities.Instances;

namespace ZooArchitect.View.GameLogic.Entities.Systems
{
    internal sealed class EntityRegistryView : IService
    {
        public bool IsPersistance => false;

        private Dictionary<uint, EntityView> entities;
        private Dictionary<Type, List<uint>> entityIdsPerType;

        public EntityRegistryView()
        {
            entities = new Dictionary<uint, EntityView>();
            entityIdsPerType = new Dictionary<Type, List<uint>>();
        }

        internal string RegisterMethodName => nameof(Register);
        private void Register(EntityView entityView)
        {
            entities.Add(entityView.ArchitectureEntityID, entityView);
            Type currentEntityType = entityView.GetType();
            do
            {
                if (!entityIdsPerType.ContainsKey(currentEntityType))
                {
                    entityIdsPerType.Add(currentEntityType, new List<uint>());
                }
                entityIdsPerType[currentEntityType].Add(entityView.ArchitectureEntityID);
                currentEntityType = currentEntityType.BaseType;
            } while (currentEntityType != typeof(EntityView));
        }

        public EntityType GetAs<EntityType>(uint ID) where EntityType : EntityView
        {
            if (ID == Entity.UNASSIGNED_ENTITY_ID)
            {
                throw new NullReferenceException("Entity id 0 represent a null entity");
            }
            if (!entities.ContainsKey(ID))
            {
                throw new KeyNotFoundException(ID.ToString());
            }
            if (entities[ID] is not EntityType)
            {
                throw new InvalidCastException($"An attempt was made to obtain  type {entities[ID].GetType().Name}" +
                    $" entity as type {typeof(EntityType).Name} from EntityRegistry");
            }
            return entities[ID] as EntityType;
        }

        public IEnumerable<EntityType> FilterEntities<EntityType>() where EntityType : EntityView
        {
            if (entityIdsPerType.ContainsKey(typeof(EntityType)))
            {
                foreach (uint ID in entityIdsPerType[typeof(EntityType)])
                {
                    yield return entities[ID] as EntityType;
                }
            }
        }
    }
}
