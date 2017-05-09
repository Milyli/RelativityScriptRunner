namespace Milyli.ScriptRunner.EventHandlers
{
    using Services;

    [kCura.EventHandler.CustomAttributes.Description("Milyli ScriptRunner Post Install EventHandler to set up the database schema")]
    [System.Runtime.InteropServices.Guid("67c92cea-c099-4770-9b8c-4feb34910595")]
    public class ScriptRunnerSetupSchema : kCura.EventHandler.PostInstallEventHandler
    {
        private void InstallTables()
        {
            var service = new PostInstallSqlService(this.Helper);
            service.AddTablesAndSchema();
        }

#pragma warning disable SA1202 // Elements must be ordered by access
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Convention AFAICT")]
        public override kCura.EventHandler.Response Execute()
#pragma warning restore SA1202 // Elements must be ordered by access
        {
            kCura.Config.Config.ApplicationName = "Milyli.ScriptRunner::InstancePostInstall";
            kCura.EventHandler.Response retVal = new kCura.EventHandler.Response();
            retVal.Success = true;
            retVal.Message = string.Empty;
            try
            {
                this.InstallTables();
            }
            catch (System.Exception ex)
            {
                retVal.Success = false;
                retVal.Message = ex.ToString();
            }

            return retVal;
        }
    }
}
