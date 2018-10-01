namespace Milyli.ScriptRunner.Services.Contracts.V1.Requests
{
	/// <summary>
	/// Request definition to read a script run's history.
	/// </summary>
	public class ReadHistoryRequest
	{
		/// <summary>
		/// Script run identifier.
		/// </summary>
		public int ScriptRunId { get; set; }
	}
}
