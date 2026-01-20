using OpenTK.Mathematics;

namespace ZooArchitect.View.Rendering
{
    class Transformation
    {
        private Matrix3 rotate;
        private Vector3 translate;
        private Vector3 scale;
        public Transformation()
        {
            this.rotate = Matrix3.Identity;
            this.translate = Vector3.Zero;
            this.scale = Vector3.One;
        }

        public void SetRotate(Matrix3 rotate)
        {
            this.rotate = rotate;
        }

        public void SetTranslate(Vector3 translate)
        {
            this.translate = translate;
        }

        public void SetScale(Vector3 scale)
        { 
            this.scale = scale;
        }

        public void Product(Transformation A, Transformation B)
        {
            rotate = A.rotate * B.rotate;
            translate = scale * (A.rotate * B.translate) + A.translate;
            scale = A.scale * B.scale;
        }
    }
}
