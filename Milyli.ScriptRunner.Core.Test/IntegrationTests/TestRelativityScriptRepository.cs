// <copyright file="TestRelativityScriptRepository.cs" company="Milyli">
// Copyright © 2016 Milyli
// </copyright>

using Milyli.ScriptRunner.Core.Repositories.Interfaces;

namespace Milyli.ScriptRunner.Core.Test.IntegrationTests
{
    using System.Linq;
    using Milyli.ScriptRunner.Core.Repositories;
    using NUnit.Framework;

    [TestFixture(Category="Integration")]
    public class TestRelativityScriptRepository : IntegrationTestFixture
    {
        private IRelativityScriptRepository RelativityScriptRepository
        {
            get
            {
                return this.Container.GetInstance<IRelativityScriptRepository>();
            }
        }

        [Test]
        public void TestGetScripts()
        {
            var scripts = this.RelativityScriptRepository.GetRelativityScripts(Models.RelativityWorkspace.AdminWorkspace);
            Assert.That(scripts.Any(), "we are expecting some scripts in the admin workspace");
        }
    }
}
