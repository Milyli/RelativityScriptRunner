namespace Milyli.ScriptRunner.Services.Interfaces.V1
{
	using System.Threading.Tasks;
	using Models.Responses;
	using Relativity.Kepler.Services;

	[WebService("ScriptApi")]
	[ServiceAudience(Audience.Public)]
	[RoutePrefix("API/Script")]
	public interface IScriptManager
	{
		[Route("ReadSingle")]
		Task<ScriptResponse> GetScriptAsync(int caseId, int scriptId);

		[Route("ReadAll")]
		Task<CaseScriptResponse> GetCaseScripts(int caseId);
	}
}
