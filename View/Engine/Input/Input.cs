using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Rexar.View.Engine.Input
{
    public static class Input
    {
        private static KeyState[] keys;

        public static void Init()
        {
            keys = new KeyState[(int)Keys.LastKey];
        }
        public static bool GetKey(Keys keyCode)
        {
            return keys[(int)keyCode].IsDown;
        }
        public static bool GetKeyDown(Keys keyCode)
        {
            return keys[(int)keyCode].IsDown && !keys[(int)keyCode].WasDown;
        }

        public static void SetKey(Keys keyCode, bool down)
        {
            keys[(int)keyCode].IsDown = down;
        }

        public static void Tick()
        {
            for(int i = 0; i < keys.Length; i++)
            {
                keys[i].WasDown = keys[i].IsDown;
            }
        }
    }
}
