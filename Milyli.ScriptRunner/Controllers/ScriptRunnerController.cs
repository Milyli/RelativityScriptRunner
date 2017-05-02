namespace Milyli.ScriptRunner.Controllers
{
    using System.Web.Mvc;
    using Milyli.ScriptRunner.Core.Repositories;

    public abstract class ScriptRunnerController : Controller
    {
#pragma warning disable SA1401 // Fields must be private
        protected IJobScheduleRepository jobScheduleRepository;
        protected IRelativityScriptRepository relativityScriptRepository;
        protected IRelativityWorkspaceRepository workspaceRepository;
#pragma warning restore SA1401 // Fields must be private

        public ScriptRunnerController(IJobScheduleRepository jobScheduleRepository, IRelativityScriptRepository scriptRepository, IRelativityWorkspaceRepository workspaceRepository)
        {
            this.jobScheduleRepository = jobScheduleRepository;
            this.relativityScriptRepository = scriptRepository;
            this.workspaceRepository = workspaceRepository;
        }
    }
}