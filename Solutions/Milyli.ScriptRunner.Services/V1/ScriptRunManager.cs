namespace Milyli.ScriptRunner.Services.V1
{
	using System.Threading.Tasks;
	using Interfaces.Models.Requests;
	using Interfaces.Models.Responses;
	using Interfaces.V1;

	public class ScriptRunManager : IScriptRunManager
	{
		public Task<ReadScriptRunResponse> GetScriptRunAsync(ReadScriptRunRequest req)
		{
			throw new System.NotImplementedException();
		}

		public Task<CreateScriptRunResponse> CreateScriptRunAsync(CreateScriptRunRequest req)
		{
			throw new System.NotImplementedException();
		}

		public Task<ReadRunHistoryResponse> GetRunHistoryAsync(ReadHistoryRequest req)
		{
			throw new System.NotImplementedException();
		}

		public Task<UpdateScriptRunResponse> UpdateScriptRunAsync(UpdateScriptRunRequest req)
		{
			throw new System.NotImplementedException();
		}

		public Task RunAsync(RunScriptRunRequest req)
		{
			throw new System.NotImplementedException();
		}

		public Task RunAllAsync(RunAllRequest req)
		{
			throw new System.NotImplementedException();
		}
	}
}
