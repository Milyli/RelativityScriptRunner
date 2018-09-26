namespace Milyli.ScriptRunner.Services.Interfaces.V1.Models.Requests
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
