using ZooArchitect.Architecture;
using ZooArchitect.View.Rendering;
using ZooArchitect.View.Logs;

using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;


namespace ZooArchitect.View
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
        private Gameplay gameplay;
        private ConsoleView consoleView;

        // TODO: move all this to a Rendering system
        private Shader shader;
        private Texture texture;
        private Model model;
        float time = 0.0f;

        public GameplayView(int width, int height, string title) 
            : base(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = (width, height), Title = title }) 
        {
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            gameplay = new Gameplay();
            consoleView = new ConsoleView();
            
            shader = new Shader("../Assets/Shaders/shader.vert", "../Assets/Shaders/shader.frag");
            texture = new Texture("../Assets/Models/werewolf/bakedtexture.png");
            model = new Model("../Assets/Models/werewolf/werewolffbx.fbx");

            shader.Bind();
            shader.SetInt("texture0", 0);

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            
            PerPass perPass = new PerPass();
            perPass.Proj = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), 800.0f / 600.0f, 0.1f, 1000.0f).Transposed();
            perPass.View = Matrix4.CreateTranslation(0.0f, -50.0f, -150.0f).Transposed();
            shader.UpdateUniformBuffer("PerPass", ref perPass);
            
            GL.Enable(EnableCap.DepthTest);
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            
            model.Dispose();
            texture.Dispose();
            shader.Dispose();
            consoleView.Dispose();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            if(KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            float deltaTime = (float)e.Time;

            gameplay.Update(deltaTime);

            time += deltaTime * 100.0f;

            PerDraw perDraw = new PerDraw();
            perDraw.World = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(-90.0f)) * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(time));
            perDraw.World.Transpose();
            perDraw.Color = new Vector4(0, 1, 0, 1);
            shader.UpdateUniformBuffer("PerDraw", ref perDraw);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            texture.Bind(TextureUnit.Texture0);
            shader.Bind();

            model.Draw();
            
            SwapBuffers();

            base.OnRenderFrame(e);
        }

        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
            PerPass perPass = new PerPass();
            perPass.Proj = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45.0f), (float)e.Width / (float)e.Height, 0.1f, 1000.0f).Transposed();
            perPass.View = Matrix4.CreateTranslation(0.0f, -50.0f, -150.0f).Transposed();
            shader.UpdateUniformBuffer("PerPass", ref perPass);
        }
    }
}
