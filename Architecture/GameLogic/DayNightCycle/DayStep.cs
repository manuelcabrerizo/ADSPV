using Rexar.Toolbox.Blueprint;

namespace ZooArchitect.Architecture.GameLogic
{
    public struct DayStep
    {
        [BlueprintParameter("Name")] public string Name;
        [BlueprintParameter("Duration")] public float Duration;

        public DayStep(string name, float duration)
        {
            Name = name;
            Duration = duration;
        }
    }
}
