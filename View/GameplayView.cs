using Rexar.Toolbox.Events;
using Rexar.Toolbox.Services;
using Rexar.Architecture;
using Rexar.View.Rendering;

using System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;


namespace Rexar.View
{
    public struct PerPass
    {
        public Matrix4 View;
        public Matrix4 Proj;
    }

    public struct PerDraw
    {
        public Matrix4 World;
        public Vector4 Color;
    }

    public class GameplayView : GameWindow
    {
        private EventBus? EventBus => ServiceProvider.Instance.GetService<EventBus>();
        
        private Gameplay? gameplay = null;
        private ConsoleView consoleView;

        // TODO: move all this to a Rendering system
        private Shader? mShader = null;
        private Texture? mTexture0 = null;
        private Texture? mTexture1 = null;
        private int mVAO;
        private int mVBO;
        private int mEBO;
        private float[] mVertices = {
            //Position          Texture coordinates
             0.5f,  0.5f, 0.0f, 1.0f, 1.0f, // top right
             0.5f, -0.5f, 0.0f, 1.0f, 0.0f, // bottom right
            -0.5f, -0.5f, 0.0f, 0.0f, 0.0f, // bottom left
            -0.5f,  0.5f, 0.0f, 0.0f, 1.0f  // top left
        };
        uint[] mIndices = {  // note that we start from 0!
            0, 1, 3,   // first triangle
            1, 2, 3    // second triangle
        };

        public GameplayView(int width, int height, string title) 
            : base(GameWindowSettings.Default, new NativeWindowSettings() { Size = (width, height), Title = title }) 
        {
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            gameplay = new Gameplay();
            consoleView = new ConsoleView();
            

            mShader = new Shader("../Assets/Shaders/shader.vert", "../Assets/Shaders/shader.frag");
            mTexture0 = new Texture("../Assets/Textures/tiles_floor_5.png");
            mTexture1 = new Texture("../Assets/Textures/stone_hell_10.png");

            mShader?.Bind();
            mShader?.SetInt("texture0", 0);
            mShader?.SetInt("texture1", 1);

            mVAO = GL.GenVertexArray();
            GL.BindVertexArray(mVAO);

            mVBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, mVBO);
            GL.BufferData(BufferTarget.ArrayBuffer, mVertices.Length * sizeof(float), mVertices, BufferUsageHint.StaticDraw);

            mEBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, mEBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, mIndices.Length * sizeof(uint), mIndices, BufferUsageHint.StaticDraw);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            
            PerPass perPass = new PerPass();
            perPass.Proj = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), 800.0f / 600.0f, 0.1f, 100.0f).Transposed();
            perPass.View = Matrix4.CreateTranslation(0.0f, 0.0f, -2.0f).Transposed();
            PerDraw perDraw = new PerDraw();
            perDraw.World = Matrix4.Identity;
            perDraw.Color = new Vector4(0, 1, 0, 1);

            mShader?.SetMatrix4("model", ref perDraw.World);
            mShader?.SetMatrix4("view", ref perPass.View);
            mShader?.SetMatrix4("proj", ref perPass.Proj);
            mShader?.UpdateUniformBuffer("PerPass", ref perPass);
            mShader?.UpdateUniformBuffer("PerDraw", ref perDraw);
            
            GL.Enable(EnableCap.DepthTest);
        }

        protected override void OnUnload()
        {
            base.OnUnload();

            mShader?.Dispose();
            consoleView.Dispose();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            if(KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }
            gameplay?.Update((float)e.Time);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            mTexture0?.Bind(TextureUnit.Texture0);
            mTexture1?.Bind(TextureUnit.Texture1);
            mShader?.Bind();
            GL.BindVertexArray(mVAO);

            GL.DrawElements(PrimitiveType.Triangles, mIndices.Length, DrawElementsType.UnsignedInt, 0);

            SwapBuffers();

            base.OnRenderFrame(e);
        }

        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
        }
    }
}
