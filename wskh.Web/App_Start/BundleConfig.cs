using System.Web;
using System.Web.Optimization;

namespace wskh.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new ScriptBundle("~/bundles/Theme2").Include(
         "~/wwwroot/Dashboard_V2/assets/plugins/popper/popper.js",
         "~/wwwroot/Dashboard_V2/assets/plugins/jquery-blockui/jquery.blockui.min.js",
         "~/wwwroot/Dashboard_V2/assets/plugins/jquery-slimscroll/jquery.slimscroll.js",
         "~/wwwroot/Dashboard_V2/assets/plugins/bootstrap/js/bootstrap.min.js",
         "~/wwwroot/Dashboard_V2/assets/plugins/bootstrap-switch/js/bootstrap-switch.min.js",
         "~/wwwroot/Dashboard_V2/assets/plugins/sparkline/jquery.sparkline.js",
         "~/wwwroot/Dashboard_V2/assets/js/pages/sparkline/sparkline-data.js",
         "~/wwwroot/Dashboard_V2/assets/js/app.js",
         "~/wwwroot/Dashboard_V2/assets/js/layout.js",
         "~/wwwroot/Dashboard_V2/assets/js/theme-color.js",
         "~/wwwroot/Dashboard_V2/assets/plugins/material/material.min.js",
         "~/wwwroot/Dashboard_V2/assets/plugins/apexcharts/apexcharts.min.js",
         "~/wwwroot/Dashboard_V2/assets/js/pages/chart/chartjs/home-data.js",
         "~/wwwroot/Dashboard_V2/assets/plugins/summernote/summernote.js",
         "~/wwwroot/Dashboard_V2/assets/js/pages/summernote/summernote-data.js",
         "~/wwwroot/componenets/toastr/toastr.min.js",
         "~/wwwroot/componenets/bootbox/bootbox.min.js",
         "~/wwwroot/componenets/global/global.js"
         ));







            bundles.Add(new StyleBundle("~/Content/Theme2").Include(
                     "~/wwwroot/Dashboard_V2/fonts/simple-line-icons/simple-line-icons.min.css",
                     "~/wwwroot/Dashboard_V2/fonts/font-awesome/css/font-awesome.min.css",
                     "~/wwwroot/Dashboard_V2/fonts/material-design-icons/material-icon.css",
                     "~/wwwroot/Dashboard_V2/assets/plugins/bootstrap/css/bootstrap.min.css",
                     "~/wwwroot/Dashboard_V2/assets/plugins/summernote/summernote.css",
                     "~/wwwroot/Dashboard_V2/assets/plugins/material/material.min.css",
                     "~/wwwroot/Dashboard_V2/assets/plugins/material/material.rtl.min.css",
                     "~/wwwroot/Dashboard_V2/assets/css/material_style.css",
                     "~/wwwroot/Dashboard_V2/assets/css/pages/inbox.min.css",
                     "~/wwwroot/Dashboard_V2/assets/css/theme/rtl/theme_style.css",
                     "~/wwwroot/Dashboard_V2/assets/css/plugins.min.css",
                     "~/wwwroot/Dashboard_V2/assets/css/theme/rtl/style.css",
                     "~/wwwroot/Dashboard_V2/assets/css/responsive.css",
                     "~/wwwroot/Dashboard_V2/assets/css/theme/rtl/theme-color.css",
                     "~/wwwroot/Dashboard_V2/assets/css/theme/rtl/rtl.css",
                     "~/wwwroot/custome/css/custome.css",
                     "~/wwwroot/componenets/toastr/toastr.css",
                     "~/wwwroot/componenets/global/global.css",
                     "~/wwwroot/componenets/select2/select2.min.css",
                     "~/wwwroot/dashboard/FarsiFont/FarsiFonts.css"
                     ));





            bundles.Add(new ScriptBundle("~/bundles/Default").Include(
          "~/wwwroot/dashboard/plugins/jquery/jquery.min.js",
          "~/wwwroot/dashboard/plugins/popper/popper.js",
          "~/wwwroot/dashboard/plugins/jquery-blockui/jquery.blockui.min.js",
          "~/wwwroot/dashboard/plugins/jquery-slimscroll/jquery.slimscroll.js",
          "~/wwwroot/custome/js/custome.js",
          "~/wwwroot/componenets/bootbox/bootbox.min.js",
          "~/wwwroot/componenets/global/global.js"
          ));

            bundles.Add(new ScriptBundle("~/bundles/Bootstrap").Include(
         "~/wwwroot/dashboard/plugins/bootstrap/js/bootstrap.min.js",
         "~/wwwroot/dashboard/plugins/bootstrap-switch/js/bootstrap-switch.min.js",
         "~/wwwroot/dashboard/plugins/sparkline/jquery.sparkline.js",
         "~/wwwroot/dashboard/js/pages/sparkline/sparkline-data.js",
         "~/wwwroot/componenets/toastr/toastr.min.js"
         ));

            bundles.Add(new ScriptBundle("~/bundles/Common").Include(
         "~/wwwroot/dashboard/js/app.js",
         "~/wwwroot/dashboard/js/layout.js",
         "~/wwwroot/dashboard/js/theme-color.js",
         "~/wwwroot/componenets/select2/select2.min.js"
         ));

            bundles.Add(new ScriptBundle("~/bundles/Material").Include(
         "~/wwwroot/dashboard/plugins/material/material.min.js"
         ));

            bundles.Add(new ScriptBundle("~/bundles/Chart").Include(
         "~/wwwroot/dashboard/plugins/chart-js/Chart.bundle.js",
         "~/wwwroot/dashboard/plugins/chart-js/utils.js",
         "~/wwwroot/dashboard/js/pages/chart/chartjs/home-data.js"
         ));
            bundles.Add(new ScriptBundle("~/bundles/Summernote").Include(
       "~/wwwroot/dashboard/plugins/summernote/summernote.js",
   "~/wwwroot/dashboard/js/pages/summernote/summernote-data.js"
       ));



            bundles.Add(new StyleBundle("~/Content/FontsAndIcons").Include(
                  "~/wwwroot/dashboard/FarsiFont/FarsiFonts.css",
                  "~/wwwroot/dashboard/fonts/simple-line-icons/simple-line-icons.min.css",
                  "~/wwwroot/dashboard/fonts/font-awesome/css/font-awesome.min.css",
                  "~/wwwroot/dashboard/fonts/material-design-icons/material-icon.css",
                  "~/wwwroot/custome/css/custome.css"
                  ));
            bundles.Add(new StyleBundle("~/Content/Bootstrap").Include(
                  "~/wwwroot/dashboard/plugins/bootstrap/css/bootstrap.min.css",
                  "~/wwwroot/dashboard/plugins/summernote/summernote.css",
                  "~/wwwroot/dashboard/css/pages/extra_pages.css",
                  "~/wwwroot/componenets/toastr/toastr.css"
                  ));
            bundles.Add(new StyleBundle("~/Content/Material").Include(
                 "~/wwwroot/dashboard/plugins/material/material.min.css",
                 "~/wwwroot/dashboard/plugins/material/material.rtl.min.css",
                 "~/wwwroot/dashboard/css/material_style.css",
                 "~/wwwroot/dashboard/fonts/material-design-icons/material-icon.css"
                 ));
            bundles.Add(new StyleBundle("~/Content/Theme").Include(
                 "~/wwwroot/componenets/global/global.css",
                 "~/wwwroot/dashboard/css/pages/inbox.min.css",
                 "~/wwwroot/dashboard/css/theme/rtl/theme_style.css",
                 "~/wwwroot/dashboard/css/plugins.min.css",
                 "~/wwwroot/dashboard/css/theme/rtl/style.css",
                 "~/wwwroot/dashboard/css/responsive.css",
                 "~/wwwroot/dashboard/css/theme/rtl/theme-color.css",
                 "~/wwwroot/dashboard/css/theme/rtl/theme-color.css",
                 "~/wwwroot/componenets/select2/select2.min.css"
                 ));
        }
    }
}
