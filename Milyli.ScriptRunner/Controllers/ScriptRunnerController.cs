using Milyli.ScriptRunner.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Milyli.ScriptRunner.Controllers
{
    public abstract class ScriptRunnerController : Controller
    {
        protected IJobScheduleRepository jobScheduleRepository;
        protected IRelativityScriptRepository scriptRepository;
        protected IRelativityWorkspaceRepository workspaceRepository;

        public ScriptRunnerController(IJobScheduleRepository jobScheduleRepository, IRelativityScriptRepository scriptRepository, IRelativityWorkspaceRepository workspaceRepository)
        {
            this.jobScheduleRepository = jobScheduleRepository;
            this.scriptRepository = scriptRepository;
            this.workspaceRepository = workspaceRepository;
        }
    }
}