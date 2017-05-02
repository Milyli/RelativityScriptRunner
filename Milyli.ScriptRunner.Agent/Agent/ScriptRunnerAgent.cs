// <copyright file="ScriptRunnerAgent.cs" company="Milyli">
// Copyright © 2016 Milyli
// </copyright>

namespace Milyli.ScriptRunner.Agent.Agent
{
    using System;
    using Milyli.ScriptRunner.Agent.DependencyResolution;
    using Relativity.API;
    using StructureMap;

    public abstract class ScriptRunnerAgent : kCura.Agent.AgentBase
    {
        private Lazy<IContainer> lazyContainer;

        private IHelper helper;

        public new IHelper Helper
        {
            get
            {
                return this.helper ?? base.Helper;
            }

            set
            {
                this.helper = value;
            }
        }

        private IContainer Container
        {
            get
            {
                return this.lazyContainer.Value;
            }
        }

        public override void Execute()
        {
            var agentName = this.Name.Replace(" ", string.Empty);
            kCura.Config.Config.ApplicationName = $"Milyli.ScriptRunner::{agentName}";
            if (this.lazyContainer == null)
            {
                this.lazyContainer = new Lazy<IContainer>(() => ContainerBootstrapper.Setup(this.Helper), false);
            }

            using (var childContainer = this.Container.CreateChildContainer())
            {
                this.Execute(childContainer);
            }

            this.RaiseMessage("Completed.", 10);
        }

        protected abstract void Execute(IContainer container);
    }
}
