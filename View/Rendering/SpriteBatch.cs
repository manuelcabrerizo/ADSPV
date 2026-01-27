using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using ZooArchitect.View.Rendering;

namespace View.Rendering
{
    public struct BatchVertex
    {
        public Vector3 Pos;
        public Vector3 Col;
        public Vector2 Uvs;
        public float   Tex;
    }

    public class SpriteBatch : IDisposable
    {
        private const int vertexSize = 9 * sizeof(float);

        private int vao;
        private int vbo;
        private int ebo;
        private int batchSize;
        private int used;
        private Shader shader;


        public SpriteBatch(int batchSize) 
        {
            shader = new Shader("../Assets/Shaders/batch.vert", "../Assets/Shaders/batch.frag");

            this.batchSize = batchSize;
            this.used = 0;

            vao = GL.GenVertexArray();
            GL.BindVertexArray(vao);

            vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, batchSize * vertexSize * 6, (nint)null, BufferUsageHint.DynamicDraw);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, vertexSize, 0);
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, vertexSize, 3 * sizeof(float));
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, vertexSize, 6 * sizeof(float));
            GL.EnableVertexAttribArray(3);
            GL.VertexAttribPointer(3, 1, VertexAttribPointerType.Float, false, vertexSize, 8 * sizeof(float));

            ebo = GL.GenBuffer();
        }

        public void Draw(Vector3 position, Vector3 size, Vector3 color)
        {
            // TODO: remove this quad from here
            BatchVertex[] quad = new BatchVertex[6]
            {
                new BatchVertex { Pos = new Vector3(-0.5f,  0.5f, 0f), Col = new Vector3(1, 1, 1), Uvs = new Vector2(0, 1), Tex = 0f },
                new BatchVertex { Pos = new Vector3( 0.5f,  0.5f, 0f), Col = new Vector3(1, 1, 1), Uvs = new Vector2(1, 1), Tex = 0f },
                new BatchVertex { Pos = new Vector3( 0.5f, -0.5f, 0f), Col = new Vector3(1, 1, 1), Uvs = new Vector2(1, 0), Tex = 0f },
                new BatchVertex { Pos = new Vector3( 0.5f, -0.5f, 0f), Col = new Vector3(1, 1, 1), Uvs = new Vector2(1, 0), Tex = 0f },
                new BatchVertex { Pos = new Vector3(-0.5f, -0.5f, 0f), Col = new Vector3(1, 1, 1), Uvs = new Vector2(0, 0), Tex = 0f },
                new BatchVertex { Pos = new Vector3(-0.5f,  0.5f, 0f), Col = new Vector3(1, 1, 1), Uvs = new Vector2(0, 1), Tex = 0f }
            };


            if (used + 1 >= batchSize)
            {
                Render();
            }
            GL.BindVertexArray(vao);
            for (int i = 0; i < quad.Length; i++)
            {
                Matrix4 world = Matrix4.CreateScale(size) * Matrix4.CreateTranslation(position);
                quad[i].Pos = new Vector3(world.Transposed() * new Vector4(quad[i].Pos, 1.0f));
                quad[i].Col = color;
            }
            GL.BufferSubData(BufferTarget.ArrayBuffer, used * vertexSize * quad.Length, vertexSize * quad.Length, quad);
            GL.BindVertexArray(0);
            used++;
        }

        public void Render()
        {
            shader.Bind();
            GL.BindVertexArray(vao);
            GL.DrawArrays(PrimitiveType.Triangles, 0, used * 6);
            GL.BindVertexArray(0);
            used = 0;
        }

        public void Dispose()
        {
            shader.Dispose();
            GL.DeleteBuffer(ebo);
            GL.DeleteBuffer(vbo);
            GL.DeleteVertexArray(vao);
        }
    }
}
