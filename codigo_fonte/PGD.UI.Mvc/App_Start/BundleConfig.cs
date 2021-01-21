using System.Collections.Generic;
using System.Web;
using System.Web.Optimization;

namespace PGD.UI.Mvc
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"
                        ));

 
 
            var valBundle = new ScriptBundle("~/bundles/jqueryval").Include(
                    "~/Scripts/cldr.js",
                    "~/Scripts/cldr/event.js",
                    "~/Scripts/cldr/supplemental.js",
                    "~/Scripts/jquery.validate.js",
                    "~/Scripts/jquery.validate.unobtrusive.js",
                    "~/Scripts/jspdf.debug.js",
                    "~/Scripts/globalize/globalize-1.1.0/globalize.js",
                    "~/Scripts/globalize/globalize-1.1.0/globalize/message.js",
                    "~/Scripts/globalize/globalize-1.1.0/globalize/number.js",
                    "~/Scripts/globalize/globalize-1.1.0/globalize/plural.js",
                    "~/Scripts/globalize/globalize-1.1.0/globalize/date.js",
                    "~/Scripts/globalize/globalize-1.1.0/globalize/currency.js",
                    "~/Scripts/globalize/globalize-1.1.0/globalize/relative-time.js",
                    "~/Scripts/jspdf.plugin.autotable.js",
                    "~/Scripts/FileSaver.js",
                    "~/Scripts/tableExport.js",
                    "~/Scripts/jquery.wordexport.js",
                    "~/Scripts/jquery.table2excel.js",
                    "~/Scripts/html2canvas.js",
                    "~/Scripts/jquery.validate.globalize.js", 
                    "~/Scripts/selectize.js",
                    "~/Scripts/select2/select2.min.js",
                    "~/Scripts/jquery.unobtrusive-ajax.min.js",
                    "~/Scripts/jquery-ui.js",
                    "~/Scripts/jquery.maskedinput.js",
                    "~/Scripts/jquery.blockUI.js",
                    "~/Scripts/jquery-datepicker.js"
                    );

            valBundle.Orderer = new AsIsBundleOrderer();

            bundles.Add(valBundle);

             
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Content/Bootstrap/js/bootstrap.js",
                      "~/Content/Bootstrap/js/bootstrap.js",
                      "~/Scripts/bootstrap-table/bootstrap-table.min.js",
                      "~/Scripts/bootstrap-table/Locale/bootstrap-table-pt-BR.js",
                      "~/Scripts/bootstrap-table/extensions/filter-control/bootstrap-table-filter-control.js",
                      "~/Scripts/bootstrap-table/extensions/editable/bootstrap-table-editable.js",
                      "~/Scripts/bootstrap-table/extensions/mobile/bootstrap-table-mobile.js",
                      "~/Scripts/bootstrap-table/extensions/export/tableExport.min.js",
                      "~/Scripts/bootstrap-table/extensions/export/bootstrap-table-export.min.js",
                      "~/Scripts/bootstrap-table/extensions/js-xlsx/xlsx.core.min.js",
                      "~/Scripts/respond.js"));


            bundles.Add(new ScriptBundle("~/bundles/CGUUtil").Include(
                "~/Scripts/cgu.util.js",
                "~/Scripts/Functions.js",
                "~/Scripts/Site/Enums.js",
                "~/Scripts/Site/Constants.js"));


            bundles.Add(new StyleBundle("~/bundles/css").Include(
                      "~/Content/bootstrap/css/bootstrap.css",
                      "~/Content/bootstrap-table/bootstrap-table.min.css",
                      "~/Content/bootstrap/css/bootstrap-theme.min.css",
                      "~/Content/site.css",
                      "~/Content/css/Site.css",
                      "~/Content/css/datatables.min.css",
                      "~/Content/css/datepicker2.css",
                      "~/Content/select2/select2.min.css",
                      "~/Content/fontawesome5.14.0/css/all.min.css",
                      "~/Content/css/selectize.bootstrap3.css"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/inputmask").Include(
                      "~/Scripts/jquery.inputmask/inputmask.js",
                      "~/Scripts/jquery.inputmask/jquery.inputmask.js",
                      "~/Scripts/jquery.inputmask/inputmask.extensions.js",
                      "~/Scripts/jquery.inputmask/inputmask.date.extensions.js",
                      "~/Scripts/jquery.inputmask/inputmask.numeric.extensions.js"));

        }
    }

    public class AsIsBundleOrderer : IBundleOrderer
    {
        public IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> files)
        {
            return files;
        }
    }
}
