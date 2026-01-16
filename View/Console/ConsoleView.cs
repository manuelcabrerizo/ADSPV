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

        private void LogMessage(ConsoleLogEvent consolLogEvent)
        {
            Console.WriteLine(consolLogEvent.Message);
        }

        private void LogWarning(ConsoleWarningEvent consoleWarningEvent)
        {
            Console.WriteLine(consoleWarningEvent.Message);
        }

        private void LogError(ConsoleErrorEvent consoleErrorEvent)
        {
            Console.WriteLine(consoleErrorEvent.Message);
        }

        public void Dispose()
        {
            EventBus.Unsubscribe<ConsoleLogEvent>(LogMessage);
            EventBus.Unsubscribe<ConsoleWarningEvent>(LogWarning);
            EventBus.Unsubscribe<ConsoleErrorEvent>(LogError);
        }
    }
}
