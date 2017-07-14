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

    [TestFixture(Category = "Integration")]
    public class TestJobScheduleRepository : IntegrationTestFixture
    {
#pragma warning disable SA1310 // Field names must not contain underscore
        private static readonly int TEST_SCRIPT_ID = 42;

        private static readonly int TEST_WORKSPACE_ID = -1;
#pragma warning restore SA1310 // Field names must not contain underscore

        private IJobScheduleRepository JobScheduleRepository
        {
            get
            {
                return this.Container.GetInstance<IJobScheduleRepository>();
            }
        }

        [OneTimeTearDown]
        public void Cleanup()
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
                ExecutionTime = JobSchedule.TimeSeconds(DateTime.UtcNow)
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
                ExecutionTime = JobSchedule.TimeSeconds(DateTime.UtcNow.AddMinutes(-3)),
                NextExecutionTime = DateTime.UtcNow
						};

            jobSchedule.Id = this.JobScheduleRepository.Create(jobSchedule);
            var jobList = this.JobScheduleRepository.GetJobSchedules(DateTime.UtcNow);
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
                ExecutionTime = JobSchedule.TimeSeconds(DateTime.UtcNow.AddMinutes(-3)),
                NextExecutionTime = DateTime.UtcNow
						};

            var okAsWell = new JobSchedule()
            {
                RelativityScriptId = TEST_SCRIPT_ID,
                WorkspaceId = TEST_WORKSPACE_ID,
                ExecutionSchedule = 0x7F,
                ExecutionTime = JobSchedule.TimeSeconds(DateTime.UtcNow.AddMinutes(-3)),
                NextExecutionTime = DateTime.UtcNow.AddHours(-4)
            };

            var tooEarly = new JobSchedule()
            {
                RelativityScriptId = TEST_SCRIPT_ID,
                WorkspaceId = TEST_WORKSPACE_ID,
                ExecutionSchedule = 0x7F,
                ExecutionTime = JobSchedule.TimeSeconds(DateTime.UtcNow.AddMinutes(-3)),
                NextExecutionTime = DateTime.UtcNow.AddDays(-3)
            };

            var tooLate = new JobSchedule()
            {
                RelativityScriptId = TEST_SCRIPT_ID,
                WorkspaceId = TEST_WORKSPACE_ID,
                ExecutionSchedule = 0x7F,
                ExecutionTime = JobSchedule.TimeSeconds(DateTime.UtcNow.AddMinutes(3)),
                NextExecutionTime = DateTime.UtcNow.AddDays(3)
            };

            jobSchedule.Id = this.JobScheduleRepository.Create(jobSchedule);
            tooLate.Id = this.JobScheduleRepository.Create(tooLate);
            tooEarly.Id = this.JobScheduleRepository.Create(tooEarly);
            var jobList = this.JobScheduleRepository.GetJobSchedules(DateTime.UtcNow);
            var ids = jobList.Select(j => j.Id).ToList();

            Assert.That(ids.Contains(jobSchedule.Id), string.Format("There should be one job (id {0}) in the result", jobSchedule.Id));
            Assert.That(ids.Contains(tooEarly.Id), string.Format("Job id {0} expected but not found (fail on checking for old jobs", tooEarly.Id));
            Assert.That(!ids.Contains(tooLate.Id), string.Format("Did not expect jobs of ids {0}, but found those jobs (too late)", tooLate.Id));
        }

        [Test]
        public void TestJobFinish()
        {
            var jobSchedule = new JobSchedule()
            {
                RelativityScriptId = TEST_SCRIPT_ID,
                WorkspaceId = TEST_WORKSPACE_ID,
                ExecutionSchedule = 0x7F,
                ExecutionTime = JobSchedule.TimeSeconds(DateTime.UtcNow),
                NextExecutionTime = DateTime.UtcNow
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
                ExecutionTime = JobSchedule.TimeSeconds(DateTime.UtcNow),
                JobEnabled = true
            };

            var jobInputs = new List<JobScriptInput>()
            {
                new JobScriptInput()
                {
                    InputId = "scriptInput1",
                    InputName = "scriptInput1",
                    InputValue = "scriptValue1"
                },
                new JobScriptInput()
                {
                    InputId = "scriptInput2",
                    InputName = "scriptInput2",
                    InputValue = "scriptValue2"
                },
            };

            var result = this.JobScheduleRepository.SaveJobSchedule(jobSchedule, jobInputs);
            Assert.That(result > 0);
            Assert.That(this.JobScheduleRepository.GetJobInputs(jobSchedule).All(jsi => jsi.JobScheduleId != 0 && jsi.JobScheduleId == jobSchedule.Id));
        }

        [Test]
        public void TestActivateJob()
        {
            var jobSchedule = new JobSchedule()
            {
                RelativityScriptId = TEST_SCRIPT_ID,
                WorkspaceId = TEST_WORKSPACE_ID,
                ExecutionSchedule = 0x7F,
                ExecutionTime = JobSchedule.TimeSeconds(DateTime.UtcNow),
                JobEnabled = true
            };

            var jobInputs = new List<JobScriptInput>()
            {
                new JobScriptInput()
                {
                    InputId = "scriptInput1",
                    InputName = "scriptInput1",
                    InputValue = "scriptValue1"
                },
                new JobScriptInput()
                {
                    InputId = "scriptInput2",
                    InputName = "scriptInput2",
                    InputValue = "scriptValue2"
                },
            };
            var result = this.JobScheduleRepository.SaveJobSchedule(jobSchedule, jobInputs);
            this.JobScheduleRepository.ActivateJob(jobSchedule);
            Assert.That(jobSchedule.JobStatus == (int)JobStatus.Waiting);
        }

        [Test]
        public void TestCannotActivateRunningJob()
        {
            var jobSchedule = new JobSchedule()
            {
                RelativityScriptId = TEST_SCRIPT_ID,
                WorkspaceId = TEST_WORKSPACE_ID,
                ExecutionSchedule = 0x7F,
                ExecutionTime = JobSchedule.TimeSeconds(DateTime.UtcNow),
                JobEnabled = true
            };

            var jobInputs = new List<JobScriptInput>()
            {
                new JobScriptInput()
                {
                    InputId = "scriptInput1",
                    InputName = "scriptInput1",
                    InputValue = "scriptValue1"
                },
                new JobScriptInput()
                {
                    InputId = "scriptInput2",
                    InputName = "scriptInput2",
                    InputValue = "scriptValue2"
                },
            };
            var result = this.JobScheduleRepository.SaveJobSchedule(jobSchedule, jobInputs);
            var status = this.JobScheduleRepository.StartJob(jobSchedule);
            Assert.That(status == JobActivationStatus.Started);
            var activationStatus = this.JobScheduleRepository.ActivateJob(jobSchedule);
            Assert.That(activationStatus == JobActivationStatus.AlreadyRunning);
        }

        [Test]
        public void TestJobHistory()
        {
            var jobSchedule = new JobSchedule()
            {
                RelativityScriptId = TEST_SCRIPT_ID,
                WorkspaceId = TEST_WORKSPACE_ID,
                ExecutionSchedule = 0x7F,
                ExecutionTime = JobSchedule.TimeSeconds(DateTime.UtcNow),
                JobEnabled = true
            };

            var jobInputs = new List<JobScriptInput>()
            {
                new JobScriptInput()
                {
                    InputId = "scriptInput1",
                    InputName = "scriptInput1",
                    InputValue = "scriptValue1"
                },
                new JobScriptInput()
                {
                    InputId = "scriptInput2",
                    InputName = "scriptInput2",
                    InputValue = "scriptValue2"
                },
            };
            var result = this.JobScheduleRepository.SaveJobSchedule(jobSchedule, jobInputs);
            this.JobScheduleRepository.StartJob(jobSchedule);
            this.JobScheduleRepository.FinishJob(jobSchedule);
            int count;
            this.JobScheduleRepository.GetJobHistory(jobSchedule, out count);
            Assert.That(count == 1);
        }

        [Test]
        public void TestJobHistoryPaging()
        {
            var jobSchedule = new JobSchedule()
            {
                RelativityScriptId = TEST_SCRIPT_ID,
                WorkspaceId = TEST_WORKSPACE_ID,
                ExecutionSchedule = 0x7F,
                ExecutionTime = JobSchedule.TimeSeconds(DateTime.UtcNow),
                JobEnabled = true
            };

            var jobInputs = new List<JobScriptInput>()
            {
                new JobScriptInput()
                {
                    InputId = "scriptInput1",
                    InputName = "scriptInput1",
                    InputValue = "scriptValue1"
                },
                new JobScriptInput()
                {
                    InputId = "scriptInput2",
                    InputName = "scriptInput2",
                    InputValue = "scriptValue2"
                },
            };
            var result = this.JobScheduleRepository.SaveJobSchedule(jobSchedule, jobInputs);
            for (int x = 0; x < 50; x++)
            {
                this.JobScheduleRepository.StartJob(jobSchedule);
                this.JobScheduleRepository.FinishJob(jobSchedule);
            }

            int count;
            var page = this.JobScheduleRepository.GetJobHistory(jobSchedule, out count, 0, 10);
            Assert.That(count == 50);
            var nextPage = this.JobScheduleRepository.GetJobHistory(jobSchedule, out count, 1, 10).ToDictionary(jh => jh.Id, jh => true);
            Assert.That(page.All(p => !nextPage.ContainsKey(p.Id)), "Page should be distinct");
        }

        [Test]
        public void TestJobHistoryDoesNotPagePastEnd()
        {
            var jobSchedule = new JobSchedule()
            {
                RelativityScriptId = TEST_SCRIPT_ID,
                WorkspaceId = TEST_WORKSPACE_ID,
                ExecutionSchedule = 0x7F,
                ExecutionTime = JobSchedule.TimeSeconds(DateTime.UtcNow),
                JobEnabled = true
            };

            var jobInputs = new List<JobScriptInput>()
            {
                new JobScriptInput()
                {
                    InputId = "scriptInput1",
                    InputName = "scriptInput1",
                    InputValue = "scriptValue1"
                },
                new JobScriptInput()
                {
                    InputId = "scriptInput2",
                    InputName = "scriptInput2",
                    InputValue = "scriptValue2"
                },
            };
            var result = this.JobScheduleRepository.SaveJobSchedule(jobSchedule, jobInputs);
            for (int x = 0; x < 50; x++)
            {
                this.JobScheduleRepository.StartJob(jobSchedule);
                this.JobScheduleRepository.FinishJob(jobSchedule);
            }

            int count;
            this.JobScheduleRepository.GetJobHistory(jobSchedule, out count, 0, 10);
            var page = this.JobScheduleRepository.GetJobHistory(jobSchedule, out count, 11, 10);
            Assert.That(page.Count == 0);
        }
    }
}
