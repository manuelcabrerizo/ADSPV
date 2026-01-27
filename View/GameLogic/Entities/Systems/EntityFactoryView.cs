using Architecture.GameLogic.Entities.Systems.Events;
using OpenTK.Mathematics;
using Rexar.Toolbox.Events;
using Rexar.Toolbox.Services;
using System;
using System.Reflection;
using ZooArchitect.Architecture.GameLogic.Entities;
using ZooArchitect.View.GameLogic.Entities.Instances;
using ZooArchitect.View.Resources;

namespace ZooArchitect.View.GameLogic.Entities.Systems
{
    internal sealed class EntityFactoryView : IDisposable
    {
        private Engine Engine => ServiceProvider.Instance.GetService<Engine>();
        private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();
        private PrefabsRegistryView PrefabsRegistryView => ServiceProvider.Instance.GetService<PrefabsRegistryView>();
        private EntityRegistryView EntityRegistryView => ServiceProvider.Instance.GetService<EntityRegistryView>();


        private MethodInfo registerEntityMethod;

        public EntityFactoryView() 
        {
            EventBus.Subscribe<EntityCreatedEvent<Entity>>(OnEntityCreated);

            registerEntityMethod = EntityRegistryView.GetType().GetMethod(EntityRegistryView.RegisterMethodName,
                BindingFlags.NonPublic | BindingFlags.Instance);

        }

        private void OnEntityCreated(in EntityCreatedEvent<Entity> callback)
        {
            GameObject instance = Engine.Instantiate(PrefabsRegistryView.Get(callback.blueprintId),
                new Vector3((float)callback.coordinate.Origin.X, (float)callback.coordinate.Origin.Y, 0.0f));
           
            //registerEntityMethod.Invoke(EntityRegistryView, new object[] { instance.GetComponent<EntityView>() });
        }

        public void Dispose()
        {
            EventBus.Unsubscribe<EntityCreatedEvent<Entity>>(OnEntityCreated);
        }
    }
}