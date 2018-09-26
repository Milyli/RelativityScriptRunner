namespace Milyli.ScriptRunner.Services.Interfaces.V1.Models
{
	/// <summary>
	/// Defines a relativity script input.
	/// </summary>
	public class Input
	{
		/// <summary>
		/// Name of the script
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Data type of the input.
		/// </summary>
		public string InputType { get; set; }

		/// <summary>
		/// Indicates if the input is required.
		/// </summary>
		public bool IsRequired { get; set; }
	}
}
