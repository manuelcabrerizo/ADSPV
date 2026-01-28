using Architecture.GameLogic.Entities;
using Rexar.Toolbox.Events;
using Rexar.Toolbox.Services;
using System;
using ZooArchitect.Architecture.Controllers.Events;
using ZooArchitect.Architecture.GameLogic.Entities.Systems;

namespace ZooArchitect.Architecture.Controllers
{
    public sealed class SpawnEntityControllerArchitecture : IDisposable
    {
        private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();
        private EntityRegistry EntityRegistry => ServiceProvider.Instance.GetService<EntityRegistry>();
        public SpawnEntityControllerArchitecture()
        {
            EventBus.Subscribe<SpawnEntityRequestEvent>(RequestSpawnEntity);
        }
        public void Dispose()
        {
            EventBus.Unsubscribe<SpawnEntityRequestEvent>(RequestSpawnEntity);
        }

        private void RequestSpawnEntity(in SpawnEntityRequestEvent spawnEntityRequestEvent)
        {
            bool collides = false;
            foreach (Animal animal in EntityRegistry.FilterEntities<Animal>())
            {
                if (animal.coordinate.Origin == spawnEntityRequestEvent.coordinateToSpawn.Origin)
                {
                    collides = true;
                    break;
                }
            }

            if (collides)
            {
                EventBus.Raise<SpawnEntityRequestRejectedEvent>(
                    spawnEntityRequestEvent.blueprintToSpawn, spawnEntityRequestEvent.coordinateToSpawn);
            }
            else
            {
                EventBus.Raise<SpawnEntityRequestAceptedEvent>(
                    spawnEntityRequestEvent.blueprintToSpawn, spawnEntityRequestEvent.coordinateToSpawn);
            }
        }
    }
}
