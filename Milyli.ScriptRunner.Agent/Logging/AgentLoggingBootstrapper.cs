namespace Milyli.ScriptRunner.Agent.Logging
{
	using Milyli.ScriptRunner.Agent.Agent;
	using Milyli.ScriptRunner.Core.Logging;
	using NLog;
	using NLog.Config;

	public static class AgentLoggingBootstrapper
	{
		public const string AgentTargetName = "scriptrunner-agent";

		public static void ConfigureAgentTarget(AScriptRunnerAgent agent, int levelLimit, string targetName = AgentTargetName)
		{
			var agentTarget = new RelativityAgentNLogTarget(agent, levelLimit);
			var configuration = LoggingBootstrapper.EnsureConfiguration();
			configuration.AddTarget(AgentTargetName, agentTarget);

			LogLevel logLevel;
			if (levelLimit >= 10)
			{
				logLevel = LogLevel.Info;
			}
			else if (levelLimit >= 5)
			{
				logLevel = LogLevel.Warn;
			}
			else if (levelLimit >= 1)
			{
				logLevel = LogLevel.Error;
			}
			else
			{
				logLevel = LogLevel.Off;
			}

			configuration.AddRule(logLevel, LogLevel.Fatal, targetName);

			LogManager.ReconfigExistingLoggers();
		}
	}
}
