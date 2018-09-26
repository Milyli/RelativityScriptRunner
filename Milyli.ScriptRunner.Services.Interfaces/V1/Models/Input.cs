namespace Milyli.ScriptRunner.Services.Interfaces.V1.Models
{
	public class Input
	{
		public string Name { get; set; }

		public string InputId { get; internal set; }

		public string InputType { get; set; }

		public bool IsRequired { get; set; }
	}
}
