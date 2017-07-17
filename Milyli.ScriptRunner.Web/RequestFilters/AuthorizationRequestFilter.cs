namespace Milyli.ScriptRunner.Web.RequestFilters
{
    using System.Web.Mvc;
    using Core.Services;

    public class AuthorizationRequestFilter : ActionFilterAttribute
    {
        private const int RelativityScriptArtifactType = 28;
        private readonly IPermissionsService permissionService;

        // TODO: Implement logic
        public AuthorizationRequestFilter(IPermissionsService permissionService)
        {
            this.permissionService = permissionService;
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
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