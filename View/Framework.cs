using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Rexar.Toolbox.Services;
using Rexar.View.Engine.Input;
using View.Engine.Rendering;

namespace ZooArchitect.View
{
    public sealed class Framework : GameWindow
    {
        private int windowWidth;
        private int windowHeight;

        private Engine Engine => ServiceProvider.Instance.GetService<Engine>();
        private Graphics Graphics => ServiceProvider.Instance.GetService<Graphics>();

        public Framework(int width, int height, string title)
            : base(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = (width, height), Title = title })
        {
            this.windowWidth  = width;
            this.windowHeight = height;
            ServiceProvider.Instance.AddService<Engine>(new Engine());
            ServiceProvider.Instance.AddService<Graphics>(new Graphics());
            CreateDefaultGameObjects();
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            Input.Init();
            Engine.Awake();
            Engine.Start();

            Graphics.SetProj(Matrix4.CreateOrthographic(windowWidth, windowHeight, 0.0f, 100.0f).Transposed());
            Graphics.SetView(Matrix4.CreateTranslation(0.0f, 0.0f, 0.0f).Transposed());
        }

        protected override void OnUnload()
        {
            base.OnUnload();

            Graphics.Dispose();
            Engine.Dispose();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            Graphics.DrawBegin();

            float deltaTime = (float)e.Time;
            Engine.Update(deltaTime);
            Input.Tick();

            Graphics.DrawEnd();

            SwapBuffers();
            base.OnRenderFrame(e);
        }

        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);
            Graphics.OnResize(e.Width, e.Height);
            Graphics.SetProj(Matrix4.CreateOrthographic((float)e.Width, (float)e.Height, 0.0f, 100.0f).Transposed());
        }

        private void CreateDefaultGameObjects()
        {
            GameObject view = Engine.Instantiate(null, Vector3.Zero);
            view.AddComponent<GameplayView>();
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            Input.SetKey(e.Key, true);
            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyboardKeyEventArgs e)
        {
            Input.SetKey(e.Key, false);
            base.OnKeyUp(e);
        }
    }
}
