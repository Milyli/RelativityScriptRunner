namespace Milyli.ScriptRunner.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using Milyli.ScriptRunner.Core.Models;

    public class RelativityScriptModel
    {
        public RelativityScriptModel()
        {
        }

        public RelativityScriptModel(RelativityScript relativityScript, IEnumerable<JobSchedule> jobSchedules)
            : this(relativityScript)
        {
            this.JobSchedules = jobSchedules.OrderBy(s => s.NextExecutionTime).ToList();
        }

        public RelativityScriptModel(RelativityScript relativityScript)
        {
            this.RelativityScript = relativityScript;
        }

        public RelativityScript RelativityScript { get; private set; }

        public List<JobSchedule> JobSchedules { get; private set; } = new List<JobSchedule>();
    }
}