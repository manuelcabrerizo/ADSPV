using Architecture.GameLogic.Entities;
using System;

namespace ZooArchitect.View.GameLogic.Entities.Instances
{
    [ViewOf(typeof(Jail))]
    internal sealed class JailView : StructureView
    {
        public override Type ArchitectureEntityType => typeof(Jail);
    }
}
