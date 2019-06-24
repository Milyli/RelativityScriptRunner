namespace Milyli.ScriptRunner.Core.Tools
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Milyli.ScriptRunner.Core.Models;

	/// <summary>
	/// This class primarily serves to offer extension methods to <see cref="JobSchedule"/> for converting between/to local and UTC when displaying times to the user
	/// </summary>
	public static class ScheduleConversionHelper
	{
		private static readonly int SecondsInDay = (int)TimeSpan.FromDays(1).TotalSeconds;
		public static int LocalOffset
		{
			get { return (int)DateTimeOffset.Now.Offset.TotalSeconds; }
			private set { }
		}

		private static readonly int NumDays = 7;

		/// <summary>
		/// Convert a jobSchedule sent with local parameters for  ExecutionTime and Schedule to UTC
		/// </summary>
		/// <param name="jobSchedule">Local JobSchedule</param>
		/// <returns>UTC JobSchedule</returns>
		public static JobSchedule ConvertLocalToUtc(this JobSchedule jobSchedule)
		{
			int utcExecutionTime = jobSchedule.ExecutionTime - ScheduleConversionHelper.LocalOffset;
			jobSchedule.LastExecutionTime = jobSchedule.LastExecutionTime?.ToUniversalTime();
			jobSchedule.NextExecutionTime = jobSchedule.NextExecutionTime?.ToUniversalTime();
			return ShiftSchedule(jobSchedule, utcExecutionTime);
		}

		/// <summary>
		/// Convert a jobSchedule received with UTC parameters ExecutionTime and Schedule to local
		/// </summary>
		/// <param name="jobSchedule">UTC Jobschedule</param>
		/// <returns>Local Jobschedule</returns>
		public static JobSchedule ConvertUtcToLocal(this JobSchedule jobSchedule)
		{
			int utcExecutionTime = jobSchedule.ExecutionTime + ScheduleConversionHelper.LocalOffset;
			jobSchedule.LastExecutionTime = jobSchedule.LastExecutionTime?.ToLocalTime();
			jobSchedule.NextExecutionTime = jobSchedule.NextExecutionTime?.ToLocalTime();
			return ShiftSchedule(jobSchedule, utcExecutionTime);
		}

		/// <summary>
		/// Determine whether the ExecutionSchedule for a <see cref="JobSchedule"/> needs to be shifted based on UTC offset, and if so do it
		/// </summary>
		/// <param name="jobSchedule">the JobSchedule which will be modified</param>
		/// <param name="utcExecutionTime">local execution time with UTC offset added</param>
		/// <returns>the UTC Jobschedule</returns>
		public static JobSchedule ShiftSchedule(JobSchedule jobSchedule, int utcExecutionTime)
		{

			// we need to shift all the days "back"
			if (utcExecutionTime < 0)
			{
				jobSchedule.ExecutionTime = utcExecutionTime + ScheduleConversionHelper.SecondsInDay;
				jobSchedule.ExecutionSchedule = ShiftDaysLeft(jobSchedule.ExecutionSchedule);
			}
			else if (utcExecutionTime >= SecondsInDay)
			{
				// need to shift all days "forward"
				jobSchedule.ExecutionTime = utcExecutionTime - ScheduleConversionHelper.SecondsInDay;
				jobSchedule.ExecutionSchedule = ShiftDaysRight(jobSchedule.ExecutionSchedule);
			}
			else if (utcExecutionTime == 0)
			{
				// ambiguous case: if the TZ is ahead the days should not be shifted, if it is behind they should
				if (ScheduleConversionHelper.LocalOffset < 0)
				{
					// behind: add a day
					jobSchedule.ExecutionTime = utcExecutionTime;
					jobSchedule.ExecutionSchedule = ShiftDaysRight(jobSchedule.ExecutionSchedule);
				}
				else
				{
					// ahead: 3 am ahead -> 12am utc is the same schedule
					jobSchedule.ExecutionTime = utcExecutionTime;
				}
			}
			else
			{
				jobSchedule.ExecutionTime = utcExecutionTime;
			}

			return jobSchedule;
		}

		/// <summary>
		/// Moves "up" all the days in an executionschedule
		/// </summary>
		/// <param name="executionDays">the executionschedule to shift</param>
		/// <returns>the shifted executionschedule</returns>
		public static ExecutionDay ShiftDaysRight(ExecutionDay executionDays) =>
			ShiftDay(executionDays, day =>
				day == ExecutionDay.Saturday
					? ExecutionDay.Sunday
					: (ExecutionDay)((int)day << 1));

		/// <summary>
		/// Moves "down" all the days in an executionschedule
		/// </summary>
		/// <param name="executionDays">the executionschedule to shift</param>
		/// <returns>the shifted executionschedule</returns>
		public static ExecutionDay ShiftDaysLeft(ExecutionDay executionDays) =>
			ShiftDay(executionDays, day =>
				day == ExecutionDay.Sunday
					? ExecutionDay.Saturday
					: (ExecutionDay)((int)day >> 1));

		/// <summary>
		/// Shifts any flags on the <see cref="executionDay"/> using the shiftOperation
		/// </summary>
		/// <param name="executionDay">The day (or days) to shift.</param>
		/// <param name="shiftOperation">How to shift a set day</param>
		/// <returns>The shifted day (or days).</returns>
		private static ExecutionDay ShiftDay(
			ExecutionDay executionDay,
			Func<ExecutionDay, ExecutionDay> shiftOperation)
		{
			if (executionDay == ExecutionDay.All ||
				executionDay == ExecutionDay.None)
			{
				return executionDay;
			}

			var newSchedule = ExecutionDay.None;
			var allDays = Enum.GetValues(typeof(ExecutionDay))
				.OfType<ExecutionDay>()
				.Except(new[] { ExecutionDay.All, ExecutionDay.None });
			foreach (var thisDay in allDays)
			{
				if (executionDay.HasFlag(thisDay))
				{
					var shiftedDay = shiftOperation(thisDay);
					newSchedule = newSchedule | shiftedDay;
				}
			}

			return newSchedule;
		}
	}
}
