namespace Milyli.ScriptRunner.Core.Models
{
	/// <summary>
	/// Represents the results of a Relativity Script execution.
	/// </summary>
	public class ExecuteResult
	{
		/// <summary>
		/// Gets or sets a value indicating whether the execution was successful.
		/// </summary>
		public bool Success { get; set; }

		/// <summary>
		/// Gets or sets a message containing information about the execution.
		/// </summary>
		/// <remarks>If executed by RSAPI this may be e.g. a) a stack trace of a SQL
		/// exception, b) validation error, etc. If executed through direct SQL this is
		/// only set if there was an exception thrown during execution.</remarks>
		public string Message { get; set; }
	}
}
