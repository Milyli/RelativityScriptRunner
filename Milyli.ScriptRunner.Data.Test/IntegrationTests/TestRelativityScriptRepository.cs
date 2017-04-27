namespace Milyli.ScriptRunner.Data.Test.IntegrationTests
{
    using System.Linq;
    using Milyli.ScriptRunner.Data.Repositories;
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
