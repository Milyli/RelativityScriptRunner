namespace Milyli.ScriptRunner.Services.Interfaces.V1
{
	using System.Threading.Tasks;
	using Contracts.V1.Responses;
	using Relativity.Kepler.Services;

	/// <summary>
	/// Utility for querying relativity scripts.
	/// </summary>
	[WebService("ScriptApi")]
	[ServiceAudience(Audience.Public)]
	[RoutePrefix("Script")]
	public interface IScriptManager
	{
		/// <summary>
		/// Gets a single script by id.
		/// </summary>
		/// <param name="caseId">Id of the case the script belongs to.</param>
		/// <param name="scriptId">Id of the script within the case.</param>
		/// <returns>Script and its associated script runs.</returns>
		[Route("Read")]
		Task<ReadScriptResponse> ReadSingleAsync(int caseId, int scriptId);

		/// <summary>
		/// Gets all scripts for a case.
		/// </summary>
		/// <param name="caseId">Id of the case whose scripts will be read.</param>
		/// <returns>All scripts in the case.</returns>
		[Route("ReadByCase")]
		Task<ReadCaseScriptResponse> GetCaseScriptsAsync(int caseId);
	}
}
