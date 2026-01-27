using Architecture.GameLogic.Entities;
using System;

namespace ZooArchitect.View.GameLogic.Entities.Instances
{
    internal sealed class VisitorView : HumanView 
    {
        public VisitorView(GameObject owner) : base(owner)
        {
        }

        public override Type ArchitectureEntityType => typeof(Visitor);

    }
}
