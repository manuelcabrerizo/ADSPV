using Architecture.GameLogic.Entities;
using System;

namespace ZooArchitect.View.GameLogic.Entities.Instances
{
    [ViewOf(typeof(Worker))]
    internal sealed class WorkerView : HumanView
    {
        public override Type ArchitectureEntityType => typeof(Worker);

    }
}
