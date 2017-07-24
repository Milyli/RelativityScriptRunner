namespace Milyli.ScriptRunner.Agent.Logging
{
	using Milyli.ScriptRunner.Agent.Agent;
	using Milyli.ScriptRunner.Core.Logging;
	using NLog;
	using NLog.Config;

	public class AgentLoggingBootstrapper
	{
		public const string AgentTargetName = "scriptrunner-agent";

		public static void ConfigureAgentTarget(AScriptRunnerAgent agent, int levelLimit, string targetName = AgentTargetName)
		{
			var configuration = LoggingBootstrapper.EnsureConfiguration();

			var agentTarget = new RelativityAgentNLogTarget(agent, levelLimit);
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

			var rule = new LoggingRule(targetName, logLevel, agentTarget);
			configuration.LoggingRules.Add(rule);

			LogManager.ReconfigExistingLoggers();
		}
	}
}
