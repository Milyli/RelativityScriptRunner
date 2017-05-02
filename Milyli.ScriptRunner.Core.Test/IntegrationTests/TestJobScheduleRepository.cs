// <copyright file="TestJobScheduleRepository.cs" company="Milyli">
// Copyright © 2016 Milyli
// </copyright>

namespace Milyli.ScriptRunner.Core.Test.IntegrationTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Milyli.ScriptRunner.Core.Repositories;
    using Models;
    using NUnit.Framework;

    [TestFixture(Category="Integration")]
    public class TestJobScheduleRepository : IntegrationTestFixture
    {
        private static readonly int TEST_SCRIPT_ID = 42;
        private static readonly int TEST_WORKSPACE_ID = -1;

        private IJobScheduleRepository JobScheduleRepository
        {
            get
            {
                return this.Container.GetInstance<IJobScheduleRepository>();
            }
        }

        [OneTimeTearDown]
        public void Cleanup ()
        {
            this.JobScheduleRepository.DeleteAllJobs(TEST_SCRIPT_ID);
        }

        [Test]
        public void TestCompetingStartup()
        {
            var jobSchedule = new JobSchedule()
            {
                RelativityScriptId = TEST_SCRIPT_ID,
                WorkspaceId = TEST_WORKSPACE_ID,
                ExecutionSchedule = 0x7F,
                ExecutionTime = JobSchedule.TimeSeconds(DateTime.Now)
            };
            jobSchedule.Id = this.JobScheduleRepository.Create(jobSchedule);
            var status = this.JobScheduleRepository.StartJob(jobSchedule);
            var nextStatus = this.JobScheduleRepository.StartJob(jobSchedule);
            Assert.That(
                status == JobActivationStatus.Started && nextStatus == JobActivationStatus.AlreadyRunning,
                string.Format("Expected to find job statuses of Started and AlreadyRunning, found {0} and {1}", status.ToString(), nextStatus.ToString()));
        }

        [Test]
        public void TestCurrentJobsToExecute()
        {
            var jobSchedule = new JobSchedule()
            {
                RelativityScriptId = TEST_SCRIPT_ID,
                WorkspaceId = TEST_WORKSPACE_ID,
                ExecutionSchedule = 0x7F,
                ExecutionTime = JobSchedule.TimeSeconds(DateTime.Now.AddMinutes(-3)),
                NextExecutionTime = DateTime.Now
            };

            jobSchedule.Id = this.JobScheduleRepository.Create(jobSchedule);
            var jobList = this.JobScheduleRepository.GetJobSchedules(DateTime.Now);
            Assert.That(jobList.Where(j => j.Id == jobSchedule.Id).Any(), "Expected at least one active job schedule");
        }

        [Test]
        public void TestJobsOutOfBounds()
        {
            var jobSchedule = new JobSchedule()
            {
                RelativityScriptId = TEST_SCRIPT_ID,
                WorkspaceId = TEST_WORKSPACE_ID,
                ExecutionSchedule = 0x7F,
                ExecutionTime = JobSchedule.TimeSeconds(DateTime.Now.AddMinutes(-3)),
                NextExecutionTime = DateTime.Now
            };

            var tooEarly = new JobSchedule()
            {
                RelativityScriptId = TEST_SCRIPT_ID,
                WorkspaceId = TEST_WORKSPACE_ID,
                ExecutionSchedule = 0x7F,
                ExecutionTime = JobSchedule.TimeSeconds(DateTime.Now.AddMinutes(-3)),
                NextExecutionTime = DateTime.Now.AddDays(-3)
            };

            var tooLate = new JobSchedule()
            {
                RelativityScriptId = TEST_SCRIPT_ID,
                WorkspaceId = TEST_WORKSPACE_ID,
                ExecutionSchedule = 0x7F,
                ExecutionTime = JobSchedule.TimeSeconds(DateTime.Now.AddMinutes(3)),
                NextExecutionTime = DateTime.Now.AddDays(3)
            };

            jobSchedule.Id = this.JobScheduleRepository.Create(jobSchedule);
            tooLate.Id = this.JobScheduleRepository.Create(tooLate);
            tooEarly.Id = this.JobScheduleRepository.Create(tooEarly);
            var jobList = this.JobScheduleRepository.GetJobSchedules(DateTime.Now);
            var ids = jobList.Select(j => j.Id).ToList();

            Assert.That(ids.Contains(jobSchedule.Id), string.Format("There should be one job (id {0}) in the result", jobSchedule.Id));
            Assert.That(!ids.Contains(tooEarly.Id) && !ids.Contains(tooLate.Id), string.Format("Did not expect jobs of ids {0} and {1}, but found those jobs", tooEarly.Id, tooLate.Id));
        }

        [Test]
        public void TestJobFinish()
        {
            var jobSchedule = new JobSchedule()
            {
                RelativityScriptId = TEST_SCRIPT_ID,
                WorkspaceId = TEST_WORKSPACE_ID,
                ExecutionSchedule = 0x7F,
                ExecutionTime = JobSchedule.TimeSeconds(DateTime.Now),
                NextExecutionTime = DateTime.Now
            };
            jobSchedule.Id = this.JobScheduleRepository.Create(jobSchedule);
            this.JobScheduleRepository.StartJob(jobSchedule);
            jobSchedule.CurrentJobHistory.StartTime = jobSchedule.CurrentJobHistory.StartTime.AddSeconds(-3);
            this.JobScheduleRepository.FinishJob(jobSchedule);
            var lastExecution = this.JobScheduleRepository.GetLastJobExecution(jobSchedule);
            Assert.That(lastExecution != null, "Expected a last execution record");
            Assert.That(lastExecution.Runtime > 0, "Expected a non-zero runtime duration");
        }

        [Test]
        public void TestSaveJobSchedule()
        {
            var jobSchedule = new JobSchedule()
            {
                RelativityScriptId = TEST_SCRIPT_ID,
                WorkspaceId = TEST_WORKSPACE_ID,
                ExecutionSchedule = 0x7F,
                ExecutionTime = JobSchedule.TimeSeconds(DateTime.Now),
                JobEnabled = true
            };

            var jobInputs = new List<JobScriptInput>()
            {
                new JobScriptInput()
                {
                    InputName = "scriptInput1",
                    InputValue = "scriptValue1"
                },
                new JobScriptInput()
                {
                    InputName = "scriptInput2",
                    InputValue = "scriptValue2"
                },
            };

            var result = this.JobScheduleRepository.SaveJobSchedule(jobSchedule, jobInputs);
            Assert.That(result > 0);
            Assert.That(this.JobScheduleRepository.GetJobInputs(jobSchedule).All(jsi => jsi.JobScheduleId != 0 && jsi.JobScheduleId == jobSchedule.Id));
        }
    }
}
