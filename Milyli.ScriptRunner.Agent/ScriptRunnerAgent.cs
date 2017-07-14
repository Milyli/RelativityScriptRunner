// <copyright file="ScriptRunnerAgent.cs" company="Milyli">
// Copyright © 2016 Milyli
// </copyright>

namespace Milyli.ScriptRunner.Agent
{
    using System;
    using Core.Services;
    using Milyli.ScriptRunner.Agent.Agent;
    using StructureMap;

    [kCura.Agent.CustomAttributes.Name("Milyli ScriptRunner Agent")]
    [System.Runtime.InteropServices.Guid("a3e50474-a27d-486b-b46b-c0fb6b40d3c5")]
    public class ScriptRunnerAgent : Agent.AScriptRunnerAgent
    {
        /// <summary>
        /// Gets the name of the agent
        /// </summary>
        public override string Name
        {
            get
            {
                return "Milyli ScriptRunner Agent";
            }
        }

        protected override void Execute(IContainer container)
        {
            var scriptRunner = container.GetInstance<IRelativityScriptRunner>();
          scriptRunner.ExecuteAllJobs(DateTime.UtcNow);
        }
    }
}
