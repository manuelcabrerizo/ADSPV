using Architecture.GameLogic.Entities;
using System;

namespace ZooArchitect.View.GameLogic.Entities.Instances
{
    [ViewOf(typeof(LivingEntity))]
    internal abstract class LivingEntityView : EntityView 
    {
        public override Type ArchitectureEntityType => typeof(LivingEntity);
    }
}
