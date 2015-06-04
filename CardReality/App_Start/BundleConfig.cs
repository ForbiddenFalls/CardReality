using System.Web;
using System.Web.Optimization;

namespace CardReality
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-1.10.2.js",
                "~/Scripts/jquery-ui-1.11.4.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/Content/ThemeJs").Include(
                "~/Content/js/jquery-migrate-1.2.1.min.js",
                "~/Content/js/jquery.isotope.min.js",
                "~/Content/js/jquery.appear.js",
                "~/Content/js/jquery.nicescroll.min.js",
                "~/Content/js/jquery.parallax.js",
                "~/Content/js/jquery.textillate.js",
                "~/Content/js/owl.carousel.min.js",
                "~/Content/js/count-to.js",
                "~/Content/js/nivo-lightbox.min.js",
                "~/Content/js/script.js"));

            bundles.Add(new StyleBundle("~/Content/SiteCss").Include(
                "~/Content/bootstrap.css",
                "~/Content/jquery-ui.min.css",
                "~/Content/css/font-awesome.min.css",
                "~/Content/css/style.css",
                "~/Content/css/responsive.css",
                "~/Content/css/animate.css",
                "~/Content/css/green.css"
                ));

            bundles.Add(new StyleBundle("~/bundles/jquery-ui").Include(
                "~/Content/themes/base/jquery.ui.core.css",
                "~/Content/themes/base/jquery.ui.resizable.css",
                "~/Content/themes/base/jquery.ui.selectable.css",
                "~/Content/themes/base/jquery.ui.accordion.css",
                "~/Content/themes/base/jquery.ui.autocomplete.css",
                "~/Content/themes/base/jquery.ui.button.css",
                "~/Content/themes/base/jquery.ui.dialog.css",
                "~/Content/themes/base/jquery.ui.slider.css",
                "~/Content/themes/base/jquery.ui.tabs.css",
                "~/Content/themes/base/jquery.ui.datepicker.css",
                "~/Content/themes/base/jquery.ui.progressbar.css",
                "~/Content/themes/base/jquery.ui.theme.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                "~/Content/themes/base/jquery.ui.core.css",
                "~/Content/themes/base/jquery.ui.autocomplete.css",
                "~/Content/themes/base/jquery.ui.theme.css"));

        }
    }
}
