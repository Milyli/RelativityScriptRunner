namespace Milyli.ScriptRunner.Web.Controllers
{
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using Core.Models;
    using Core.Repositories.Interfaces;
    using Newtonsoft.Json;

    public abstract class ScriptRunnerController : Controller
    {
        protected ScriptRunnerController(IJobScheduleRepository jobScheduleRepository, IRelativityScriptRepository scriptRepository, IRelativityWorkspaceRepository workspaceRepository, IPermissionRepository permissionRepository)
        {
            this.JobScheduleRepository = jobScheduleRepository;
            this.RelativityScriptRepository = scriptRepository;
            this.WorkspaceRepository = workspaceRepository;
            this.PermissionRepository = permissionRepository;
        }

        protected IJobScheduleRepository JobScheduleRepository { get; }

        protected IRelativityScriptRepository RelativityScriptRepository { get; }

        protected IRelativityWorkspaceRepository WorkspaceRepository { get; }

        protected IPermissionRepository PermissionRepository { get; }

        protected static ContentResult JsonContent(object model)
        {
            return new ContentResult()
            {
                ContentType = "application/json",
                Content = JsonConvert.SerializeObject(model)
            };
        }

        protected void NotFound(string message)
        {
            this.Response.StatusCode = (int)HttpStatusCode.NotFound;
            throw new HttpException((int)HttpStatusCode.NotFound, message);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;
            // If we made a json request, return a json object
            if (this.Request.ContentType.StartsWith("application/json", System.StringComparison.InvariantCultureIgnoreCase))
            {
                filterContext.Result = JsonContent(new
                {
                    Error = this.Response.Status,
                    Message = filterContext.Exception.Message,
                    Stacktrace = filterContext.Exception.StackTrace
                });
            }
            else
            {
                var viewResult = new ViewResult()
                {
                    ViewName = "~/Views/Error/Index.cshtml",
                };
                viewResult.ViewBag.Exception = filterContext.Exception;
                viewResult.ViewBag.Title = this.Response.Status;
                filterContext.Result = viewResult;
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