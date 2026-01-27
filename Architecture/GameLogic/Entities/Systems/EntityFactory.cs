using Architecture.GameLogic.Entities;
using Architecture.GameLogic.Entities.Systems.Events;
using Rexar.Toolbox.Blueprint;
using Rexar.Toolbox.Events;
using Rexar.Toolbox.Services;
using System;
using System.Collections.Generic;
using System.Reflection;
using ZooArchitect.Architecture.Data;
using ZooArchitect.Architecture.GameLogic.Math;

namespace ZooArchitect.Architecture.GameLogic.Entities.Systems
{
    public sealed class EntityFactory : IService
    {
        private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();
        private EntityRegistry EntityRegistry => ServiceProvider.Instance.GetService<EntityRegistry>();
        private BlueprintBinder BlueprintBinder => ServiceProvider.Instance.GetService<BlueprintBinder>();


        private uint lastAssignedEntityId;
        public bool IsPersistance => false;

        private Dictionary<Type, ConstructorInfo> entityConstructors;
        private MethodInfo registerEntityMethod;
        private MethodInfo raiseEntityCreatedMethod;

        public EntityFactory() 
        {
            lastAssignedEntityId = Entity.UNASSIGNED_ENTITY_ID;
            entityConstructors = new Dictionary<Type, ConstructorInfo>();
            registerEntityMethod = EntityRegistry.GetType().GetMethod(EntityRegistry.RegisterMethodName, BindingFlags.NonPublic | BindingFlags.Instance);
            raiseEntityCreatedMethod = GetType().GetMethod(nameof(RaiseEntityCreatedMethod), BindingFlags.NonPublic | BindingFlags.Instance);

            RegisterEntityMethods();
        }

        public void CreateInstance<EntityType>(string blueprintId, Coordinate coordinate) where EntityType : Entity
        {
            lastAssignedEntityId++;
            uint newEntityId = lastAssignedEntityId;
            if (!entityConstructors.ContainsKey(typeof(EntityType)))
            {
                throw new MissingMethodException($"Missing constructor for {typeof(EntityType).Name}");
            }

            object newEntity = entityConstructors[typeof(EntityType)].Invoke(new object[] {newEntityId, coordinate});

            BlueprintBinder.Apply(ref newEntity, TableNames.ANIMALS_TABLE_NAME, blueprintId);

            (newEntity as EntityType).Init();

            if (registerEntityMethod == null)
            {
                throw new MissingMethodException($"Missing EntityRegister register method");
            }
            registerEntityMethod.Invoke(EntityRegistry, new object[] { newEntity });

            List<Type> entityTypes = new List<Type>();
            Type currentEntityType = null;
            do
            {
                currentEntityType = currentEntityType == null ? newEntity.GetType() : currentEntityType.BaseType;
                entityTypes.Add(currentEntityType);

            } while (currentEntityType != typeof(Entity));

            for (int i = entityTypes.Count - 1; i >= 0; i--)
            {
                raiseEntityCreatedMethod.MakeGenericMethod(entityTypes[i]).Invoke(this, new object[] { blueprintId, newEntity });
            }

            (newEntity as EntityType).LateInit();
        }

        private void RaiseEntityCreatedMethod<EntityType>(string blueprintId, EntityType newEntity) where EntityType : Entity
        {
            EventBus.Raise<EntityCreatedEvent<EntityType>>(blueprintId, newEntity.ID, newEntity.coordinate);
        }

        private void RegisterEntityMethods()
        {
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (type.IsClass && !type.IsAbstract)
                {
                    if (typeof(Entity).IsAssignableFrom(type))
                    {
                        RegisterEntity(type);
                    }
                }
            }

            void RegisterEntity(Type entityType)
            {
                foreach (ConstructorInfo constructorInfo in entityType.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance))
                {
                    ParameterInfo[] parameters = constructorInfo.GetParameters();
                    if (parameters.Length == 2 &&
                        parameters[0].ParameterType == typeof(uint) &&
                        parameters[1].ParameterType == typeof(Coordinate))
                    {
                        entityConstructors.Add(entityType, constructorInfo);
                        break;
                    }
                }
            }
        }

    }
}
