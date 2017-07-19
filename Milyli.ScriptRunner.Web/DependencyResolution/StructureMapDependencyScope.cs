// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructureMapDependencyScope.cs" company="Web Advanced">
// Copyright 2012 Web Advanced (www.webadvanced.com)
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Milyli.ScriptRunner.Web.DependencyResolution
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    using Microsoft.Practices.ServiceLocation;

    using StructureMap;

    /// <summary>
    /// The structure map dependency scope.
    /// </summary>
    public class StructureMapDependencyScope : ServiceLocatorImplBase
    {
        private const string NestedContainerKey = "Nested.Container.Key";

        public StructureMapDependencyScope(IContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            this.Container = container;
        }

        public IContainer Container { get; set; }

        public IContainer CurrentNestedContainer
        {
            get
            {
                return (IContainer)this.HttpContext?.Items[NestedContainerKey];
            }

            set
            {
                this.HttpContext.Items[NestedContainerKey] = value;
            }
        }

        private HttpContextBase HttpContext
        {
            get
            {
                var ctx = this.Container.TryGetInstance<HttpContextBase>();
                return ctx ??
                    (System.Web.HttpContext.Current != null ?
                        new HttpContextWrapper(System.Web.HttpContext.Current)
                        :
                        null);
            }
        }

        public void CreateNestedContainer()
        {
            if (this.CurrentNestedContainer != null)
            {
                return;
            }

            this.CurrentNestedContainer = this.Container.GetNestedContainer();
        }

        public void Dispose()
        {
            this.DisposeNestedContainer();
            this.Container.Dispose();
        }

        public void DisposeNestedContainer()
        {
            if (this.CurrentNestedContainer != null)
            {
                this.CurrentNestedContainer.Dispose();
				this.CurrentNestedContainer = null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return this.DoGetAllInstances(serviceType);
        }

        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            return (this.CurrentNestedContainer ?? this.Container).GetAllInstances(serviceType).Cast<object>();
        }

        protected override object DoGetInstance(Type serviceType, string key)
        {
            IContainer container = this.CurrentNestedContainer ?? this.Container;

            if (string.IsNullOrEmpty(key))
            {
                return serviceType.IsAbstract || serviceType.IsInterface
                    ? container.TryGetInstance(serviceType)
                    : container.GetInstance(serviceType);
            }

            return container.GetInstance(serviceType, key);
        }
    }
}
