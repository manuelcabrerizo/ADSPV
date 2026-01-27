using Architecture.GameLogic.Entities;
using System;

namespace ZooArchitect.View.GameLogic.Entities.Instances
{
    [ViewOf(typeof(Visitor))]
    internal sealed class VisitorView : HumanView 
    {
        public override Type ArchitectureEntityType => typeof(Visitor);

    }
}
