namespace Rexar.View.Engine.Input
{
    public struct KeyState
    {
        public bool IsDown;
        public bool WasDown;

        public KeyState()
        { 
            IsDown = false;
            WasDown = false;
        }
    }
}
