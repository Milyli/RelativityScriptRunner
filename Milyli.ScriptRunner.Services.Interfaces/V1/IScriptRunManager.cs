namespace Milyli.ScriptRunner.Services.Interfaces.V1
{
	using System.Threading.Tasks;
	using Models.Responses;
	using Relativity.Kepler.Services;

	[WebService("ScriptRunApi")]
	[ServiceAudience(Audience.Public)]
	[RoutePrefix("API/ScriptRun")]
    public interface IScriptRunManager
	{
		[Route("Read")]
		Task<ScriptRunResponse> GetScriptRunAsync(int scriptRunId);

		[Route("Create")]
		Task CreateScriptRunAsync();

		[Route("History")]
		Task<RunHistoryResponse> GetRunHistoryAsync(int scriptRunId);

		[Route("Update")]
		Task<int> UpdateScriptRunAsync();

		[Route("Run")]
		Task RunAsync(int scriptRunId);

		[Route("RunAll")]
		Task RunAllAsync();
	}
}
