using Milyli.ScriptRunner.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Milyli.ScriptRunner.Web.RequestFilters
{
	public class AuthorizationRequestFilter : IActionFilter
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
			if (!this.permissionsService.CanEdit(-1, RelativityScriptArtifactType))
			{
				var redirectResult = new RedirectResult("/");
				filterContext.Result = redirectResult;
			}
		}
	}
}