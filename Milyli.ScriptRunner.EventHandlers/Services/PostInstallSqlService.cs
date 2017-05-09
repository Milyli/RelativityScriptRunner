namespace Milyli.ScriptRunner.EventHandlers.Services
{
    using Relativity.API;

    public class PostInstallSqlService
    {
        private const int DEFAULT_CASE_ID = -1;
        private IDBContext dbContext;

        public PostInstallSqlService(IDBContext context)
        {
            this.dbContext = context;
        }

        public PostInstallSqlService(IHelper helper)
            : this(helper.GetDBContext(DEFAULT_CASE_ID))
        {
        }

        public void AddTablesAndSchema()
        {
            this.dbContext.ExecuteNonQuerySQLStatement(SqlScript.JobSchedule_Table);
            this.dbContext.ExecuteNonQuerySQLStatement(SqlScript.JobScriptInput_Table);
            this.dbContext.ExecuteNonQuerySQLStatement(SqlScript.JobHistory_Table);
        }
    }
}
