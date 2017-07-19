namespace Milyli.ScriptRunner.Web.DependencyResolution
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using App_Start;
    using StructureMap;

    public class ContainerFilterProvider : IFilterProvider
    {
        private readonly IContainer container;

        public ContainerFilterProvider(IContainer container)
        {
            this.container = container;
        }

        public IEnumerable<Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            return StructuremapMvc.StructureMapDependencyScope.CurrentNestedContainer
                .GetAllInstances<IActionFilter>()
                .Select(f => new Filter(f, FilterScope.Action, null));
        }
    }
}