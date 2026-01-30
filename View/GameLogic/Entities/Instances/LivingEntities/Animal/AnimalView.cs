using Architecture.GameLogic.Entities;
using System;

namespace ZooArchitect.View.GameLogic.Entities.Instances
{
    [ViewOf(typeof(Animal))]
    internal sealed class AnimalView : LivingEntityView 
    {
        public override Type ArchitectureEntityType => typeof(Animal);
    }
}
