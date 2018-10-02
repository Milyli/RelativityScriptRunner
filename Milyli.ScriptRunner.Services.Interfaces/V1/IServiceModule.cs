namespace Milyli.ScriptRunner.Services.Interfaces.V1
{
	using Relativity.Kepler.Services;

	/// <summary>
	/// Service module needed to register all kepler interfaces within this namespace with Relativity.
	/// This module correlates to the V1 service interfaces.
	/// </summary>
	[RoutePrefix("Milyli.ScriptRunner/V1")]
	[ServiceModule("Milyli_ScriptRunner_V1_Kepler_Service_Module")]
	public interface IScriptRunnerApiModule
	{
	}
}