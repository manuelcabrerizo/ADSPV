using Rexar.Toolbox.Events;
using Rexar.Toolbox.Services;
using Rexar.Toolbox.Updateable;
using Rexar.Toolbox.Scheduling;
using ZooArchitect.Architecture.Logs;
using ZooArchitect.Architecture.GameLogic;

namespace ZooArchitect.Architecture
{
    public sealed class Gameplay : IUpdateable
    {
        private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();
        private TaskScheduler TaskScheduler => ServiceProvider.Instance.GetService<TaskScheduler>();
        private Time Time => ServiceProvider.Instance.GetService<Time>();
        private DayNightCycle DayNightCycle => ServiceProvider.Instance.GetService<DayNightCycle>();

        public Gameplay()
        {
            ServiceProvider.Instance.AddService<EventBus>(new EventBus());
            ServiceProvider.Instance.AddService<TaskScheduler>(new TaskScheduler());
            ServiceProvider.Instance.AddService<Time>(new Time());
            ServiceProvider.Instance.AddService<DayNightCycle>(new DayNightCycle());
            ServiceProvider.Instance.AddService<Wallet>(new Wallet());
        }

        public void Update(float deltaTime)
        {
            Time.Update(deltaTime);
            TaskScheduler.Update(Time.LogicDeltaTime);
        }
    }
}
