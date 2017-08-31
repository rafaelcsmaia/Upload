using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace System.Web
{
    public class DynamicPermission :  AuthorizeAttribute
    {
        public string ControllerDescription { get; set; }
        public string Name { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            DynamicMenu m = SessionHelper.GetSessionValue();
            if (m == null)
            {
                httpContext.Response.Redirect("~/Login");
                return false;
            }
            else if (m.funcionalidades.Where(x => x.Controller == httpContext.Request.RequestContext.RouteData.Values["controller"].ToString() && x.Action == httpContext.Request.RequestContext.RouteData.Values["action"].ToString()).Count() == 0 && httpContext.Request.RequestContext.RouteData.Values["controller"].ToString() != "Home")
            {
                httpContext.Response.Redirect("~/Error/NotAllowed");
                return false;
            }

            return true;
        }

        //public void OnAuthorization(AuthorizationContext filterContext)
        //{
        //    DynamicMenu m = SessionHelper.GetSessionValue();
        //    if (m == null)
        //    {
        //        filterContext.HttpContext.Response.Redirect("~/Login");
        //    }
        //    else if (m.funcionalidades.Where(x => x.Controller == filterContext.ActionDescriptor.ControllerDescriptor.ControllerName && x.Action == filterContext.ActionDescriptor.ActionName).Count() == 0 && filterContext.ActionDescriptor.ControllerDescriptor.ControllerName != "Home")
        //    {
        //        filterContext.HttpContext.Response.Redirect("~/Error/NotAllowed");
        //    }
        //}
    }
}
