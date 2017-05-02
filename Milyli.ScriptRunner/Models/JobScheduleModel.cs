namespace Milyli.ScriptRunner.Models
{
    using System.Collections.Generic;
    using Milyli.ScriptRunner.Core.Models;
    using System.Linq;

    public class JobScheduleModel
    {
        public RelativityWorkspace RelativityWorkspace { get; set; }

        public RelativityScript RelativityScript { get; set; }

        public JobSchedule JobSchedule { get; set; }

        public List<JobScriptInputModel> JobScriptInputs { get; set; }

        public List<JobScriptInput> GetJobScriptInputs()
        {
            return this.JobScriptInputs.Select(si => new JobScriptInput()
            {
                JobScheduleId = this.JobSchedule.Id,
                InputName = si.InputName,
                InputValue = si.InputValue
            }).ToList();
        }
    }
}