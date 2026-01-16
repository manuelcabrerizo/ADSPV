namespace ZooArchitect.Architecture.GameLogic
{
    public struct DayStep
    {
        public string Name;
        public float Duration;

        public DayStep(string name, float duration)
        {
            Name = name;
            Duration = duration;
        }
    }
}
