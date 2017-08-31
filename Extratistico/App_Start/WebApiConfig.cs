using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Extratistico
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{arg0}",
                defaults: new { arg0 = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi2",
                routeTemplate: "api/{controller}/{Action}/{arg0}",
                defaults: new { arg0 = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi3",
                routeTemplate: "api/{controller}/{Action}/{arg0}/{arg1}",
                defaults: new { arg0 = RouteParameter.Optional, arg1 = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
    name: "DefaultApi5",
    routeTemplate: "api/{controller}/{Action}/{arg0}/{arg1}/{arg2}",
    defaults: new { arg0 = RouteParameter.Optional, arg1 = RouteParameter.Optional, arg2 = RouteParameter.Optional }
);

            config.Routes.MapHttpRoute(
    name: "DefaultApi4",
    routeTemplate: "api/{controller}/{Action}"
);

            var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);

            // Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable<T> return type.
            // To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.
            // For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.
            //config.EnableQuerySupport();

            // To disable tracing in your application, please comment out or remove the following line of code
            // For more information, refer to: http://www.asp.net/web-api
            //config.EnableSystemDiagnosticsTracing();
        }
    }
}