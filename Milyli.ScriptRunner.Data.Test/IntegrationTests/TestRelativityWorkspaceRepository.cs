namespace Milyli.ScriptRunner.Data.Test.IntegrationTests
{
    using System.Linq;
    using Milyli.ScriptRunner.Data.Repositories;
    using NUnit.Framework;

    [TestFixture(Category = "Integration")]
    public class TestRelativityWorkspaceRepository : IntegrationTestFixture
    {
        private IRelativityWorkspaceRepository WorkspaceRepository
        {
            get
            {
                return this.Container.GetInstance<IRelativityWorkspaceRepository>();
            }
        }

        [Test]
        public void TestGetWorkspaces()
        {
            var workspaces = this.WorkspaceRepository.AllWorkspaces;
            Assert.That(workspaces.Any(), "Should have some workspaces");
        }
    }
}
