namespace Milyli.ScriptRunner.Services.Interfaces.V1
{
	using System.Threading.Tasks;
	using Models.Requests;
	using Models.Responses;
	using Relativity.Kepler.Services;

	/// <summary>
	/// Utility for querying relativity scripts.
	/// </summary>
	[WebService("ScriptApi")]
	[ServiceAudience(Audience.Public)]
	[RoutePrefix("API/V1/Script")]
	public interface IScriptManager
	{
		/// <summary>
		/// Gets a single script by id.
		/// </summary>
		/// <param name="req">Request defining script id and case id.</param>
		/// <returns>Script and its associated script runs.</returns>
		[Route("ReadSingle")]
		Task<ReadScriptResponse> GetScriptAsync(ReadScriptRequest req);

		/// <summary>
		/// Gets all scripts for a case.
		/// </summary>
		/// <param name="req">Request defining the case to read scripts from.</param>
		/// <returns>All scripts in the case.</returns>
		[Route("ReadAll")]
		Task<ReadCaseScriptResponse> GetCaseScripts(ReadCaseScriptsRequest req);
	}
}
