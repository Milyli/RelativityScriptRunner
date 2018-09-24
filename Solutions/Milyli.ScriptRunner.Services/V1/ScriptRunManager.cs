namespace Milyli.ScriptRunner.Services.V1
{
	using System.Threading.Tasks;
	using Interfaces.Models.Responses;
	using Interfaces.V1;

	public class ScriptRunManager : IScriptRunManager
	{
		public Task<ScriptRunResponse> GetScriptRunAsync(int scriptRunId)
		{
			throw new System.NotImplementedException();
		}

		public Task CreateScriptRunAsync()
		{
			throw new System.NotImplementedException();
		}

		public Task<RunHistoryResponse> GetRunHistoryAsync(int scriptRunId)
		{
			throw new System.NotImplementedException();
		}

		public Task<int> UpdateScriptRunAsync()
		{
			throw new System.NotImplementedException();
		}

		public Task RunAsync(int scriptRunId)
		{
			throw new System.NotImplementedException();
		}

		public Task RunAllAsync()
		{
			throw new System.NotImplementedException();
		}
	}
}
