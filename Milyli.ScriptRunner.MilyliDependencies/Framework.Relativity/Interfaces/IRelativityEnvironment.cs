// Copyright © 2017 Milyli

namespace Milyli.ScriptRunner.MilyliDependencies.Framework.Relativity.Interfaces
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public interface IRelativityEnvironment
    {
        [Required]
        string Server { get; }
        [Required]
        int WorkspaceId { get; }
        int MarkupSetId { get; }
        int SavedSearchId { get; }
        int JobId { get; }
        IEnumerable<IRelativityFile> Files { get; }
        string ExecutingMachineName { get; }
        [Required]
        string TestEmailAddress { get; }
        string DatabasePassword { get; }
    }

    public interface IRelativityFile
    {
        string FileGuid { get; }
    }
}