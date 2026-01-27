using Architecture.GameLogic.Entities;
using System;

namespace ZooArchitect.View.GameLogic.Entities.Instances
{
    internal abstract class LivingEntityView : EntityView 
    {
        public override Type ArchitectureEntityType => typeof(LivingEntity);
        protected LivingEntityView(GameObject owner) : base(owner)
        {
        }
    }
}
