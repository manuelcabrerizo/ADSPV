using Architecture.GameLogic.Entities;
using System;

namespace ZooArchitect.View.GameLogic.Entities.Instances
{
    internal sealed class WorkerView : HumanView
    {
        public WorkerView(GameObject owner) : base(owner)
        {
        }

        public override Type ArchitectureEntityType => typeof(Worker);

    }
}
