using Rexar.Toolbox.Events;
using Rexar.Toolbox.Services;
using Rexar.Toolbox.Updateable;

namespace Rexar.Architecture
{
    public sealed class Gameplay : IUpdateable
    {
        private EventBus? EventBus => ServiceProvider.Instance.GetService<EventBus>();

        public Gameplay()
        {
            ServiceProvider.Instance.AddService<EventBus>(new EventBus());
        }

        public void Update(float deltaTime)
        {
            
        }
    }
}
