namespace Milyli.ScriptRunner.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using Milyli.ScriptRunner.Core.Models;

    public class JobScheduleModel
    {
        public bool IsNew { get; internal set; }

        public RelativityWorkspace RelativityWorkspace { get; set; }

        public RelativityScript RelativityScript { get; set; }

        public JobSchedule JobSchedule { get; set; }

        public List<JobScriptInputModel> JobScriptInputs { get; set; }

        public List<JobScriptInput> ToJobScriptInputs()
        {
            return this.JobScriptInputs
                .Where(si => !string.IsNullOrWhiteSpace(si.InputValue))
                .Select(si => new JobScriptInput()
            {
                JobScheduleId = this.JobSchedule.Id,
                InputId = si.InputId,
                InputName = si.InputName,
                InputValue = si.InputValue
            }).ToList();
        }
    }
}