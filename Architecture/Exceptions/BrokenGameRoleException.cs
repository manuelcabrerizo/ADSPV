using System;

namespace ZooArchitect.Architecture.Exceptions
{
    public sealed class BrokenGameRoleException : Exception
    {
        public BrokenGameRoleException(string message) : base(message)
        {
        }
    }

}
