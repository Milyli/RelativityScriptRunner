namespace Milyli.ScriptRunner.Web.DependencyResolution
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Web.Mvc;
	using StructureMap;

	public class ContainerFilterProvider : IFilterProvider
	{
		private readonly IEnumerable<Filter> filters;

		public ContainerFilterProvider (IContainer container)
		{
			this.filters = container.GetAllInstances<IActionFilter>()
				.Select(f => new Filter(f, FilterScope.Action, null));
		}

		public IEnumerable<Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
		{
			return this.filters;
		}
	}
}