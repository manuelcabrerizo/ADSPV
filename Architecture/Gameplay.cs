using Rexar.Toolbox.Blueprint;
using Rexar.Toolbox.DataFlow;
using Rexar.Toolbox.Events;
using Rexar.Toolbox.Scheduling;
using Rexar.Toolbox.Services;
using System;
using ZooArchitect.Architecture.Controllers;
using ZooArchitect.Architecture.GameLogic;
using ZooArchitect.Architecture.GameLogic.Entities.Systems;

namespace ZooArchitect.Architecture
{
    public sealed class Gameplay : IInitable, ITickable, IDisposable
    {
        private TaskScheduler TaskScheduler => ServiceProvider.Instance.GetService<TaskScheduler>();
        private Time Time => ServiceProvider.Instance.GetService<Time>();

        private SpawnEntityControllerArchitecture spawnEntityControllerArchitecture;

        public Gameplay(string blueprintPath)
        {
            ServiceProvider.Instance.AddService<EventBus>(new EventBus());
            ServiceProvider.Instance.AddService<BlueprintRegistry>(new BlueprintRegistry(blueprintPath));
            ServiceProvider.Instance.AddService<BlueprintBinder>(new BlueprintBinder());
            ServiceProvider.Instance.AddService<TaskScheduler>(new TaskScheduler());
            spawnEntityControllerArchitecture = new SpawnEntityControllerArchitecture();
        }

        public void Init()
        {
            ServiceProvider.Instance.AddService<Time>(new Time());
            ServiceProvider.Instance.AddService<DayNightCycle>(new DayNightCycle());
            ServiceProvider.Instance.AddService<Wallet>(new Wallet());
            ServiceProvider.Instance.AddService<EntityRegistry>(new EntityRegistry());
            ServiceProvider.Instance.AddService<EntityFactory>(new EntityFactory());
        }

        public void LateInit()
        {
            new Map(10, 10);
        }

        public void Tick(float deltaTime)
        {
            Time.Tick(deltaTime);
            TaskScheduler.Tick(Time.LogicDeltaTime);
        }

        public void Dispose()
        {
            spawnEntityControllerArchitecture.Dispose();
        }
    }
}