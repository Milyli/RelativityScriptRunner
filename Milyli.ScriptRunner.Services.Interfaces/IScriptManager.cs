namespace Milyli.ScriptRunner.Services.Interfaces
{
	using Relativity.Kepler.Services;

	[WebService("ScriptApi")]
	[ServiceAudience(Audience.Public)]
	[RoutePrefix("API/Script")]
	public interface IScriptManager
	{
	}
}
