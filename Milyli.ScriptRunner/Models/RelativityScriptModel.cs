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

        public int? RelativityScriptId
        {
            get
            {
                return this.RelativityScript?.RelativityScriptId;
            }

            set
            {
                if (this.RelativityScript != null)
                {
                    this.RelativityScript.RelativityScriptId = value.Value;
                }
            }
        }

        public string Name
        {
            get
            {
                return this.RelativityScript.Name;
            }

            set
            {
                if (this.RelativityScript != null)
                {
                    this.RelativityScript.Name = value;
                }
            }
        }

        public RelativityScript RelativityScript { get; private set; }

        public List<JobSchedule> JobSchedules { get; private set; } = new List<JobSchedule>();
    }
}