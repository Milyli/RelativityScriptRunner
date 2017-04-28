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
        private IJobScheduleRepository jobScheduleRepository;
        private IRelativityScriptRepository scriptRepository;
    }
}