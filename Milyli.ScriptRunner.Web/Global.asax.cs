﻿namespace Milyli.ScriptRunner.Web
{
	using System;
	using System.Web.Mvc;
	using System.Web.Optimization;
	using System.Web.Routing;
	using Milyli.ScriptRunner.Core.Tools;
	using Models;

	// Note: For instructions on enabling IIS6 or IIS7 classic mode,
	// visit http://go.microsoft.com/?LinkId=9394801
	public class MvcApplication : System.Web.HttpApplication
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "MVC Convention")]
		protected void Application_Start()
		{
			AppDomain.CurrentDomain.AssemblyResolve += KcuraAssemblyResolver.ResolveKcuraAssembly;

			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
			ModelBinders.Binders.Add(typeof(JobScheduleModel), new JsonBinder());
		}
	}
}