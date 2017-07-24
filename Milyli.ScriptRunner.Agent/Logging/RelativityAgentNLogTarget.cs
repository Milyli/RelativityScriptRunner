namespace Milyli.ScriptRunner.Agent.Logging
{
	using System;
	using Milyli.ScriptRunner.Agent.Agent;
	using NLog;
	using NLog.Targets;

	/// <summary>
	/// Routes NLog log entries to Relativity Agent logging sink.
	/// </summary>
	[Target("RelativityAgent")]
	public class RelativityAgentNLogTarget : TargetWithLayout
	{
		/// <summary>
		/// The agent to pass log entries on to.
		/// </summary>
		private readonly AScriptRunnerAgent agent;

		/// <summary>
		/// The maximum Relativity agent log level.
		/// Relativity does this in reverse of all frameworks;
		/// that is the most critical is the smallest number.
		/// Error = 1
		/// Warn = 5
		/// Info = 10
		/// </summary>
		private readonly int levelLimit;

		/// <summary>
		/// Initializes a new instance of the <see cref="RelativityAgentNLogTarget"/> class.
		/// </summary>
		/// <param name="agent">The currently executing agent to write the messages to.</param>
		/// <param name="levelLimit">The log level assigned to the agent in Relativity.</param>
		public RelativityAgentNLogTarget(AScriptRunnerAgent agent, int levelLimit)
		{
			this.agent = agent;
			this.levelLimit = levelLimit;
		}

		/// <summary>
		/// Override of the NLog write method to write messages to Relativity event log.
		/// </summary>
		/// <param name="logEvent">The event to write.</param>
		protected override void Write(LogEventInfo logEvent)
		{
			string logMessage = logEvent.FormattedMessage;
			LogLevel logLevel = logEvent.Level;
			var logException = logEvent.Exception;

			if (logLevel == LogLevel.Error || logLevel == LogLevel.Fatal)
			{
				this.agent.RaiseLimitedError(logException, logMessage);
			}
			else if (logLevel == LogLevel.Warn)
			{
				this.agent.RaiseLimitedWarning(logException, logMessage);
			}
			else
			{
				if (logException != null)
				{
					logMessage = $"{logMessage} {Environment.NewLine} {logException.ToString()}";
				}

				this.agent.RaiseLimitedMessage(logMessage, 10);
			}
		}
	}
}
