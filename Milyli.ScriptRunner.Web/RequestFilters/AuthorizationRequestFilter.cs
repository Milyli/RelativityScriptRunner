

namespace Milyli.ScriptRunner.Web.RequestFilters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Milyli.ScriptRunner.Core.Services;
    using Relativity.Services.Group;

    public class AuthorizationRequestFilter : ActionFilterAttribute
    {
        private const int RelativityScriptArtifactType = 28;
        private readonly IPermissionsService permissionsService;

        public AuthorizationRequestFilter(IPermissionsService permissionsService)
        {
            this.permissionsService = permissionsService;
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            return;
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var mgr = this.permissionsService.PermissionManager;
            foreach (var perm in mgr.GetAdminGroupSelectorAsync().Result.EnabledGroups)
            {
                var users = mgr.GetAdminGroupUsersAsync(new GroupRef(perm.ArtifactID)).Result;
            }

            //if (true)
            //{
            //    filterContext.Result = new RedirectResult("~/Views/Error/NotAuthorized");
            //    base.OnActionExecuting(filterContext);
            //}

            if (!this.permissionsService.CanEdit(-1, RelativityScriptArtifactType))
            {
                var redirectResult = new RedirectResult("NotAuthorized");
                filterContext.Result = redirectResult;
            }
        }
    }
}