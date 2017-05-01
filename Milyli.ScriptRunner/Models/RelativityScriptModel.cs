namespace Milyli.ScriptRunner.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using Milyli.ScriptRunner.Core.Models;

    public class RelativityScriptModel
    {
        public RelativityScriptModel(RelativityScript relativityScript, IEnumerable<JobSchedule> jobSchedules)
        {
            this.RelativityScript = relativityScript;
            this.JobSchedules = jobSchedules.OrderBy(s => s.NextExecutionTime).ToList();
        }

        public RelativityScript RelativityScript { get; private set; }

        public List<JobSchedule> JobSchedules { get; private set; }
    }
}