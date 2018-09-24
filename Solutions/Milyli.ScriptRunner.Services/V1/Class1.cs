namespace Milyli.ScriptRunner.Services.V1
{
	using System.Threading.Tasks;
	using Interfaces.Models.Responses;
	using Interfaces.V1;

	public class ScriptManager : IScriptManager
	{
		public Task<ScriptResponse> GetScriptAsync(int caseId, int scriptId)
		{
			throw new System.NotImplementedException();
		}

		public Task<CaseScriptResponse> GetCaseScripts(int caseId)
		{
			throw new System.NotImplementedException();
		}
	}
}
