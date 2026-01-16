using Rexar.Toolbox.Events;
using Rexar.Toolbox.Services;
using Rexar.Toolbox.Updateable;
using Rexar.Toolbox.Scheduling;
using Rexar.Architecture.Logs;
using Rexar.Architecture.GameLogic;

namespace Rexar.Architecture
{
    public sealed class Gameplay : IUpdateable
    {
        private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();
        private Toolbox.Scheduling.TaskScheduler TaskScheduler => ServiceProvider.Instance.GetService<Toolbox.Scheduling.TaskScheduler>();
        private Time Time => ServiceProvider.Instance.GetService<Time>();
        private DayNightCycle DayNightCycle => ServiceProvider.Instance.GetService<DayNightCycle>();

        public Gameplay()
        {
            ServiceProvider.Instance.AddService<EventBus>(new EventBus());
            ServiceProvider.Instance.AddService<Toolbox.Scheduling.TaskScheduler>(new Toolbox.Scheduling.TaskScheduler());
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
