namespace Milyli.ScriptRunner.EventHandlers.PostInstall
{
    using Services;

    [kCura.EventHandler.CustomAttributes.Description("Milyli ScriptRunner Post Install EventHandler to set up the database schema")]
    [System.Runtime.InteropServices.Guid("67c92cea-c099-4770-9b8c-4feb34910595")]
    public class ScriptRunnerSetupSchema : kCura.EventHandler.PostInstallEventHandler
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Convention AFAICT")]
        public override kCura.EventHandler.Response Execute()
        {
            kCura.Config.Config.ApplicationName = "Milyli.ScriptRunner::InstancePostInstall";
            kCura.EventHandler.Response retVal = new kCura.EventHandler.Response();
            retVal.Success = true;
            retVal.Message = string.Empty;
            try
            {
                this.InstallTables();
                this.InstallTabs();
            }
            catch (System.Exception ex)
            {
                retVal.Success = false;
                retVal.Message = ex.ToString();
            }

            return retVal;
        }

        private void InstallTables()
        {
            using (var service = new PostInstallSqlService(this.Helper))
            {
                service.AddTablesAndSchema();
            }
        }

        private void InstallTabs()
        {
            var tabsService = new TabManagerService(kCura.Config.Config.ConnectionString);
            tabsService.SetupTabs();
        }
    }
}
