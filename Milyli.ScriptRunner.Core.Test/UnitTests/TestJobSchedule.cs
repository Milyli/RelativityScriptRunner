// <copyright file="TestJobSchedule.cs" company="Milyli">
// Copyright © 2016 Milyli
// </copyright>

namespace Milyli.ScriptRunner.Core.Test.UnitTests
{
    using System;
    using Milyli.ScriptRunner.Core.Models;
    using NUnit.Framework;

    [TestFixture]
    public class TestJobSchedule
    {
        [Test]
        public void TestBadSchedule()
        {
            var jobSchedule = new JobSchedule();
            var nextExecutionTime = jobSchedule.GetNextExecution(DateTime.Now);
            Assert.That(!nextExecutionTime.HasValue, "Empty schedule entry should have no next execution time");
        }

        [Test]
        public void TestNextWeekExecution()
        {
            var now = DateTime.Now;
            var timeOfDaySeconds = JobSchedule.TimeSeconds(now);
            var execNow = now.Date.Add(JobSchedule.TimeOfDay(timeOfDaySeconds));

            var jobSchedule = new JobSchedule()
            {
                ExecutionTime = timeOfDaySeconds,
                ExecutionSchedule = 1 << (int)now.DayOfWeek
            };

            var nextExecutionTime = jobSchedule.GetNextExecution(execNow);
            var expectedResult = execNow.AddDays(7);
            Assert.That(nextExecutionTime.Equals(expectedResult), string.Format("Expected next execution time of {0}, got {1}", expectedResult, nextExecutionTime));
        }

        [Test]
        public void TestEverydayExecution()
        {
            var now = DateTime.Now;
            var timeOfDaySeconds = JobSchedule.TimeSeconds(now);
            var execNow = now.Date.Add(JobSchedule.TimeOfDay(timeOfDaySeconds));

            var jobSchedule = new JobSchedule()
            {
                ExecutionTime = timeOfDaySeconds,

                // 0111 1111 in hex
                ExecutionSchedule = 0x7F
            };

            var nextExecutionTime = jobSchedule.GetNextExecution(execNow);
            var expectedResult = execNow.AddDays(1);
            Assert.That(nextExecutionTime.Equals(expectedResult), string.Format("Expected next execution time of {0}, got {1}", expectedResult, nextExecutionTime));
        }

        [Test]
        public void TestSunday()
        {
            var now = DateTime.Now;
            var timeOfDaySeconds = JobSchedule.TimeSeconds(now);
            var execNow = now.Date.AddDays(6 - (int)now.DayOfWeek).Add(JobSchedule.TimeOfDay(timeOfDaySeconds));

            var jobsSchedule = new JobSchedule()
            {
                ExecutionTime = timeOfDaySeconds,
                ExecutionSchedule = 1
            };
            var nextExecutionTime = jobsSchedule.GetNextExecution(execNow);
            var expectedResult = execNow.AddDays(1);
            Assert.That(nextExecutionTime.Equals(expectedResult), string.Format("Expected next execution time of {0}, got {1}", expectedResult, nextExecutionTime));
        }
    }
}
