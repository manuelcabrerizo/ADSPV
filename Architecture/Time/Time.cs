using Rexar.Toolbox.Services;
using Rexar.Toolbox.DataFlow;

namespace ZooArchitect.Architecture.GameLogic
{
    public sealed class Time : IService, ITickable
    {
        public bool IsPersistance => false;

        private float timeMultiplier;
        private float lastDeltaTime;
        public float LogicDeltaTime => lastDeltaTime * timeMultiplier;

        public Time()
        {
            timeMultiplier = 1.0f;
        }

        public void Tick(float deltaTime)
        {
            lastDeltaTime = deltaTime;
        }
    }
}
