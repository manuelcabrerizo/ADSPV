using Architecture.GameLogic.Entities;
using System;

namespace ZooArchitect.View.GameLogic.Entities.Instances
{
    [ViewOf(typeof(Structure))]
    internal abstract class StructureView : EntityView
    {
        public override Type ArchitectureEntityType => typeof(Structure);
    }
}