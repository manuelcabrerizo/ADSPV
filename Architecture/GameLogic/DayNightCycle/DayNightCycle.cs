using Rexar.Toolbox.Blueprint;
using Rexar.Toolbox.Events;
using Rexar.Toolbox.Scheduling;
using Rexar.Toolbox.Services;
using System.Collections.Generic;
using ZooArchitect.Architecture.GameLogic.Events;

namespace ZooArchitect.Architecture.GameLogic
{
    public sealed class DayNightCycle : IService
    {
        private TaskScheduler TaskScheduler => ServiceProvider.Instance.GetService<TaskScheduler>();
        private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();
        private BlueprintBinder BlueprintBinder => ServiceProvider.Instance.GetService<BlueprintBinder>();

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

            object dayStep = new DayStep();
            BlueprintBinder.Apply(ref dayStep, "DayNightCycle", "Manana");
            daySteps.Add((DayStep)dayStep);
            BlueprintBinder.Apply(ref dayStep, "DayNightCycle", "Mediodia");
            daySteps.Add((DayStep)dayStep);
            BlueprintBinder.Apply(ref dayStep, "DayNightCycle", "Atardecer");
            daySteps.Add((DayStep)dayStep);
            BlueprintBinder.Apply(ref dayStep, "DayNightCycle", "Anochecer");
            daySteps.Add((DayStep)dayStep);
            BlueprintBinder.Apply(ref dayStep, "DayNightCycle", "Madrugada");
            daySteps.Add((DayStep)dayStep);

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
