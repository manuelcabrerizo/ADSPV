namespace Rexar.Toolbox.Pool
{
    public interface IResetteable
    {
        public void Assign(params object[] parameters);
        public void Reset();
    }
}
