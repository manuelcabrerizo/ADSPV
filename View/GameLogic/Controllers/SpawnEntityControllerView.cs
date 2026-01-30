using OpenTK.Windowing.GraphicsLibraryFramework;
using Rexar.Toolbox.Blueprint;
using Rexar.Toolbox.DataFlow;
using Rexar.Toolbox.Events;
using Rexar.Toolbox.Services;
using Rexar.View.Engine.Input;
using System;
using System.Collections.Generic;
using ZooArchitect.Architecture.Controllers.Events;
using ZooArchitect.Architecture.Data;
using ZooArchitect.Architecture.GameLogic.Math;
using ZooArchitect.Architecture.Logs;

namespace ZooArchitect.Vew.Controller
{
    public sealed class SpawnEntityControllerView : ITickable, IDisposable
    {
        private List<Keys> keys = new List<Keys>()
        {
            Keys.D1, Keys.D2, Keys.D3, Keys.D4
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

        public void Tick(float deltaTime)
        {
            for (int i = 0; i < animalsBlueprints.Count; i++)
            {
                if (Input.GetKeyDown(keys[i]))
                {
                    EventBus.Raise<SpawnEntityRequestEvent>(animalsBlueprints[i], new Coordinate(new Point(i * 20, i * 20)));
                }
            }
        }

        private void OnSpawnRejected(in SpawnEntityRequestRejectedEvent spawnEntityRequestRejectedEvent)
        {
            GameConsole.Warning($"Spawn of {spawnEntityRequestRejectedEvent.blueprintToSpawn} in {spawnEntityRequestRejectedEvent.coordinateToSpawn} rejected\n");
        }
    }
}
