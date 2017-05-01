// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultRegistry.cs" company="Web Advanced">
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

namespace Milyli.ScriptRunner.DependencyResolution {
    using System;
    using Core.DependencyResolution;
    using Core.Relativity.Client;
    using Core.Repositories;
    using Framework.Relativity;
    using Framework.Relativity.Factories;
    using Framework.Relativity.Interfaces;
    using Framework.Repositories.Interfaces;
    using kCura.Relativity.Client;
    using Relativity.API;
    using Relativity.CustomPages;
    using StructureMap;
    using StructureMap.Web;

    public class DefaultRegistry : Registry
    {
        public DefaultRegistry()
        {
            this.Scan(
                scan =>
                {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                    scan.With(new ControllerConvention());
                });

            Func<ICPHelper> helper = () => ConnectionHelper.Helper();
            this.For<IHelper>().HttpContextScoped().Use(c => ConnectionHelper.Helper());
            this.For<IRelativityClientFactory>().HttpContextScoped().Add<RsapiClientFactory>();
            this.For<IRelativityContext>().Use(new RelativityContext(-1));
            this.For<IInstanceConnectionFactory>().Use<RelativityInstanceConnectionFactory>();
            this.IncludeRegistry(new ScriptRunnerRegistry());

            this.For<IRSAPIClient>()
                .Use(ctx => ctx.GetInstance<IRelativityClientFactory>().GetRelativityClient(ExecutionIdentity.CurrentUser))
                .ContainerScoped();
        }
    }
}