using Milyli.ScriptRunner.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milyli.ScriptRunner.Core.JobExecution
{
    public interface IJobRunner
    {
        Task<int> RunJob(JobSchedule jobSchedule, RelativityWorkspace workspace);
    }
}
