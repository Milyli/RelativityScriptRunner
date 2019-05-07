namespace Milyli.ScriptRunner.EventHandlers.Services
{
    using System;
    using Relativity.API;

    public class PostInstallSqlService : IDisposable
    {
        private const int DEFAULT_CASE_ID = -1;
        private IDBContext dbContext;
        private bool disposedValue = false;

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
            this.dbContext.ExecuteNonQuerySQLStatement(SqlScript.AddDirectSqlJobSchedule);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this.dbContext?.ReleaseConnection();
                    this.dbContext = null;
                }

                this.disposedValue = true;
            }
        }
    }
}
