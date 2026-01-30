using System;

namespace View.Exceptions
{
    internal class ParameterlessConstructorNotFoundException : Exception
    {
        public ParameterlessConstructorNotFoundException(string message) : base(message)
        {
        }
    }
}
