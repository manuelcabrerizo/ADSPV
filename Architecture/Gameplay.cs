using Rexar.Toolbox.Blueprint;
using Rexar.Toolbox.DataFlow;
using Rexar.Toolbox.Events;
using Rexar.Toolbox.Scheduling;
using Rexar.Toolbox.Services;
using System;
using ZooArchitect.Architecture.GameLogic;

namespace ZooArchitect.Architecture
{
    public sealed class Gameplay : IInitable, ITickable, IDisposable
    {
        private TaskScheduler TaskScheduler => ServiceProvider.Instance.GetService<TaskScheduler>();
        private Time Time => ServiceProvider.Instance.GetService<Time>();

        private Scene scene;

        public Gameplay(string blueprintPath)
        {
            ServiceProvider.Instance.AddService<EventBus>(new EventBus());
            ServiceProvider.Instance.AddService<BlueprintRegistry>(new BlueprintRegistry(blueprintPath));
            ServiceProvider.Instance.AddService<BlueprintBinder>(new BlueprintBinder());
            ServiceProvider.Instance.AddService<TaskScheduler>(new TaskScheduler());
            scene = new Scene();
        }

        public void Init()
        {
            scene.Init();
        }

        public void LateInit()
        {
            scene.LateInit();
        }

        public void Tick(float deltaTime)
        {
            Time.Tick(deltaTime);
            TaskScheduler.Tick(Time.LogicDeltaTime);
            scene.Tick(deltaTime);
        }

        public void Dispose()
        {
            scene.Dispose();
        }
    }
}