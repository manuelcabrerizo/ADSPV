using Architecture.GameLogic.Entities;
using System;

namespace ZooArchitect.View.GameLogic.Entities.Instances
{
    internal abstract class StructureView : EntityView
    {
        protected StructureView(GameObject owner) : base(owner)
        {
        }

        public override Type ArchitectureEntityType => typeof(Structure);
    }
}