namespace Milyli.ScriptRunner.Core.Tools
{
	using System;
	using System.Collections.Generic;
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

		private static readonly List<int> DayBits = new List<int> { 1, 2, 4, 8, 16, 32, 64 };
		private static readonly int NumDays = 7;
		private static readonly int AllDays = 127;

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
		/// <param name="executionSchedule">the executionschedule to shift</param>
		/// <returns>the shifted executionschedule</returns>
		public static int ShiftDaysRight(int executionSchedule)
		{
			int returnSchedule = 0;
			if (executionSchedule == AllDays)
			{
				return AllDays;
			}
			else
			{
				for (int i = 0; i < ScheduleConversionHelper.NumDays - 1; ++i)
				{
					if ((executionSchedule & DayBits[i]) == DayBits[i])
					{
						returnSchedule = returnSchedule | DayBits[i + 1];
					}
				}

				if ((executionSchedule & DayBits[6]) == DayBits[6])
				{
					returnSchedule = returnSchedule | DayBits[0];
				}
			}
			return returnSchedule;
		}

		/// <summary>
		/// Moves "down" all the days in an executionschedule
		/// </summary>
		/// <param name="executionSchedule">the executionschedule to shift</param>
		/// <returns>the shifted executionschedule</returns>
		public static int ShiftDaysLeft(int executionSchedule)
		{
			int returnSchedule = 0;
			if (executionSchedule == AllDays)
			{
				return AllDays;
			}
			else
			{
				for (int i = 1; i < ScheduleConversionHelper.NumDays; ++i)
				{
					if ((executionSchedule & DayBits[i]) == DayBits[i])
					{
						returnSchedule = returnSchedule | DayBits[i - 1];
					}
				}

				if ((executionSchedule & DayBits[0]) == DayBits[0])
				{
					returnSchedule = returnSchedule | DayBits[6];
				}
			}

			return returnSchedule;
		}
	}
}
