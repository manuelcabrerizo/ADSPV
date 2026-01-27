using Architecture.GameLogic.Entities.Systems.Events;
using OpenTK.Mathematics;
using Rexar.Toolbox.Events;
using Rexar.Toolbox.Services;
using System;
using System.Reflection;
using ZooArchitect.Architecture.GameLogic.Entities;
using ZooArchitect.Architecture.GameLogic.Entities.Systems;
using ZooArchitect.View.GameLogic.Entities.Instances;
using ZooArchitect.View.Resources;

namespace ZooArchitect.View.GameLogic.Entities.Systems
{
    [ViewOf(typeof(EntityFactory))]
    internal sealed class EntityFactoryView : IDisposable
    {
        private Engine Engine => ServiceProvider.Instance.GetService<Engine>();
        private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();
        private PrefabsRegistryView PrefabsRegistryView => ServiceProvider.Instance.GetService<PrefabsRegistryView>();
        private EntityRegistryView EntityRegistryView => ServiceProvider.Instance.GetService<EntityRegistryView>();
        private EntityRegistry EntityRegistry => ServiceProvider.Instance.GetService<EntityRegistry>();

        private MethodInfo registerEntityMethod;
        private MethodInfo setEntityIdMethod;

        public EntityFactoryView() 
        {
            EventBus.Subscribe<EntityCreatedEvent<Entity>>(OnEntityCreated);

            registerEntityMethod = EntityRegistryView.GetType().GetMethod(EntityRegistryView.RegisterMethodName,
                BindingFlags.NonPublic | BindingFlags.Instance);
            setEntityIdMethod = typeof(EntityView).GetMethod(EntityView.SetIdMethodName,
                BindingFlags.NonPublic | BindingFlags.Instance);
        }

        private void OnEntityCreated(in EntityCreatedEvent<Entity> callback)
        {
            GameObject instance = Engine.Instantiate(PrefabsRegistryView.Get(callback.blueprintId),
                new Vector3((float)callback.coordinate.Origin.X, (float)callback.coordinate.Origin.Y, 0.0f));

            Component viewComponent = instance.AddComponent(ViewArchitectureMap.ViewOf(EntityRegistry[callback.entityCreatedId].GetType()));

            // TODO: viewComponent.gameObject.name += ...

            setEntityIdMethod.Invoke(viewComponent, new object[] { callback.entityCreatedId });
            registerEntityMethod.Invoke(EntityRegistryView, new object[] { viewComponent });
        }

        public void Dispose()
        {
            EventBus.Unsubscribe<EntityCreatedEvent<Entity>>(OnEntityCreated);
        }
    }
}