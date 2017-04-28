namespace Milyli.ScriptRunner.Models
{
    using System.Collections.Generic;
    using Milyli.ScriptRunner.Core.Models;

    public class JobScheduleModel
    {
        public RelativityWorkspace RelativityWorkspace { get; set; }

        public RelativityScript RelativityScript { get; set; }

        public JobSchedule JobSchedule { get; set; }

        public List<JobScriptInputModel> JobScriptInputs { get; set; }
    }
}