using Architecture.GameLogic.Entities;
using System;

namespace ZooArchitect.View.GameLogic.Entities.Instances
{
    internal sealed class AnimalView : LivingEntityView 
    {
        public AnimalView(GameObject owner) : base(owner)
        {
        }

        public override Type ArchitectureEntityType => typeof(Animal);

    }
}
