using OpenTK.Mathematics;
using Rexar.Toolbox.DataFlow;
using Rexar.Toolbox.Services;
using View.Engine.Rendering;

namespace ZooArchitect.View
{
    public sealed class Sprite : Component
    {
        private Graphics Graphics => ServiceProvider.Instance.GetService<Graphics>();

        public Vector3 color;

        public override void Update(float deltaTime)
        {
            Vector3 position = transform.position;
            Transform parent = transform.parent;
            while (parent != null)
            {
                position += parent.position;
                parent = parent.parent;
            }
            Graphics.DrawSprite(position, transform.size, color);
        }

        public override void Copy(Component component)
        {
            Sprite sprite = component as Sprite;
            color = sprite.color;
        }
    }
}