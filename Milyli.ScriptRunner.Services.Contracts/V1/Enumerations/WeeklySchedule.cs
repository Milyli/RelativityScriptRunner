namespace Milyli.ScriptRunner.Services.Contracts.V1.Enumerations
{
	using System;

	/// <summary>
	/// Serves as flags to indicate some number of distinct days of the week
	/// </summary>
	[Flags]
	public enum WeeklySchedule
	{
		Sunday = 1,
		Monday = 2,
		Tuesday = 4,
		Wednesday = 8,
		Thursday = 16,
		Friday = 32,
		Saturday = 64
	}
}
