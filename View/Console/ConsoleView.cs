using Rexar.Toolbox.Events;
using Rexar.Toolbox.Services;
using System;
using ZooArchitect.Architecture.Logs.Events;

namespace ZooArchitect.View.Logs
{
    public sealed class ConsoleView : IDisposable
    {
        private static EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

        public ConsoleView() 
        {
            EventBus.Subscribe<ConsoleLogEvent>(LogMessage);
            EventBus.Subscribe<ConsoleWarningEvent>(LogWarning);
            EventBus.Subscribe<ConsoleErrorEvent>(LogError);
        }

        private void LogMessage(in ConsoleLogEvent consolLogEvent)
        {
            Console.WriteLine("Log: " + consolLogEvent.Message);
        }

        private void LogWarning(in ConsoleWarningEvent consoleWarningEvent)
        {
            Console.WriteLine("Warning: " + consoleWarningEvent.Message);
        }

        private void LogError(in ConsoleErrorEvent consoleErrorEvent)
        {
            Console.WriteLine("Error: " + consoleErrorEvent.Message);
        }

        public void Dispose()
        {
            EventBus.Unsubscribe<ConsoleLogEvent>(LogMessage);
            EventBus.Unsubscribe<ConsoleWarningEvent>(LogWarning);
            EventBus.Unsubscribe<ConsoleErrorEvent>(LogError);
        }
    }
}
