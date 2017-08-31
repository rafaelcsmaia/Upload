using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Extratistico.Classes.Binders;
using System.Web.Optimization;
using System.Web.Http;

namespace Extratistico
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{arg0}", // URL with parameters
                new { controller = "Home", action = "Index", arg0 = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                "Default2", // Route name
                "{controller}/{action}/{arg0}/{arg1}", // URL with parameters
                new { controller = "Home", action = "Index", arg0 = UrlParameter.Optional, arg1 = UrlParameter.Optional }, // Parameter defaults
                null,
                new[] { "Extratistico.Controllers" }
            );

            routes.MapRoute(
                "Default3", // Route name
                "{controller}/{action}/{arg0}/{arg1}/{arg2}", // URL with parameters
                new { controller = "Home", action = "Index", arg0 = UrlParameter.Optional, arg1 = UrlParameter.Optional, arg2 = UrlParameter.Optional }, // Parameter defaults
                null,
                new[] { "Extratistico.Controllers" }
            );

        }

        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-1.*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/jquerymobile").Include("~/Scripts/jquery.mobile*"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/mobilecss").Include("~/Content/jquery.mobile*", "~/Content/MobileSite.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
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
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
           // ModelBinders.Binders[typeof(DateTime)] =
           //new DateTimeBinder();
            RegisterGlobalFilters(GlobalFilters.Filters);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RegisterRoutes(RouteTable.Routes);
            RegisterBundles(BundleTable.Bundles);
            
        }
    }
}