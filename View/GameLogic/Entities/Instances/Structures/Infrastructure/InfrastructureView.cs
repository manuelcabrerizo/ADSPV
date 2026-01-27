using Architecture.GameLogic.Entities;
using System;

namespace ZooArchitect.View.GameLogic.Entities.Instances
{
    [ViewOf(typeof(Infrastructure))]
    internal sealed class InfrastructureView : StructureView 
    {
        public override Type ArchitectureEntityType => typeof(Infrastructure);

    }
}
