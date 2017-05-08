namespace Milyli.ScriptRunner.Core.Relativity.Client
{
    using global::Relativity.API;
    using kCura.Relativity.Client;

    public interface IRelativityClientFactory
    {
        IRSAPIClient GetRelativityClient(ExecutionIdentity executionIdentity);

        IRSAPIClient GetRelativityClient();
    }
}
