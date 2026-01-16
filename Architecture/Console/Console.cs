using Rexar.Architecture.Logs.Events;
using Rexar.Toolbox.Events;
using Rexar.Toolbox.Services;

namespace Rexar.Architecture.Logs
{
    public static class Console
    {
        private static EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();
        public static void Log(string message)
        {
            EventBus.Raise<ConsoleLogEvent>(message);
        }

        public static void Warning(string message)
        {
            EventBus.Raise<ConsoleWarningEvent>(message);
        }

        public static void Error(string message)
        {
            EventBus.Raise<ConsoleErrorEvent>(message);
        }
    }
}