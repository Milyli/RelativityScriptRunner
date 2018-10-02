namespace Milyli.ScriptRunner.Services.Interfaces.V1
{
	using System;
	using System.Threading.Tasks;
	using Contracts.V1.Requests;
	using Contracts.V1.Responses;
	using Relativity.Kepler.Services;

	/// <summary>
	/// Handles Script Run CRUD operations, as well as History and Execution operations.
	/// </summary>
	[WebService("ScriptRunApi")]
	[ServiceAudience(Audience.Public)]
	[RoutePrefix("ScriptRun")]
    public interface IScriptRunManager
	{
		/// <summary>
		/// Read a single Script Run.
		/// </summary>
		/// <param name="scriptRunId">Script run to read.</param>
		/// <returns>Script Run definition.</returns>
		[Route("Read")]
		Task<ScriptRunResponse> ReadSingleAsync(int scriptRunId);

		/// <summary>
		/// Creates a new Script Run.
		/// </summary>
		/// <param name="req">Creation request.</param>
		/// <returns>Newly created Script Run.</returns>
		[Route("Create")]
		Task<ScriptRunResponse> CreateSingleAsync(ScriptRunRequest req);

		/// <summary>
		/// Gets the history for a single Script Run.
		/// </summary>
		/// <param name="scriptRunId">Script Run to read history of.</param>
		/// <returns>Run History.</returns>
		[Route("ReadHistory")]
		Task<ReadRunHistoryResponse> GetRunHistoryAsync(int scriptRunId);

		/// <summary>
		/// Updates definition of a single Script Run.
		/// </summary>
		/// <param name="req">Update Request.</param>
		/// <returns>Updated Script Definition.</returns>
		[Route("Update")]
		Task<ScriptRunResponse> UpdateSingleAsync(ScriptRunRequest req);

		/// <summary>
		/// Execute a single Script Run on-demand.
		/// </summary>
		/// <param name="scriptRunId">Script Run to execute.</param>
		/// <returns></returns>
		[Route("Run")]
		Task RunSingleAsync(int scriptRunId);

		/// <summary>
		/// Execute all Script Runs that are scheduled.
		/// </summary>
		/// <param name="runTimeUtc">Run Time defining which Script Runs to execute.
		/// All unrun Runs scheduled for or prior to the run time should be executed.</param>
		/// <returns></returns>
		[Route("RunAll")]
		Task RunAllAsync(DateTime runTimeUtc);
	}
}
