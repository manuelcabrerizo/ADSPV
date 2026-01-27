using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using Rexar.Toolbox.Services;
using System;
using ZooArchitect.View.Rendering;

namespace View.Rendering
{
    public struct PerPass
    {
        public Matrix4 View;
        public Matrix4 Proj;
    }

    public sealed class Graphics : IService, IDisposable
    {
        public bool IsPersistance => true;

        private PerPass perPass; 
        private SpriteBatch spriteBatch;

        public Graphics()
        {
            perPass = new PerPass();
            perPass.Proj = Matrix4.Identity;
            perPass.View = Matrix4.Identity;

            spriteBatch = new SpriteBatch(1024);

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Enable(EnableCap.DepthTest);
        }

        public void Dispose()
        {
            spriteBatch.Dispose();
        }

        public void DrawBegin()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        public void DrawEnd() 
        {
            spriteBatch.Render();
        }

        public void OnResize(int width, int height)
        {
            GL.Viewport(0, 0, width, height);
        }

        public void SetView(Matrix4 view)
        {
            perPass.View = view;
            Shader.UpdateUniformBuffer("PerPass", ref perPass);
        }

        public void SetProj(Matrix4 proj)
        {
            perPass.Proj = proj;
            Shader.UpdateUniformBuffer("PerPass", ref perPass);
        }

        public void DrawSprite(Vector3 position, Vector3 size, Vector3 color)
        {
            spriteBatch.Draw(position, size, color);
        }
    }
}
