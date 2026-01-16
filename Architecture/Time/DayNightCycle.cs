using Rexar.Toolbox.Events;
using Rexar.Toolbox.Services;
using Rexar.Architecture.GameLogic.Events;

namespace Rexar.Architecture.GameLogic
{
    public sealed class DayNightCycle : IService
    {
        private Toolbox.Scheduling.TaskScheduler TaskScheduler => ServiceProvider.Instance.GetService<Toolbox.Scheduling.TaskScheduler>();
        private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

        public bool IsPersistance => false;

        private const int DAY_DURATION = 24;
        private const int DAY_STEPS = 6;
        private const int DAY_STEP_DURATION = DAY_DURATION / DAY_STEPS;
        private const int HOUR_DURATION = 60;

        private readonly List<DayStep> daySteps;
        private int currentStep;

        public DayStep CurrentDayStep => daySteps[currentStep];

        public DayNightCycle()
        {
            currentStep = 0;
            daySteps = new List<DayStep>();
            daySteps.Add(new DayStep("Manana", DAY_STEP_DURATION));
            daySteps.Add(new DayStep("Mediodia", DAY_STEP_DURATION));
            daySteps.Add(new DayStep("Tarde", DAY_STEP_DURATION));
            daySteps.Add(new DayStep("Atardecer", DAY_STEP_DURATION));
            daySteps.Add(new DayStep("Anochecer", DAY_STEP_DURATION));
            daySteps.Add(new DayStep("Madrugada", DAY_STEP_DURATION));

            TaskScheduler.Schedule(ChangeStep, DAY_STEP_DURATION * HOUR_DURATION);
        }

        private void ChangeStep()
        {
            currentStep = (currentStep + 1) % daySteps.Count;
            TaskScheduler.Schedule(ChangeStep, DAY_STEP_DURATION * HOUR_DURATION);
            EventBus.Raise<DayStepChangeEvent>();
        }
    }
}
