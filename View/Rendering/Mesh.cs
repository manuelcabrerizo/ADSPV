using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;

namespace ZooArchitect.View.Rendering
{
    public sealed class Mesh : IDisposable
    {
        private int vbo;
        private int vao;
        private int ebo;
        private readonly List<Vertex> vertices;
        private readonly List<int> indices;

        public Mesh(List<Vertex> vertices, List<int> indices)
        {
            this.vertices = vertices;
            this.indices = indices;

            int vertexSize = 14 * sizeof(float);

            vao = GL.GenVertexArray();
            GL.BindVertexArray(vao);

            vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, this.vertices.Count * vertexSize, vertices.ToArray(), BufferUsageHint.StaticDraw);

            ebo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, this.indices.Count * sizeof(int), this.indices.ToArray(), BufferUsageHint.StaticDraw);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, vertexSize, 0);
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, vertexSize, 3 * sizeof(float));
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, vertexSize, 6 * sizeof(float));
            GL.EnableVertexAttribArray(3);
            GL.VertexAttribPointer(3, 3, VertexAttribPointerType.Float, false, vertexSize, 9 * sizeof(float));
            GL.EnableVertexAttribArray(4);
            GL.VertexAttribPointer(4, 2, VertexAttribPointerType.Float, false, vertexSize, 12 * sizeof(float));
        }

        public void Dispose()
        {
            GL.DeleteBuffer(ebo);
            GL.DeleteBuffer(vbo);
            GL.DeleteVertexArray(vao);
        }

        public void Draw()
        {
            GL.BindVertexArray(vao);
            GL.DrawElements(PrimitiveType.Triangles, indices.Count, DrawElementsType.UnsignedInt, 0);
        }
    }
}
