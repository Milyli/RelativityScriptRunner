namespace Milyli.ScriptRunner.Data.Repositories
{
    using Milyli.ScriptRunner.Data.Relativity.Client;

    public class RelativityScriptRepository
    {
        private IRelativityClientFactory relativityClientFactory;

        public RelativityScriptRepository(IRelativityClientFactory relativityClientFactory)
        {
            this.relativityClientFactory = relativityClientFactory;
        }
    }
}
