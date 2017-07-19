namespace Milyli.ScriptRunner.Core.Relativity.Interfaces
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public interface IRelativityConstants
    {
        [Required]
        IEnumerable<IRelativityEnvironment> RelativityEnvironments { get; }
    }
}
