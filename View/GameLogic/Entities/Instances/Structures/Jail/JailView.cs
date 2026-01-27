using Architecture.GameLogic.Entities;
using System;

namespace ZooArchitect.View.GameLogic.Entities.Instances
{
    internal sealed class JailView : StructureView
    {
        public JailView(GameObject owner) : base(owner)
        {
        }

        public override Type ArchitectureEntityType => typeof(Jail);

    }
}
