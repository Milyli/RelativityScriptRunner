using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Milyli.ScriptRunner.Core.Repositories;
using Milyli.ScriptRunner.Models;
using Milyli.ScriptRunner.Core.Models;
using System.Net;

namespace Milyli.ScriptRunner.Controllers
{
    public class RelativityScriptController : ScriptRunnerController
    {
        public RelativityScriptController(IJobScheduleRepository jobScheduleRepository, IRelativityScriptRepository scriptRepository, IRelativityWorkspaceRepository workspaceRepository) 
            : base(jobScheduleRepository, scriptRepository, workspaceRepository)
        {
        }

        // GET: RelativityScript
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Script(int relativityWorkspaceId, int relativityScriptId)
        {
            var relativityWorkspace = this.workspaceRepository.Read(relativityWorkspaceId);
            if (relativityWorkspace == null)
            {
                return new HttpNotFoundResult($"could not find relativity workspace {relativityWorkspaceId}");
            }

            var relativityScript = this.relativityScriptRepository.GetRelativityScript(relativityWorkspace, relativityScriptId);
            if (relativityScript == null)
            {
                return new HttpNotFoundResult($"could not find relativity workspace {relativityWorkspaceId}");
            }

            var jobSchedules = this.jobScheduleRepository.GetJobSchedules(relativityScript);

            var relativityScriptModel = new RelativityScriptModel(relativityScript, jobSchedules);

            return this.View(relativityScriptModel);
        }

        public JsonResult List(int relativityWorkspaceId, int? relativityScriptId = null)
        {
            var relativityWorkspace = this.workspaceRepository.Read(relativityWorkspaceId);
            if (relativityWorkspace == null)
            {
                this.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    return this.Json(new { error = $"Cannot find a workspace with id {relativityWorkspaceId}" });
            }

            var scriptListModel = new ScriptListModel()
            {
                RelativityWorkspace = relativityWorkspace,
                RelativityScripts = this.GetScriptList(relativityWorkspace).ToList()
            };

            return this.Json(scriptListModel);
        }

        private IEnumerable<RelativityScriptModel> GetScriptList(RelativityWorkspace relativityWorkspace)
        {
            var jobSchedules = this.jobScheduleRepository.GetJobSchedules(relativityWorkspace).GroupBy(s => s.RelativityScriptId).ToDictionary(s => s.Key, s => s);
            var scripts = this.relativityScriptRepository.GetRelativityScripts(relativityWorkspace);
            foreach (var script in scripts)
            {
                if (jobSchedules.ContainsKey(script.RelativityScriptId))
                {
                    yield return new RelativityScriptModel(script, jobSchedules[script.RelativityScriptId]);
                }
                else
                {
                    yield return new RelativityScriptModel(script);
                }
            }
        }
    }
}