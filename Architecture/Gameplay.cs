using Architecture.GameLogic.Entities;
using Rexar.Toolbox.Blueprint;
using Rexar.Toolbox.DataFlow;
using Rexar.Toolbox.Events;
using Rexar.Toolbox.Scheduling;
using Rexar.Toolbox.Services;
using ZooArchitect.Architecture.GameLogic;
using ZooArchitect.Architecture.GameLogic.Entities.Systems;
using ZooArchitect.Architecture.GameLogic.Math;

namespace ZooArchitect.Architecture
{
    public sealed class Gameplay : IInitable, ITickable
    {
        private TaskScheduler TaskScheduler => ServiceProvider.Instance.GetService<TaskScheduler>();
        private Time Time => ServiceProvider.Instance.GetService<Time>();
        private EntityFactory EntityFactory => ServiceProvider.Instance.GetService<EntityFactory>();


        public Gameplay(string blueprintPath)
        {
            ServiceProvider.Instance.AddService<EventBus>(new EventBus());
            ServiceProvider.Instance.AddService<BlueprintRegistry>(new BlueprintRegistry(blueprintPath));
            ServiceProvider.Instance.AddService<BlueprintBinder>(new BlueprintBinder());
            ServiceProvider.Instance.AddService<TaskScheduler>(new TaskScheduler());
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
            EntityFactory.CreateInstance<Animal>("Monkey", new Coordinate(new Point(0, 0)));
        }

        public void Tick(float deltaTime)
        {
            Time.Tick(deltaTime);
            TaskScheduler.Tick(Time.LogicDeltaTime);
        }
    }
}