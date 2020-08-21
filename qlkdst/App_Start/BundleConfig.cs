using System.Web;
using System.Web.Optimization;

namespace qlkdst
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                       "~/Scripts/jquery.validate*"
                       , "~/Scripts/additional-methods*"
                       , "~/Scripts/jquery.validate.unobtrusive.js"
                       , "~/Scripts/jquery.maskedinput.min.js"
                       , "~/Scripts/bootbox.min.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/bootstrap.css",
            //          "~/Content/site.css"));

            //sbadmin
            bundles.Add(new ScriptBundle("~/bundles/sbadminjs").Include("~/Scripts/jquery-1.12.4.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/vendor").Include(
             "~/SBAdmin/vendor/metisMenu/metisMenu.min.js",
             "~/SBAdmin/vendor/raphael/raphael.min.js",
             //"~/SBAdmin/vendor/morrisjs/morris.min.js",
             //"~/SBAdmin/data/morris-data.js",
             "~/SBAdmin/dist/js/sb-admin-2.js"));
            /*"~/Scripts/jquery-ui-1.10.2.js")*/


            bundles.Add(new StyleBundle("~/Content/css").Include(
                      //"~/Content/bootstrap.css",
                      "~/SBAdmin/vendor/bootstrap/css/bootstrap.min.css",
                      "~/SBAdmin/vendor/metisMenu/metisMenu.min.css",
                      "~/SBAdmin/dist/css/sb-admin-2.css",
                      //"~/SBAdmin/vendor/morrisjs/morris.css",
                      "~/SBAdmin/vendor/font-awesome/css/font-awesome.min.css",
                      "~/Content/site.css"
                     ));

            //scripts
            bundles.Add(new ScriptBundle("~/bundles/otherjs").Include("~/templatesite/js/gnmenu.js",
                "~/Scripts/jquery-ui-1.12.1.min.js",
                "~/Scripts/jquery.table.marge.js",
                "~/templatesite/js/jquery.basictable.min.js",
                "~/Scripts/select2.min.js"
                ));

            //style
            bundles.Add(new StyleBundle("~/bundles/othercss").Include(
                "~/templatesite/css/basictable.css",
                "~/Content/themes/base/jquery-ui.min.css",
                "~/templatesite/css/component.css",
                "~/templatesite/css/style_grid.css",
                "~/Content/css/select2.min.css",
                 "~/Content/site.css"
                ));




            //END SBADMIN
            //bundles.Add(new ScriptBundle("~/bundles/vendor").Include(
            //  "~/SBAdmin/vendor/metisMenu/metisMenu.min.js",
            //  "~/SBAdmin/vendor/raphael/raphael.min.js",            
            //  "~/SBAdmin/dist/js/sb-admin-2.js",                   
            //     "~/templatesite/js/amcharts.js",
            //     "~/templatesite/js/serial.js",
            //     "~/templatesite/js/export.js",
            //      "~/templatesite/js/light.js",
            //       "~/templatesite/js/chart1.js",
            //       "~/templatesite/js/Chart.min.js",
            //       "~/templatesite/js/modernizr.custom.js",
            //       "~/templatesite/js/classie.js",
            //       "~/templatesite/js/gnmenu.js",
            //       "~/templatesite/js/circles.js",
            //       "~/templatesite/js/skycons.js",
            //       "~/templatesite/js/screenfull.js",
            //       "~/templatesite/js/flipclock.js",
            //       "~/templatesite/js/bars.js",
            //        "~/templatesite/js/jquery.nicescroll.js",
            //        "~/templatesite/js/scripts.js"
            //        ,"~/templatesite/js/jquery.basictable.min.js",
            //        "~/Scripts/jquery-ui-1.12.1.min.js",
            //        "~/Scripts/jquery.validate.min.js",
            //        "~/Scripts/additional-methods.min.js",
            //        "~/Scripts/jquery.validate.unobtrusive.js",
            //        "~/Scripts/jquery.maskedinput.min.js",
            //       "~/templatesite/js/bootstrap-3.1.1.min.js", "~/Scripts/jquery.table.marge.js"));

            //bundles.Add(new StyleBundle("~/Content/css").Include(

            //        "~/SBAdmin/vendor/bootstrap/css/bootstrap.min.css",
            //        "~/SBAdmin/vendor/metisMenu/metisMenu.min.css",
            //        "~/SBAdmin/dist/css/sb-admin-2.css",
            //        "~/templatesite/css/basictable.css",
            //        "~/templatesite/css/component.css",
            //        "~/templatesite/css/export.css",
            //        "~/templatesite/css/flipclock.css",
            //        "~/templatesite/css/circles.css",
            //        "~/templatesite/css/style_grid.css",
            //        "~/templatesite/css/style.css",
            //         "~/SBAdmin/vendor/font-awesome/css/font-awesome.min.css",
            //        "~/Content/themes/base/jquery-ui.min.css",
            //        "~/Content/site.css"
            //       ));

            //datatables plugin
            bundles.Add(new StyleBundle("~/Content/DataTables").Include(
                "~/Content/DataTables/css/dataTables.bootstrap4.min.css"

                ));
            bundles.Add(new ScriptBundle("~/Script/DataTables").Include(
                "~/Scripts/DataTables/jquery.dataTables.min.js",
                "~/Scripts/DataTables/dataTables.bootstrap4.min.js"));
            //end sbadmin

            //trstyle
            bundles.Add(new StyleBundle("~/Content/trstylemainpage").Include(
                "~/templatesite/css/bootstrap.css",
                "~/templatesite/css/table-style.css",
                "~/templatesite/css/basictable.css",
                "~/templatesite/css/component.css",
                "~/templatesite/css/export.css",
                "~/templatesite/css/flipclock.css",
                "~/templatesite/css/circles.css",
                "~/templatesite/css/style_grid.css",
                "~/templatesite/css/style.css",
                "~/templatesite/css/font-awesome.css", "~/Content/themes/base/jquery-ui.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/trjsmain").Include(
                 //"~/templatesite/js/jquery-2.1.4.min.js",
                 "~/Scripts/jquery-1.12.4.min.js",
                 "~/templatesite/js/amcharts.js",
                 "~/templatesite/js/serial.js",
                 "~/templatesite/js/export.js",
                  "~/templatesite/js/light.js",
                   "~/templatesite/js/chart1.js",
                   "~/templatesite/js/Chart.min.js",
                   "~/templatesite/js/modernizr.custom.js",
                   "~/templatesite/js/classie.js",
                   "~/templatesite/js/gnmenu.js",
                   "~/templatesite/js/circles.js",
                   "~/templatesite/js/skycons.js",
                   "~/templatesite/js/screenfull.js",
                   "~/templatesite/js/flipclock.js",
                   "~/templatesite/js/bars.js",
                    "~/templatesite/js/jquery.nicescroll.js",
                    "~/templatesite/js/scripts.js", "~/templatesite/js/jquery.basictable.min.js",
                    "~/Scripts/jquery-ui-1.12.1.min.js",
                    "~/Scripts/jquery.validate.min.js",
                    "~/Scripts/additional-methods.min.js",
                    "~/Scripts/jquery.validate.unobtrusive.js",
                    "~/Scripts/jquery.maskedinput.min.js",
                   "~/templatesite/js/bootstrap-3.1.1.min.js", "~/Scripts/jquery.table.marge.js"));

            bundles.Add(new StyleBundle("~/Content/trstyle").Include(
                  "~/templatesite/css/bootstrap.css",
                  "~/templatesite/css/component.css",
                  "~/templatesite/css/style_grid.css",
                  "~/templatesite/css/style.css",
                  "~/templatesite/css/font-awesome.css"));

            bundles.Add(new ScriptBundle("~/bundles/trjs").Include(
                 "~/templatesite/js/jquery-2.1.4.min.js",
                 "~/templatesite/js/modernizr.custom.js",
                 "~/templatesite/js/classie.js",
                 "~/templatesite/js/gnmenu.js",
                  "~/templatesite/js/prettymaps.js",
                   "~/templatesite/js/screenfull.js",
                   "~/templatesite/js/jquery.nicescroll.js",
                   "~/templatesite/js/scripts.js",
                   "~/templatesite/js/bootstrap-3.1.1.min.js"
                   ,"~/Scripts/jquery.table.marge.js"));


            //https://timepicker.co
            bundles.Add(new StyleBundle("~/Content/TimepickerPlugin").Include("~/Content/timepickerplugin/css/jquery.timepicker.min.css"));
            bundles.Add(new ScriptBundle("~/Script/TimepickerPlugin").Include("~/Scripts/timepickerplugin/jquery.timepicker.min.js"));


            bundles.Add(new ScriptBundle("~/bundles/inputmask").Include(
            //~/Scripts/inputmask/dependencyLibs/inputmask.dependencyLib.js",  //if not using jquery
            "~/Scripts/inputmask/inputmask.js",
            "~/Scripts/inputmask/jquery.inputmask.js",
            "~/Scripts/inputmask/inputmask.extensions.js",
            "~/Scripts/inputmask/inputmask.date.extensions.js",
            //and other extensions you want to include
            "~/Scripts/inputmask/inputmask.numeric.extensions.js"));

        }
    }
}
