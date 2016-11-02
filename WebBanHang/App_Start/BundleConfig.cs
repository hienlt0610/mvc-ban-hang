using System.Web;
using System.Web.Optimization;

namespace WebBanHang
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            // Add Plugin
            bundles.Add(new StyleBundle("~/bundles/plugin-css").Include(
                "~/Content/admin/plugins/color-picker/css/bootstrap-colorpicker.min.css",
                "~/Content/admin/plugins/x-editable/css/bootstrap-editable.css"
            ));
            bundles.Add(new ScriptBundle("~/bundles/plugin-js").Include(
                "~/Content/admin/plugins/color-picker/js/bootstrap-colorpicker.min.js",
                "~/Content/admin/plugins/x-editable/js/bootstrap-editable.min.js"
            ));
        }
    }
}
