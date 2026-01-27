using Rexar.Toolbox.Services;
using System;
using ZooArchitect.Architecture.GameLogic.Entities;
using ZooArchitect.Architecture.GameLogic.Entities.Systems;

namespace ZooArchitect.View.GameLogic.Entities.Instances
{
    internal abstract class EntityView : ViewComponent
    {
        protected EntityRegistry EntityRegistry => ServiceProvider.Instance.GetService<EntityRegistry>();
        protected uint architectureEntityID;

        public abstract Type ArchitectureEntityType { get; }
        public uint ArchitectureEntityID => architectureEntityID;
        protected Entity ArchitectureEntity => EntityRegistry.GetAs<Entity>(architectureEntityID);

        protected EntityView(GameObject owner) : base(owner)
        {
        }
    }
}
