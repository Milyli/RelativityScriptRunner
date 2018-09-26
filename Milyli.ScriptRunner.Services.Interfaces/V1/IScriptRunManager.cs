namespace Milyli.ScriptRunner.Services.Interfaces.V1
{
	using System.Threading.Tasks;
	using Models.Requests;
	using Models.Responses;
	using Relativity.Kepler.Services;

	/// <summary>
	/// Handles Script Run CRUD operations, as well as History and Execution operations.
	/// </summary>
	[WebService("ScriptRunApi")]
	[ServiceAudience(Audience.Public)]
	[RoutePrefix("API/V1/ScriptRun")]
    public interface IScriptRunManager
	{
		/// <summary>
		/// Read a single Script Run.
		/// </summary>
		/// <param name="req">Request defining the single script run to return.</param>
		/// <returns>Script Run definition.</returns>
		[Route("Read")]
		Task<ReadScriptRunResponse> GetScriptRunAsync(ReadScriptRunRequest req);

		/// <summary>
		/// Creates a new Script Run.
		/// </summary>
		/// <param name="req">Creation request.</param>
		/// <returns>Newly created Script Run.</returns>
		[Route("Create")]
		Task<CreateScriptRunResponse> CreateScriptRunAsync(CreateScriptRunRequest req);

		/// <summary>
		/// Gets the history for a single Script Run.
		/// </summary>
		/// <param name="req">History request.</param>
		/// <returns>Run History.</returns>
		[Route("History")]
		Task<ReadRunHistoryResponse> GetRunHistoryAsync(ReadHistoryRequest req);

		/// <summary>
		/// Updates definition of a single Script Run.
		/// </summary>
		/// <param name="req">Update Request.</param>
		/// <returns>Updated Script Definition.</returns>
		[Route("Update")]
		Task<UpdateScriptRunResponse> UpdateScriptRunAsync(UpdateScriptRunRequest req);

		/// <summary>
		/// Execute a single Script Run on-demand.
		/// </summary>
		/// <param name="req">Run Request indicating which script run to execute.</param>
		/// <returns></returns>
		[Route("Run")]
		Task RunAsync(RunScriptRunRequest req);

		/// <summary>
		/// Execute all Script Runs that are scheduled.
		/// </summary>
		/// <param name="req">Run All Request.</param>
		/// <returns></returns>
		[Route("RunAll")]
		Task RunAllAsync(RunAllRequest req);
	}
}
