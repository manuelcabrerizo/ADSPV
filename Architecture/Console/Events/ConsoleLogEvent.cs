using Rexar.Toolbox.Events;

namespace Rexar.Architecture.Logs.Events
{
    public struct ConsoleLogEvent : IEvent
    {
        public string Message;
        public void Assign(params object[] parameters)
        {
            Message = parameters[0] as string;
        }

        public void Reset()
        {
            Message = default(string);
        }
    }

}