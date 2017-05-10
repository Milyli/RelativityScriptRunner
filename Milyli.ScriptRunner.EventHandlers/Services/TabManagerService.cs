namespace Milyli.ScriptRunner.EventHandlers.Services
{
    using System;
    using RelativityTabManager;
    using RelativityTabManager.Models;

    public class TabManagerService
    {
        private string dbConnectionString;

        public TabManagerService(string dbConnectionString)
        {
            this.dbConnectionString = dbConnectionString;
        }

        public void SetupTabs()
        {
            // Create tab manager instance
            var tabManager = TabManagerFactory.GetTabManager(this.dbConnectionString);

            // Create parent tab
            var tabGroup = new TabGroup
            {
                Name = RelativityApplicationConstants.ApplicationName,
                Guid = new Guid(RelativityApplicationConstants.TabGroupGuid),
                DisplayOrder = 10000,
                ExternalLink = null,
                TabDisplay = TabDisplay.Vertical
            };

            // Create child tabs
            var scheduledJobTab = new Tab
            {
                Name = RelativityApplicationConstants.ScheduleJobsTabName,
                Guid = new Guid(RelativityApplicationConstants.TabGuid),
                DisplayOrder = 10001,
                ExternalLink = $"%ApplicationPath%/custompages/{RelativityApplicationConstants.ApplicationGuid}/",
                TabDisplay = TabDisplay.Vertical
            };
            tabGroup.ChildTabs.Add(scheduledJobTab);

            // Configure tabs
            tabManager.ConfigureTabGroup(tabGroup);
        }
    }
}
