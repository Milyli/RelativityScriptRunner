// <copyright file="JobScheduleRepositoryMock.cs" company="Milyli">
// Copyright © 2016 Milyli
// </copyright>

namespace Milyli.ScriptRunner.Core.Test.UnitTests.Mocks
{
    using System.Collections.Generic;
    using Milyli.ScriptRunner.Core.Models;
    using Milyli.ScriptRunner.Core.Repositories;
    using Moq;

    public class JobScheduleRepositoryMock
    {
        public JobScheduleRepositoryMock()
        {
            this.Mock = new Mock<IJobScheduleRepository>();
            this.CurrentJobSchedule = new JobSchedule();

            this.Mock.Setup(jsr => jsr.StartJob(It.IsAny<JobSchedule>())).Callback(() =>
            {
                this.CurrentJobSchedule.CurrentJobHistory = new JobHistory();
                this.JobScheduleStarted = true;
            })
            .Returns(JobActivationStatus.Started);

            this.Mock.Setup(jsr => jsr.GetJobInputs(It.IsAny<JobSchedule>())).Returns(new List<JobScriptInput>());

            this.Mock.Setup(jsr => jsr.FinishJob(It.IsAny<JobSchedule>())).Callback(() =>
            {
                this.JobScheduleFinished = true;
            });
        }

        public bool JobScheduleStarted { get; private set; } = false;

        public bool JobScheduleFinished { get; private set; } = false;

        public IJobScheduleRepository JobScheduleRepository
        {
            get
            {
                return this.Mock.Object;
            }
        }

        public JobSchedule CurrentJobSchedule { get; private set; }

        public Mock<IJobScheduleRepository> Mock { get; private set; }
    }
}
