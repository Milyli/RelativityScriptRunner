// Copyright © 2017 Milyli

namespace Milyli.ScriptRunner.MilyliDependencies
{
    using System;
    using System.Linq;
    using Framework.Relativity.Interfaces;

    public static class RelativityConstantsExtensions
    {
        public static IRelativityEnvironment GetFirstEnvironment(this IRelativityConstants config)
            => config.RelativityEnvironments.First();

        public static IRelativityEnvironment GetEnvironment(this IRelativityConstants config, string server)
            => config.RelativityEnvironments.Single(x => x.Server == server);
        public static IRelativityEnvironment GetFirstEnvironmentForExecutingMachine(this IRelativityConstants config)
            => config.RelativityEnvironments.Single(x => x.ExecutingMachineName.Equals(Environment.MachineName, StringComparison.InvariantCultureIgnoreCase));
    }
}
