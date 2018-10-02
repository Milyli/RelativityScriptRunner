namespace Milyli.ScriptRunner.Services.Contracts.V1
{
	using System;

	/// <summary>
	/// Defines a single past execution of a Script Run.
	/// </summary>
	public class ScriptRunHistory
	{
		/// <summary>
		/// Start time of the execution in UTC.
		/// </summary>
		public DateTime StartTimeUtc { get; set; }

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
