using Architecture.GameLogic.Entities;
using System;

namespace ZooArchitect.View.GameLogic.Entities.Instances
{
    internal sealed class InfrastructureView : StructureView 
    {
        public InfrastructureView(GameObject owner) : base(owner)
        {
        }

        public override Type ArchitectureEntityType => typeof(Infrastructure);

    }
}
