using OpenTK.Mathematics;

namespace ZooArchitect.View
{
    public sealed class Transform : Component
    {
        public Transform parent { get; set; }
        public Vector3 position;
        public Vector3 size;
        public float rotation;

        public override void OnDisable()
        {
        }

        public override void Copy(Component component)
        {
            Transform transform = component as Transform;
            position = transform.position;
            size = transform.size;
            rotation = transform.rotation;
        }
    }
}