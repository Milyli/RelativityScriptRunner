namespace Milyli.ScriptRunner.Core.Logging
{
    using System.Linq;
    using NLog;
    using NLog.Config;
    using NLog.Targets;

    public static class LoggingBootstrapper
    {
        public const string EVENT_LOG_APPLICATION = "Application";

        public const string EVENT_LOG_DEFAULT_TARGET = EVENT_LOG_APPLICATION;

        public const string DEFAULT_LAYOUT = "${message}${newline}${newline}${exception:format=tostring}";

        public const string EventLogTargetName = "milyli-scriptrunner-event-log";

        public const string MemoryTargetName = "milyli-scriptrunner-memotry";

        public static LoggingConfiguration EnsureCongifuration()
        {
            return LogManager.Configuration = LogManager.Configuration ?? new LoggingConfiguration();
        }

        public static void ConfigureEventLogTarget(string sourceName)
        {
            var configuration = EnsureCongifuration();
            var eventLogTarget = new EventLogTarget()
            {
                Layout = DEFAULT_LAYOUT,
                MachineName = ".",
                Source = sourceName,
                Log = EVENT_LOG_DEFAULT_TARGET,
                Name = EventLogTargetName
            };
            configuration.AddTarget(eventLogTarget);
            FlushCurrentRules(EventLogTargetName);
            configuration.AddRule(LogLevel.Info, LogLevel.Fatal, EventLogTargetName);
            LogManager.ReconfigExistingLoggers();
        }

        public static MemoryTarget ConfigureMemoryTarget()
        {
            var configuration = EnsureCongifuration();
            var memoryTarget = new MemoryTarget()
            {
                Name = MemoryTargetName,
                Layout = DEFAULT_LAYOUT
            };
            configuration.AddTarget(memoryTarget);
            FlushCurrentRules(MemoryTargetName);
            configuration.AddRule(LogLevel.Info, LogLevel.Fatal, MemoryTargetName);
            LogManager.ReconfigExistingLoggers();

            return memoryTarget;
        }

        private static void FlushCurrentRules(string targetName)
        {
            var existingRules = LogManager.Configuration.LoggingRules.FirstOrDefault(r => r.Targets.Any(t => t.Name.Equals(targetName)));
            if (existingRules != null)
            {
                LogManager.Configuration.LoggingRules.Remove(existingRules);
            }
        }
    }
}
