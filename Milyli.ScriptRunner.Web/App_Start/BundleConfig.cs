namespace Milyli.ScriptRunner.Web
{
    using System.Web.Optimization;

    public static class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            /*script bundles*/
            bundles.Add(new ScriptBundle("~/bundles/tools").Include(
                "~/Scripts/Tools/*.js"));

            bundles.Add(new ScriptBundle("~/bundles/libs").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/bootstrap.js",
                "~/Scripts/typeahead.bundle.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/knockout").Include(
                "~/Scripts/knockout-{version}.js",
                "~/Scripts/knockout.viewmodel.{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/JobSchedule").Include(
                "~/Scripts/ViewModel/JobHistoryViewModel.js",
                "~/Scripts/ViewModel/JobScheduleViewModel.js"));

            bundles.Add(new ScriptBundle("~/bundles/datatables/js").Include(
                        "~/Scripts/DataTables/jquery.dataTables.js",
                        "~/Scripts/DataTables/jquery.dataTables.js"));

            /*style bundles*/
            bundles.Add(new StyleBundle("~/bundles/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/font-awesome.css"));

            bundles.Add(new StyleBundle("~/Content/DataTables/css").Include(
                "~/Content/DataTables/css/common.css",
                "~/Content/DataTables/css/jquery.dataTables.css",
                "~/Content/DataTables/css/dataTables.bootstrap.css"));
        }
    }
}