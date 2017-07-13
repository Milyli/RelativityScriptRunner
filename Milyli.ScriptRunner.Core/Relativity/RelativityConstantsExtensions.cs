namespace Milyli.ScriptRunner.Core.Relativity
{
    using System;
    using System.Linq;
    using Interfaces;

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
