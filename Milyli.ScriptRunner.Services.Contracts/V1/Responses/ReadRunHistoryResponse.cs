namespace Milyli.ScriptRunner.Services.Contracts.V1.Responses
{
	using System;

	/// <summary>
	/// Response to Script Run history requests
	/// </summary>
	public class ReadRunHistoryResponse
	{
		/// <summary>
		/// Script Run History identifier.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Script Run identifier.
		/// </summary>
		public int ScriptRunId { get; set; }

		/// <summary>
		/// Start time of the execution in UTC.
		/// </summary>
		public DateTimeOffset StartTimeUTC { get; set; }

		/// <summary>
		/// Running time of the script execution.
		/// </summary>
		public int? Runtime { get; set; }
		
		/// <summary>
		/// Indicates errors.
		/// </summary>
		public bool HasError { get; set; }
		
		/// <summary>
		/// Result message.
		/// </summary>
		public string ResultText { get; set; }
	}
}
