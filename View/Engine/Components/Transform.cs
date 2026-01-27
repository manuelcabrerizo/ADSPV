using OpenTK.Mathematics;

namespace ZooArchitect.View
{
    public sealed class Transform : Component
    {
        public Vector3 position;
        public Vector3 size;
        public float rotation;

        public Transform()
        {
        }

        public Transform(GameObject owner, Vector3 position, Vector3 size, float rotation)
        {
            this.position = position;
            this.size = size;
            this.rotation = rotation;
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