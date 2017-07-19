using System;
using System.Collections.Generic;
using Milyli.ScriptRunner.Core.Models;

namespace Milyli.ScriptRunner.Core.Test.UnitTests
{
	using Milyli.ScriptRunner.Core.Tools;
	using NUnit.Framework;

	[TestFixture(Category = "Unit")]
	public class TestScheduleConversionHelper
	{

		private readonly int dayInSeconds = 86400;

		[TestCase(75, 23)]	// Su/M/W/Sat -> Su/M/Tu/Th
		[TestCase(23, 46)] // Su/M/Tu/Th -> M/Tu/W/Fr
		[TestCase(64, 1)] // Sat -> Sun
		[TestCase(127, 127)] // all -> all
		[Description("Test shifting of the week bitmasks to the right ('add' day)")]
		public void TestShiftRight(int local, int utc)
		{
			int shifted = ScheduleConversionHelper.ShiftDaysRight(local);
			Assert.AreEqual(utc, shifted, string.Format("Got {0} for schedule bitmask, expected {1}", shifted, utc));
		}

		[TestCase(75, 101)] // Su/M/W/Sat -> Su/Tu/Fri/Sat
		[TestCase(21, 74)] // Su/Tu/Th -> M/W/Sat
		[TestCase(1, 64)] // Sun -> Sat
		[TestCase(127, 127)] // All->all
		[Description("Test shifting of the week bitmasks to the left ('Subtract' day)")]
		public void TestShiftLeft(int local, int utc)
		{
			int shifted = ScheduleConversionHelper.ShiftDaysLeft(local);
			Assert.AreEqual(utc, shifted, string.Format("Got {0} for schedule bitmask, expected {1}", shifted, utc));
		}

		[Test]
		[Description("Test shifting a schedule which is behind UTC time")]
		public void TestShiftingSchedulesBehindUtc()
		{
			JobSchedule localSchedule = new JobSchedule();
			localSchedule.ExecutionTime = 70000;
			localSchedule.ExecutionSchedule = 23; // Su/M/Tu/Th

			int utcOffset = -18000;	// CDT offset
			int expectedExecutionTime = localSchedule.ExecutionTime - utcOffset - dayInSeconds;
			int expectedSchedule = 46; // M/Tu/W/Fr

			JobSchedule utcSchedule = ScheduleConversionHelper.ShiftSchedule(localSchedule, localSchedule.ExecutionTime - utcOffset);

			Assert.AreEqual(expectedExecutionTime, utcSchedule.ExecutionTime, string.Format("Got {0} for UTC Execution time, expected {1}", utcSchedule.ExecutionTime, expectedExecutionTime));
			Assert.AreEqual(expectedSchedule, utcSchedule.ExecutionSchedule, string.Format("Got {0} for UTC Execution schedule, expected {1}", utcSchedule.ExecutionSchedule, expectedSchedule));
		}

		[Test]
		[Description("Test shifting a schedule which is ahead of UTC time")]
		public void TestShiftingSchedulesAheadUtc()
		{
			JobSchedule localSchedule = new JobSchedule();
			localSchedule.ExecutionTime = 1400;
			localSchedule.ExecutionSchedule = 21; // Su/Tu/Th

			int utcOffset = 18000;  // CDT offset
			int expectedExecutionTime = localSchedule.ExecutionTime - utcOffset + dayInSeconds;
			int expectedSchedule = 74; // M/W/Sat

			JobSchedule utcSchedule = ScheduleConversionHelper.ShiftSchedule(localSchedule, localSchedule.ExecutionTime - utcOffset);

			Assert.AreEqual(expectedExecutionTime, utcSchedule.ExecutionTime, string.Format("Got {0} for UTC Execution time, expected {1}", utcSchedule.ExecutionTime, expectedExecutionTime));
			Assert.AreEqual(expectedSchedule, utcSchedule.ExecutionSchedule, string.Format("Got {0} for UTC Execution schedule, expected {1}", utcSchedule.ExecutionSchedule, expectedSchedule));
		}

		[Test]
		[Description("Make sure that if we convert local to UTC and then re-convert we get original local schedule out")]
		public void TestConversionsCancelOut()
		{
			JobSchedule firstLocalSchedule = new JobSchedule();
			firstLocalSchedule.ExecutionTime = 70000;
			firstLocalSchedule.ExecutionSchedule = 23;
			JobSchedule secondLocalSchedule = new JobSchedule();
			secondLocalSchedule.ExecutionTime = 10000;
			secondLocalSchedule.ExecutionSchedule = 23;

			JobSchedule firstUtcSchedule = ScheduleConversionHelper.ConvertLocalToUtc(firstLocalSchedule);
			JobSchedule secondUtcSchedule = ScheduleConversionHelper.ConvertLocalToUtc(secondLocalSchedule);
			JobSchedule firstLocalFromUtc = ScheduleConversionHelper.ConvertUtcToLocal(firstUtcSchedule);
			JobSchedule secondLocalFromUtc = ScheduleConversionHelper.ConvertUtcToLocal(secondUtcSchedule);

			Assert.AreEqual(firstLocalSchedule.ExecutionTime, firstLocalFromUtc.ExecutionTime);
			Assert.AreEqual(secondLocalSchedule.ExecutionTime, secondUtcSchedule.ExecutionTime);
			Assert.AreEqual(firstLocalSchedule.ExecutionSchedule, firstLocalFromUtc.ExecutionSchedule);
			Assert.AreEqual(secondLocalSchedule.ExecutionSchedule, secondLocalFromUtc.ExecutionSchedule);
		}

	}
}