namespace Milyli.ScriptRunner.Services.Contracts.V1
{
	/// <summary>
	/// Defines a relativity script input.
	/// </summary>
	public class Input
	{
		/// <summary>
		/// Script Run input identifier.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Relativity script input identifier.
		/// </summary>
		public int RelativityInputId { get; set; }

		/// <summary>
		/// Name of the input field.
		/// Read only property.
		/// </summary>
		public string InputName { get; set; }

		/// <summary>
		/// Current input value.
		/// </summary>
		public string InputValue { get; set; }
	}
}
