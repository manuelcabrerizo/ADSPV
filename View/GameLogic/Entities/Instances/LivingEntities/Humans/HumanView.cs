using Architecture.GameLogic.Entities;
using System;

namespace ZooArchitect.View.GameLogic.Entities.Instances
{
    internal abstract class HumanView : LivingEntityView
    {
        protected HumanView(GameObject owner) : base(owner)
        {
        }

        public override Type ArchitectureEntityType => typeof(Human);

    }
}
