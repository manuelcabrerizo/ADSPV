using Rexar.Toolbox.Blueprint;
using Rexar.Toolbox.Events;
using Rexar.Toolbox.Scheduling;
using Rexar.Toolbox.Services;
using System.Collections.Generic;
using ZooArchitect.Architecture.Data;
using ZooArchitect.Architecture.GameLogic.Events;

namespace ZooArchitect.Architecture.GameLogic
{
    public sealed class DayNightCycle : IService
    {
        public bool IsPersistance => false;
        private TaskScheduler TaskScheduler => ServiceProvider.Instance.GetService<TaskScheduler>();
        private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();
        private BlueprintBinder BlueprintBinder => ServiceProvider.Instance.GetService<BlueprintBinder>();
        private BlueprintRegistry BlueprintRegistry => ServiceProvider.Instance.GetService<BlueprintRegistry>();

        private readonly List<DayStep> daySteps;
        private int currentStep;

        public DayStep CurrentDayStep => daySteps[currentStep];

        public DayNightCycle()
        {
            currentStep = 0;
            daySteps = new List<DayStep>();

            object dayStep = new DayStep();
            foreach (string dayStepId in BlueprintRegistry.BlueprintsOf(TableNames.DAY_NIGHT_CYCLE_TABLE_NAME))
            {
                BlueprintBinder.Apply(ref dayStep, TableNames.DAY_NIGHT_CYCLE_TABLE_NAME, dayStepId);
                daySteps.Add((DayStep)dayStep);
            }

            TaskScheduler.Schedule(ChangeStep, CurrentDayStep.Duration);
        }

        private void ChangeStep()
        {
            currentStep = (currentStep + 1) % daySteps.Count;
            TaskScheduler.Schedule(ChangeStep, CurrentDayStep.Duration);
            EventBus.Raise<DayStepChangeEvent>();
        }
    }
}
