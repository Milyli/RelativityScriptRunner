namespace Milyli.ScriptRunner.Services.V1
{
	using System.Threading.Tasks;
	using Interfaces.Models.Requests;
	using Interfaces.Models.Responses;
	using Interfaces.V1;

	public class ScriptManager : IScriptManager
	{
		public Task<ReadScriptResponse> GetScriptAsync(ReadScriptRequest req)
		{
			throw new System.NotImplementedException();
		}

		public Task<ReadCaseScriptResponse> GetCaseScripts(ReadCaseScriptsRequest req)
		{
			throw new System.NotImplementedException();
		}
	}
}
