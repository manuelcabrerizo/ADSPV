using Rexar.Toolbox.Services;
using System;
using ZooArchitect.Architecture.GameLogic.Entities;
using ZooArchitect.Architecture.GameLogic.Entities.Systems;

namespace ZooArchitect.View.GameLogic.Entities.Instances
{
    [ViewOf(typeof(Entity))]
    internal abstract class EntityView : ViewComponent
    {
        protected EntityRegistry EntityRegistry => ServiceProvider.Instance.GetService<EntityRegistry>();
        public abstract Type ArchitectureEntityType { get; }


        public static string SetIdMethodName = nameof(SetId);
        private void SetId(uint ID) 
        {
            architectureEntityID = ID;
        }

        protected uint architectureEntityID;
        public uint ArchitectureEntityID => architectureEntityID;
        protected Entity ArchitectureEntity => EntityRegistry.GetAs<Entity>(architectureEntityID);

    }
}
