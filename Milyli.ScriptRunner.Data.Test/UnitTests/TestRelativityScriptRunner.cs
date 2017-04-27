namespace Milyli.ScriptRunner.Data.Test.UnitTests
{
    using System.Collections.Generic;
    using kCura.Relativity.Client;
    using Models;
    using Moq;
    using NUnit.Framework;
    using Repositories;
    using Services;
    using System;

    [TestFixture(Category="Unit")]
    public class TestRelativityScriptRunner
    {
        [Test]
        public void TestGoodRun()
        {
            var clientMock = new Mock<IRSAPIClient>();
            var result = new RelativityScriptResult()
            {
                Success = true,
                Message = "unit test result"
            };
            var calledStart = false;
            var calledFinish = false;
            var jobScheduleRepository = new Mock<IJobScheduleRepository>();
            var scriptRunner = new RelativityScriptRunner(jobScheduleRepository.Object, clientMock.Object);
            var jobSchedule = new JobSchedule();
            jobSchedule.CurrentJobHistory = new JobHistory();

            clientMock.Setup(client => client.ExecuteRelativityScript(It.IsAny<APIOptions>(), It.IsAny<int>(), It.IsAny<List<RelativityScriptInput>>())).Returns(result);
            clientMock.Setup(m => m.APIOptions).Returns(new APIOptions(-1));

            jobScheduleRepository.Setup(jsr => jsr.StartJob(It.IsAny<JobSchedule>())).Callback(() =>
            {
                calledStart = true;
            })
            .Returns(JobActivationStatus.Started);

            jobScheduleRepository.Setup(jsr => jsr.GetJobInputs(It.IsAny<JobSchedule>())).Returns(new List<JobScriptInput>());

            jobScheduleRepository.Setup(jsr => jsr.FinishJob(It.IsAny<JobSchedule>())).Callback(() =>
            {
                calledFinish = true;
            });

            scriptRunner.ExecuteScriptJob(jobSchedule);
            Assert.That(jobSchedule.CurrentJobHistory.ResultText.Equals(result.Message));
            Assert.That(!jobSchedule.CurrentJobHistory.Errored);
            Assert.That(calledFinish && calledStart);
        }

        [Test]
        public void TestExecptionThrown()
        {
            var clientMock = new Mock<IRSAPIClient>();
            var result = new RelativityScriptResult()
            {
                Success = true,
                Message = "unit test result"
            };
            var calledStart = false;
            var calledFinish = false;
            var jobScheduleRepository = new Mock<IJobScheduleRepository>();
            var scriptRunner = new RelativityScriptRunner(jobScheduleRepository.Object, clientMock.Object);
            var jobSchedule = new JobSchedule();
            jobSchedule.CurrentJobHistory = new JobHistory();

            clientMock
                .Setup(client => client.ExecuteRelativityScript(It.IsAny<APIOptions>(), It.IsAny<int>(), It.IsAny<List<RelativityScriptInput>>()))
                .Throws(new Exception("None shall pass"));

            clientMock.Setup(m => m.APIOptions).Returns(new APIOptions(-1));

            jobScheduleRepository.Setup(jsr => jsr.GetJobInputs(It.IsAny<JobSchedule>())).Returns(new List<JobScriptInput>());

            jobScheduleRepository.Setup(jsr => jsr.StartJob(It.IsAny<JobSchedule>())).Callback(() =>
            {
                calledStart = true;
            })
            .Returns(JobActivationStatus.Started);
            jobScheduleRepository.Setup(jsr => jsr.FinishJob(It.IsAny<JobSchedule>())).Callback(() =>
            {
                calledFinish = true;
            });

            Assert.Throws<Exception>(() => scriptRunner.ExecuteScriptJob(jobSchedule));
            Assert.That(jobSchedule.CurrentJobHistory.Errored);
            Assert.That(calledFinish && calledStart);
        }
    }
}
