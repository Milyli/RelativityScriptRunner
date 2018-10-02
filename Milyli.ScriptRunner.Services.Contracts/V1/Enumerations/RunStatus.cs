namespace Milyli.ScriptRunner.Services.Contracts.V1.Enumerations
{
	/// <summary>
	/// Describes status of a Script Run.
	/// </summary>
	public enum RunStatus
	{
		/// <summary>
		/// Script run is idle.
		/// </summary>
		Idle = 0,

		/// <summary>
		/// Script run is ready to execute and waiting to be picked up by an agent.
		/// </summary>
		Waiting = 1,

		/// <summary>
		/// Script run is executing.
		/// </summary>
		Running = 2
	}
}
