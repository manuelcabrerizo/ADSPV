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

        private Transform transform;

        public Sprite()
        {
        }

        public Sprite(GameObject owner, Vector3 color)
        {
            this.color = color;
        }

        public override void Init()
        {
            transform = GetComponent<Transform>();
        }

        public override void Tick(float deltaTime)
        {
            Graphics.DrawSprite(transform.position, transform.size, color);
        }

        public override void OnDisable()
        {
        }

        public override void Copy(Component component)
        {
            Sprite sprite = component as Sprite;
            color = sprite.color;
        }
    }
}