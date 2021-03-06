﻿// <copyright file="TestRelativityWorkspaceRepository.cs" company="Milyli">
// Copyright © 2016 Milyli
// </copyright>

using Milyli.ScriptRunner.Core.Repositories.Interfaces;

namespace Milyli.ScriptRunner.Core.Test.IntegrationTests
{
    using System.Linq;
    using Milyli.ScriptRunner.Core.Repositories;
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
