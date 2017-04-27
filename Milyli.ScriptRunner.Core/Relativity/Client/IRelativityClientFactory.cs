namespace Milyli.ScriptRunner.Core.Relativity.Client
{
    using kCura.Relativity.Client;

    public interface IRelativityClientFactory
    {
        IRSAPIClient GetRelativityClient();
    }
}
