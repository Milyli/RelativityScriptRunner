namespace Milyli.ScriptRunner.Services.Interfaces
{
	using Relativity.Kepler.Services;

	[WebService("ScriptRunApi")]
	[ServiceAudience(Audience.Public)]
	[RoutePrefix("API/ScriptRun")]
    public interface IScriptRunManager
    {
    }
}
