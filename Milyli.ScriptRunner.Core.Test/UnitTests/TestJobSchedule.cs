// <copyright file="TestJobSchedule.cs" company="Milyli">
// Copyright © 2016 Milyli
// </copyright>

namespace Milyli.ScriptRunner.Core.Test.UnitTests
{
    using System;
	using System.Collections.Generic;
	using System.Linq;
	using Milyli.ScriptRunner.Core.Models;
	using Milyli.ScriptRunner.Core.Tools;
	using NUnit.Framework;

    [TestFixture(Category="Unit")]
    public class TestJobSchedule
    {
        [Test]
        public void TestBadSchedule()
        {
            var jobSchedule = new JobSchedule();
            var nextExecutionTime = jobSchedule.GetNextExecution(DateTime.UtcNow);
            Assert.That(!nextExecutionTime.HasValue, "Empty schedule entry should have no next execution time");
        }

        [Test]
        public void TestNextWeekExecution()
        {
            var now = DateTime.UtcNow;
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
            var now = DateTime.UtcNow;
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
            var now = DateTime.UtcNow;
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

		public class RoundTripScheduling
		{
			public static IEnumerable<TestCaseData> RoundTripScheduling_TestCaseSource
			{
				get
				{
					var days = new[]
					{
						new
						{
							Name = "sunday",
							Date = 17,
							Bit = 1
						},
						new
						{
							Name = "thursday",
							Date = 21,
							Bit = 16
						},
						new
						{
							Name = "saturday",
							Date = 23,
							Bit = 64
						}
					};

					foreach (var thisDay in days)
					{
						foreach (var thisHour in Enumerable.Range(0, 24))
						{
							yield return new TestCaseData(
								thisDay.Bit,
								((60 * thisHour) + 20) * 60, // HH:20am
								DateTime.Parse($"03/{thisDay.Date}/2019 00:05:00"),
								DateTime.Parse($"03/{thisDay.Date}/2019 {thisHour}:20:00"))
							{
								TestName = $"Later on the same day ({thisDay.Name}) @ {thisHour:00}",
							};

							yield return new TestCaseData(
								thisDay.Bit,
								((60 * thisHour) + 20) * 60, // HH:20am
								DateTime.Parse($"03/{thisDay.Date}/2019 23:55:00"),
								DateTime.Parse($"03/{thisDay.Date + 7}/2019 {thisHour}:20:00"))
							{
								TestName = $"Earlier on the same day ({thisDay.Name}) @ {thisHour:00}",
							};
						}
					}
				}
			}

			[TestCaseSource(nameof(RoundTripScheduling_TestCaseSource))]
			public void CanRoundTrip(
				int executionSchedule,
				int executionTime,
				DateTime now,
				DateTime expectedNextExecutionTime)
			{
				var schedule = new JobSchedule
				{
					ExecutionSchedule = executionSchedule,
					ExecutionTime = executionTime
				};

				var utcSchedule = ScheduleConversionHelper.ConvertLocalToUtc(schedule);
				utcSchedule.NextExecutionTime = utcSchedule.GetNextExecution(now);

				var roundTripSchedule = ScheduleConversionHelper.ConvertUtcToLocal(utcSchedule);

				Assert.That(roundTripSchedule.NextExecutionTime,
					Is.EqualTo(expectedNextExecutionTime)
						.Within(1).Minutes);
			}
		}
    }
}
