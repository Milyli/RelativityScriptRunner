// Copyright © 2017 Milyli

namespace Milyli.ScriptRunner.MilyliDependencies.Framework.Relativity.Interfaces
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public interface IRelativityConstants
    {
        [Required]
        IEnumerable<IRelativityEnvironment> RelativityEnvironments { get; }
    }
}
