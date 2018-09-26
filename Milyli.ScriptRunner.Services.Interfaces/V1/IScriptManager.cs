namespace Milyli.ScriptRunner.Services.Interfaces.V1
{
	using System.Threading.Tasks;
	using Models.Requests;
	using Models.Responses;
	using Relativity.Kepler.Services;

	[WebService("ScriptApi")]
	[ServiceAudience(Audience.Public)]
	[RoutePrefix("API/V1/Script")]
	public interface IScriptManager
	{
		[Route("ReadSingle")]
		Task<ReadScriptResponse> GetScriptAsync(ReadScriptRequest req);

		[Route("ReadAll")]
		Task<ReadCaseScriptResponse> GetCaseScripts(ReadCaseScriptsRequest req);
	}
}
