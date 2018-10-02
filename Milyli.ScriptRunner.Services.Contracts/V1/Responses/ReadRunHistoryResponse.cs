namespace Milyli.ScriptRunner.Services.Contracts.V1.Responses
{
	using System.Collections.Generic;

	/// <summary>
	/// Response to Script Run history requests
	/// </summary>
	public class ReadRunHistoryResponse
	{
		/// <summary>
		/// Collection of all histories for the run.
		/// </summary>
		public List<ScriptRunHistory> RunHistory { get; set; }
	}
}
