using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Milyli.ScriptRunner.Controllers
{
    public class JobScheduleController : ScriptRunnerController
    {
        //
        // GET: /JobSchedule/

        public ActionResult Index(int jobScheduleId)
        {

            return View();
        }

    }
}
