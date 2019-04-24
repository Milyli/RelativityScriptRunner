// <copyright file="TestRelativityScriptRunner.cs" company="Milyli">
// Copyright © 2016 Milyli
// </copyright>

using Milyli.ScriptRunner.Core.Models;

namespace Milyli.ScriptRunner.Core.Test.UnitTests
{
	using System;
	using System.Collections.Generic;
	using global::Relativity.API;
	using kCura.Relativity.Client;
	using kCura.Relativity.Client.DTOs;
	using Milyli.ScriptRunner.Core.Repositories.Interfaces;
	using Mocks;
	using Moq;
	using NUnit.Framework;
	using Relativity.Client;
	using Services;

	[TestFixture(Category = "Unit")]
	public class TestRelativityScriptRunner
	{
		[Test]
		public void TestGoodRun()
		{
			var clientMock = new Mock<IRSAPIClient>();
			var factoryMock = new Mock<IRelativityClientFactory>();
			var scriptExecutionRepositoryMock = new Mock<IRelativityScriptRepository>();

			factoryMock.Setup(m => m.GetRelativityClient(It.IsAny<ExecutionIdentity>())).Returns(clientMock.Object);
			factoryMock.Setup(m => m.GetRelativityClient()).Returns(clientMock.Object);

			var result = new ExecuteResult()
			{
				Success = true,
				Message = "unit test result"
			};

			scriptExecutionRepositoryMock
				.Setup(repo => repo.ExecuteRelativityScript(It.IsAny<int>(), It.IsAny<List<JobScriptInput>>(), It.IsAny<RelativityWorkspace>())).Returns(result);

			clientMock.Setup(m => m.APIOptions).Returns(new APIOptions(-1));

			var jobScheduleRepositoryMock = new JobScheduleRepositoryMock();
			var scriptRunner = new RelativityScriptRunner(jobScheduleRepositoryMock.JobScheduleRepository, factoryMock.Object, scriptExecutionRepositoryMock.Object);

			var jobSchedule = jobScheduleRepositoryMock.CurrentJobSchedule;
			scriptRunner.ExecuteScriptJob(jobSchedule);

			Assert.That(jobSchedule.CurrentJobHistory.ResultText.Equals(result.Message));
			Assert.That(!jobSchedule.CurrentJobHistory.HasError);
			Assert.That(jobScheduleRepositoryMock.JobScheduleStarted && jobScheduleRepositoryMock.JobScheduleFinished);
		}

		[Test]
		public void TestExecptionThrown()
		{
			var clientMock = new Mock<IRSAPIClient>();
			var factoryMock = new Mock<IRelativityClientFactory>();
			var scriptExecutionRepositoryMock = new Mock<IRelativityScriptRepository>();
			factoryMock.Setup(m => m.GetRelativityClient(It.IsAny<ExecutionIdentity>())).Returns(clientMock.Object);
			factoryMock.Setup(m => m.GetRelativityClient()).Returns(clientMock.Object);

			scriptExecutionRepositoryMock
				.Setup(repo => repo.ExecuteRelativityScript(It.IsAny<int>(), It.IsAny<List<JobScriptInput>>(), It.IsAny<RelativityWorkspace>()))
				.Throws(new Exception("None shall pass"));

			clientMock.Setup(m => m.APIOptions).Returns(new APIOptions(-1));

			var jobScheduleRepositoryMock = new JobScheduleRepositoryMock();

			var scriptRunner = new RelativityScriptRunner(jobScheduleRepositoryMock.JobScheduleRepository, factoryMock.Object, scriptExecutionRepositoryMock.Object);
			var jobSchedule = jobScheduleRepositoryMock.CurrentJobSchedule;

			scriptRunner.ExecuteScriptJob(jobSchedule);
			Assert.That(jobSchedule.CurrentJobHistory.HasError);
			Assert.That(jobScheduleRepositoryMock.JobScheduleStarted && jobScheduleRepositoryMock.JobScheduleFinished);
		}
	}
}
