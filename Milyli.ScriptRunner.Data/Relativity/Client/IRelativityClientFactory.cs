namespace Milyli.ScriptRunner.Data.Relativity.Client
{
    using kCura.Relativity.Client;

    public interface IRelativityClientFactory
    {
        IRSAPIClient GetRelativityClient();
    }
}
