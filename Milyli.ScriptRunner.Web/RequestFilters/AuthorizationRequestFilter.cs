namespace Milyli.ScriptRunner.Web.RequestFilters
{
    using System.Web.Mvc;
    using Milyli.ScriptRunner.Core.Repositories;
    using Milyli.ScriptRunner.Core.Services;
    using Relativity.Services.Group;

    public class AuthorizationRequestFilter : ActionFilterAttribute
    {
        private const int RelativityScriptArtifactType = 28;
        private readonly IPermissionRepository permissionRepository;
        private readonly IPermissionsService permissionService;

        public AuthorizationRequestFilter(IPermissionRepository permissionsService, IPermissionsService permissionService)
        {
            this.permissionRepository = permissionsService;
            this.permissionService = permissionService;
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            return;
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {

            if (!this.permissionService.CanEdit(-1, RelativityScriptArtifactType))
            {
                var redirectResult = new RedirectResult("NotAuthorized");
                filterContext.Result = redirectResult;
            }
        }
    }
}