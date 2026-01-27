using Architecture.GameLogic.Entities;
using System;

namespace ZooArchitect.View.GameLogic.Entities.Instances
{
    [ViewOf(typeof(Human))]
    internal abstract class HumanView : LivingEntityView
    {
        public override Type ArchitectureEntityType => typeof(Human);

    }
}
