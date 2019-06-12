namespace Milyli.ScriptRunner.Core.Services
{
	using System;
	using Milyli.ScriptRunner.Core.Models;

	public interface IRelativityScriptRunner
	{
		/// <summary>
		/// Executes a <see cref="JobSchedule"/> run through direct SQL instead of executing
		/// scripts through RSAPI.
		/// </summary>
		/// <param name="job"><see cref="JobSchedule"/> to run.</param>
		void ExecuteDirectSqlJob(JobSchedule job);

		/// <summary>
		/// Executes a <see cref="JobSchedule"/> by running the Relativity Script through RSAPI.
		/// </summary>
		/// <param name="job"><see cref="JobSchedule"/> to run.</param>
		void ExecuteScriptJob(JobSchedule job);

		/// <summary>
		/// Executes all jobs up scheduled to the specified time.
		/// </summary>
		/// <param name="executionTime">Time which specifies which scheduled jobs will be executed.</param>
		void ExecuteAllJobs(DateTime executionTime);
	}
}
