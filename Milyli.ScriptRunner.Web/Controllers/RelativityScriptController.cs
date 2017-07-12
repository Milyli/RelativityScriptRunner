namespace Milyli.ScriptRunner.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using Milyli.ScriptRunner.Core.Models;
    using Milyli.ScriptRunner.Core.Repositories;
    using Milyli.ScriptRunner.Web.Models;
    public class RelativityScriptController : ScriptRunnerController
    {
        public RelativityScriptController(IJobScheduleRepository jobScheduleRepository, IRelativityScriptRepository scriptRepository, IRelativityWorkspaceRepository workspaceRepository, IPermissionRepository permissionRepository)
            : base(jobScheduleRepository, scriptRepository, workspaceRepository, permissionRepository)
        {
        }

        // GET: RelativityScript
        public ActionResult Index()
        {
            if (this.PermissionRepository.IsUserAdmin(Relativity.CustomPages.ConnectionHelper.Helper().GetAuthenticationManager().UserInfo.ArtifactID))
            {
                return this.Redirect(this.Url.Action("List"));
            }
            else
            {
                return this.NotAuthorized();
            }
        }

        public ViewResult NotAuthorized()
        {

            var viewResult = new ViewResult()
            {
                ViewName = "~/Views/Error/NotAuthorized.cshtml",
            };

            return viewResult;
        }

        public ActionResult Script(int relativityWorkspaceId, int relativityScriptId)
        {
            var relativityWorkspace = this.GetWorkspace(relativityWorkspaceId);
            if (relativityWorkspace == null)
            {
                this.NotFound($"Could not find relativity workspace with id {relativityWorkspaceId}");
            }

            var relativityScript = this.RelativityScriptRepository.GetRelativityScript(relativityWorkspace, relativityScriptId);
            if (relativityScript == null)
            {
                this.NotFound($"Could not find relativity script with id {relativityScriptId}");
            }

            var jobSchedules = this.JobScheduleRepository.GetJobSchedules(relativityScript);

            var relativityScriptModel = new RelativityScriptModel(relativityScript, relativityWorkspace, jobSchedules);

            return this.View(relativityScriptModel);
        }

        public ActionResult List(int? relativityWorkspaceId = null)
        {
            var relativityWorkspace = this.GetWorkspace(relativityWorkspaceId);
            if (relativityWorkspace == null)
            {
                this.NotFound($"Cannot find a workspace with id {relativityWorkspaceId ?? default(int)}");
            }

            var scriptListModel = new ScriptListModel()
            {
                RelativityWorkspace = relativityWorkspace,
                RelativityWorkspaces = this.WorkspaceRepository.AllWorkspaces.ToList(),
                RelativityScripts = this.GetScriptList(relativityWorkspace).ToList()
            };

            return this.View(scriptListModel);
        }

        private IEnumerable<RelativityScriptModel> GetScriptList(RelativityWorkspace relativityWorkspace)
        {
            var jobSchedules = this.JobScheduleRepository.GetJobSchedules(relativityWorkspace).GroupBy(s => s.RelativityScriptId).ToDictionary(s => s.Key, s => s);
            var scripts = this.RelativityScriptRepository.GetRelativityScripts(relativityWorkspace);
            foreach (var script in scripts)
            {
                if (jobSchedules.ContainsKey(script.RelativityScriptId))
                {
                    yield return new RelativityScriptModel(script, relativityWorkspace, jobSchedules[script.RelativityScriptId]);
                }
                else
                {
                    yield return new RelativityScriptModel(script, relativityWorkspace);
                }
            }
        }
    }
}