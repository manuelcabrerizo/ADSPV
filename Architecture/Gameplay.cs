using Rexar.Toolbox.Events;
using Rexar.Toolbox.Scheduling;
using Rexar.Toolbox.Services;
using Rexar.Toolbox.Updateable;
using ZooArchitect.Architecture.GameLogic;
using ZooArchitect.Architecture.GameLogic.Entities.Systems;

namespace ZooArchitect.Architecture
{
    public sealed class Gameplay : IUpdateable
    {
        private TaskScheduler TaskScheduler => ServiceProvider.Instance.GetService<TaskScheduler>();
        private Time Time => ServiceProvider.Instance.GetService<Time>();



        public Gameplay()
        {
            ServiceProvider.Instance.AddService<EventBus>(new EventBus());
            ServiceProvider.Instance.AddService<TaskScheduler>(new TaskScheduler());
            ServiceProvider.Instance.AddService<Time>(new Time());
            ServiceProvider.Instance.AddService<DayNightCycle>(new DayNightCycle());
            ServiceProvider.Instance.AddService<Wallet>(new Wallet());
            ServiceProvider.Instance.AddService<EntityRegistry>(new EntityRegistry());
            ServiceProvider.Instance.AddService<EntityFactory>(new EntityFactory());
        }

        public void Update(float deltaTime)
        {
            Time.Update(deltaTime);
            TaskScheduler.Update(Time.LogicDeltaTime);
        }
    }
}