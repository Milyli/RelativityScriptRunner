// <copyright file="ContainerBootstrapper.cs" company="Milyli">
// Copyright © 2016 Milyli
// </copyright>

namespace Milyli.ScriptRunner.Agent.DependencyResolution
{
    using Relativity.API;
    using StructureMap;

    public static class ContainerBootstrapper
    {
        public static IContainer Setup(IHelper helper)
        {
            var container = new Container(c =>
            {
                c.AddRegistry(new AgentRegistry(helper));
            });
            return container;
        }
    }
}
