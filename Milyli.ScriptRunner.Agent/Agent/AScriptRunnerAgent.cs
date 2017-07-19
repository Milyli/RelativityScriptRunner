// <copyright file="AScriptRunnerAgent.cs" company="Milyli">
// Copyright © 2016 Milyli
// </copyright>

namespace Milyli.ScriptRunner.Agent.Agent
{
	using System;
	using Milyli.ScriptRunner.Agent.DependencyResolution;
	using Relativity.API;
	using StructureMap;

	public abstract class AScriptRunnerAgent : kCura.Agent.AgentBase
	{
		public override void Execute()
		{
			var agentName = this.Name.Replace(" ", string.Empty);
			kCura.Config.Config.ApplicationName = $"Milyli.ScriptRunner::{agentName}";
			using (var parentContainer = ContainerBootstrapper.Setup(base.Helper))
			{
				using (var childContainer = parentContainer.CreateChildContainer())
				{
					this.Execute(childContainer);
				}

				this.RaiseMessage("Completed.", 10);
			}
		}

		protected abstract void Execute(IContainer container);
	}
}
