﻿
using System.Web.Optimization;

namespace PM
{
    public class BundleConfig
    {
    
        public static void RegisterBundles(BundleCollection bundles)
        {

            //third party JS
            bundles.Add(new ScriptBundle("~/bundles/lib").Include(
                     "~/Scripts/jquery-{version}.js",
                     "~/Scripts/bootstrap.js",
                     "~/scripts/bootbox.js",
                     "~/Scripts/respond.js",
                     "~/scripts/datatables/jquery.datatables.js",
                     "~/scripts/datatables/datatables.bootstrap.js",
                     "~/scripts/typeahead.bundle.js",
                     "~/scripts/toastr.js"
                   ));


   

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

          
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            //third party CSS
            bundles.Add(new StyleBundle("~/Content/css").Include(
                 "~/Content/bootstrap-lumen.css",
                 "~/Content/bootstrap-theme.css",
                 "~/content/datatables/css/datatables.bootstrap.css",
                 "~/content/typeahead.css",
                 "~/content/toastr.css",
                 "~/Content/site.css"));


    
        }
    }
}