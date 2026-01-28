using Rexar.Toolbox.Blueprint;
using Rexar.Toolbox.DataFlow;
using Rexar.Toolbox.Events;
using Rexar.Toolbox.Services;
using System;
using System.Collections.Generic;
using ZooArchitect.Architecture.Controllers.Events;
using ZooArchitect.Architecture.Data;
using ZooArchitect.Architecture.GameLogic.Math;

namespace ZooArchitect.Vew.Controller
{
    public enum KeyCode
    {
        Alpha1, Alpha2, Alpha3, Alpha4
    }

    public sealed class SpawnEntityControllerView : ITickable, IDisposable
    {
        int spawned = 0;


        private List<KeyCode> keys = new List<KeyCode>()
        {
            KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4
        };

        private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();
        private BlueprintRegistry BlueprintRegistry => ServiceProvider.Instance.GetService<BlueprintRegistry>();
        private List<string> animalsBlueprints;

        public SpawnEntityControllerView()
        { 
            animalsBlueprints = BlueprintRegistry.BlueprintsOf(TableNames.ANIMALS_TABLE_NAME);
            EventBus.Subscribe<SpawnEntityRequestRejectedEvent>(OnSpawnRejected);
        }

        public void Dispose()
        {
            EventBus.Unsubscribe<SpawnEntityRequestRejectedEvent>(OnSpawnRejected);
        }

        private void OnSpawnRejected(in SpawnEntityRequestRejectedEvent spawnEntityRequestRejectedEvent)
        {
            Console.Write($"Spawn of {spawnEntityRequestRejectedEvent.blueprintToSpawn} in {spawnEntityRequestRejectedEvent.coordinateToSpawn} rejected\n");
        }

        public void Tick(float deltaTime)
        {
            if (spawned < animalsBlueprints.Count)
            {
                for (int i = 0; i < animalsBlueprints.Count; i++)
                {
                    EventBus.Raise<SpawnEntityRequestEvent>(animalsBlueprints[i], new Coordinate(new Point(i * 100, 1)));
                    spawned++;
                    //if (Input.GetKeyDown(keys[i]))
                    //{
                    // Request
                    //}
                }
            }


        }
    }
}
