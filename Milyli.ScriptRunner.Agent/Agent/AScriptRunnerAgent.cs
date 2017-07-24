// <copyright file="AScriptRunnerAgent.cs" company="Milyli">
// Copyright © 2016 Milyli
// </copyright>

namespace Milyli.ScriptRunner.Agent.Agent
{
	using System;
	using System.Globalization;
	using Milyli.ScriptRunner.Agent.DependencyResolution;
	using Milyli.ScriptRunner.Agent.Logging;
	using Milyli.ScriptRunner.Core.Logging;
	using NLog;
	using NLog.Config;
	using Relativity.API;
	using StructureMap;

	public abstract class AScriptRunnerAgent : kCura.Agent.AgentBase
	{
		private const int Logmessagecharlimit = 2000; // 2000;

		private const int Logdetailcharlimit = 30000; // 32766; // leave some room before the absolute 32766 char limit for formatting

		public override void Execute()
		{
			var agentName = this.Name.Replace(" ", string.Empty);
			kCura.Config.Config.ApplicationName = $"Milyli.ScriptRunner::{agentName}";

			// Ensure logging is not being configured multiple times.
			LogManager.Configuration = new LoggingConfiguration();

			// Initialize agent logging
			AgentLoggingBootstrapper.ConfigureAgentTarget(this, 10);

			// Initialize event log logging.
			LoggingBootstrapper.ConfigureEventLogTarget("ScriptRunner Agent");

			NLog.MappedDiagnosticsContext.Set("Agent", $"{this.Name}:{this.AgentID}");

			using (var parentContainer = ContainerBootstrapper.Setup(this.Helper))
			{
				using (var childContainer = parentContainer.CreateChildContainer())
				{
					this.Execute(childContainer);
				}

				this.RaiseMessage("Completed.", 10);
			}
		}

		public void RaiseLimitedError(Exception exception, string message = "")
		{
			string detail = string.Empty;
			if (exception != null)
			{
				detail = exception.ToString();
			}

			detail = detail.Length > Logdetailcharlimit ? detail.Substring(0, Logdetailcharlimit) : detail;
			this.RaiseError(this.BuildMessage(exception, message), detail);
		}

		public void RaiseLimitedWarning(Exception exception, string message = "")
		{
			this.RaiseWarning(this.BuildMessage(exception, message));
		}

		public void RaiseLimitedMessage(string message, int level)
		{
			this.RaiseMessage(this.BuildMessage(null, message), level);
		}

		protected abstract void Execute(IContainer container);

		private string BuildMessage(Exception e, string msg = "")
		{
			string exceptionString = string.Empty;
			if (e != null)
			{
				exceptionString = e.ToString();
			}

			var message = string.Format(CultureInfo.InvariantCulture, "{0} {1}", msg, exceptionString);
			message = message.Length > Logmessagecharlimit ? message.Substring(0, Logmessagecharlimit) : message;
			return message;
		}
	}
}