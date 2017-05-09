namespace Milyli.ScriptRunner.Controllers
{
    using System.Web.Mvc;
    using Core.Models;
    using Milyli.ScriptRunner.Core.Repositories;
    using Newtonsoft.Json;

    public abstract class ScriptRunnerController : Controller
    {

        private IJobScheduleRepository jobScheduleRepository;
        private IRelativityScriptRepository relativityScriptRepository;
        private IRelativityWorkspaceRepository workspaceRepository;

        protected ScriptRunnerController(IJobScheduleRepository jobScheduleRepository, IRelativityScriptRepository scriptRepository, IRelativityWorkspaceRepository workspaceRepository)
        {
            this.jobScheduleRepository = jobScheduleRepository;
            this.relativityScriptRepository = scriptRepository;
            this.workspaceRepository = workspaceRepository;
        }

        protected static ContentResult JsonContent(object model)
        {
            return new ContentResult()
            {
                ContentType = "application/json",
                Content = JsonConvert.SerializeObject(model)
            };
        }

        protected IJobScheduleRepository JobScheduleRepository
        {
            get
            {
                return this.jobScheduleRepository;
            }
        }

        protected IRelativityScriptRepository RelativityScriptRepository
        {
            get
            {
                return this.relativityScriptRepository;
            }
        }

        protected IRelativityWorkspaceRepository WorkspaceRepository
        {
            get
            {
                return this.workspaceRepository;
            }
        }

        protected RelativityWorkspace GetWorkspace(int? workspaceId)
        {
            return this.GetWorkspace(workspaceId ?? RelativityWorkspace.AdminWorkspaceId);
        }

        protected RelativityWorkspace GetWorkspace(int workspaceId)
        {
            return workspaceId != RelativityWorkspace.AdminWorkspaceId ? this.WorkspaceRepository.Read(workspaceId) :
                RelativityWorkspace.AdminWorkspace;
        }
    }
}