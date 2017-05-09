namespace Milyli.ScriptRunner.Core.Logging
{
    using System.Linq;
    using NLog;
    using NLog.Config;
    using NLog.Targets;

    public static class LoggingBootstrapper
    {
#pragma warning disable CA1709
        public const string EventLogApplication = "Application";

        public const string EventLogDefaultTarget = EventLogApplication;

        public const string DefaultLayout = "${message}${newline}${newline}${exception:format=tostring}";
#pragma warning restore CA1709
        public const string EventLogTargetName = "milyli-scriptrunner-event-log";

        public const string MemoryTargetName = "milyli-scriptrunner-memotry";

        public static LoggingConfiguration EnsureConfiguration()
        {
            return LogManager.Configuration = LogManager.Configuration ?? new LoggingConfiguration();
        }

        public static void ConfigureEventLogTarget(string sourceName)
        {
            var configuration = EnsureConfiguration();
            var eventLogTarget = new EventLogTarget()
            {
                Layout = DefaultLayout,
                MachineName = ".",
                Source = sourceName,
                Log = EventLogDefaultTarget,
                Name = EventLogTargetName
            };
            configuration.AddTarget(eventLogTarget);
            FlushCurrentRules(EventLogTargetName);
            configuration.AddRule(LogLevel.Info, LogLevel.Fatal, EventLogTargetName);
            LogManager.ReconfigExistingLoggers();
        }

        public static MemoryTarget ConfigureMemoryTarget()
        {
            var configuration = EnsureConfiguration();
            var memoryTarget = new MemoryTarget()
            {
                Name = MemoryTargetName,
                Layout = DefaultLayout
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
