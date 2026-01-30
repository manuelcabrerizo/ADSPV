using Rexar.Toolbox.DataFlow;
using System;

namespace ZooArchitect.View
{
    public abstract class ViewComponent : Component, IInitable, ITickable, IDisposable
    {
        public virtual void Init() { }
        public virtual void LateInit() { }
        public virtual void Tick(float deltaTime) { }
        public virtual void Dispose() { }
    }
}
