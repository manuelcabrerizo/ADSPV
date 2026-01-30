using Architecture.GameLogic.Entities.Systems.Events;
using OpenTK.Mathematics;
using Rexar.Toolbox.Events;
using Rexar.Toolbox.Services;
using System;
using System.Reflection;
using ZooArchitect.Architecture.GameLogic.Entities;
using ZooArchitect.Architecture.GameLogic.Entities.Systems;
using ZooArchitect.View.Data;
using ZooArchitect.View.GameLogic.Entities.Instances;
using ZooArchitect.View.Resources;

namespace ZooArchitect.View.GameLogic.Entities.Systems
{
    [ViewOf(typeof(EntityFactory))]
    internal sealed class EntityFactoryView : IDisposable
    {
        private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();
        private PrefabsRegistryView PrefabsRegistryView => ServiceProvider.Instance.GetService<PrefabsRegistryView>();
        private EntityRegistryView EntityRegistryView => ServiceProvider.Instance.GetService<EntityRegistryView>();
        private EntityRegistry EntityRegistry => ServiceProvider.Instance.GetService<EntityRegistry>();
        private GameScene GameScene => ServiceProvider.Instance.GetService<GameScene>();


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

        private void OnEntityCreated(in EntityCreatedEvent<Entity> entityCreatedEvent)
        {
            ViewComponent viewComponent = GameScene.AddSceneComponent(
                ViewArchitectureMap.ViewOf(EntityRegistry[entityCreatedEvent.entityCreatedId].GetType()),
                PrefabsRegistryView.Get(TableNamesView.ANIMALS_VIEW_TABLE_NAME, entityCreatedEvent.blueprintId).name += $"  -  Architecture type: {EntityRegistry[entityCreatedEvent.entityCreatedId].GetType().Name} - ID: {entityCreatedEvent.entityCreatedId}",
                GameScene.EntitiesContainer.transform,
                PrefabsRegistryView.Get(TableNamesView.ANIMALS_VIEW_TABLE_NAME, entityCreatedEvent.blueprintId));

            viewComponent.transform.position = new Vector3((float)entityCreatedEvent.coordinate.Origin.X, (float)entityCreatedEvent.coordinate.Origin.Y, 0.0f);

            setEntityIdMethod.Invoke(viewComponent, new object[] { entityCreatedEvent.entityCreatedId });
            registerEntityMethod.Invoke(EntityRegistryView, new object[] { viewComponent });
        }

        public void Dispose()
        {
            EventBus.Unsubscribe<EntityCreatedEvent<Entity>>(OnEntityCreated);
        }
    }
}