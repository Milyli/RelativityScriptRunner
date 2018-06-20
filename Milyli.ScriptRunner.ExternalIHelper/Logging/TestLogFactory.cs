namespace Milyli.ScriptRunner.ExternalIHelper.Logging
{
	using API = Relativity.API;

	public class TestLogFactory : API.ILogFactory
	{
		public API.IAPILog GetLogger()
		{
			return new TraceLogger();
		}
	}
}
