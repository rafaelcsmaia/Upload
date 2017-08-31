using System;
using System.Collections.Generic;
using System.Linq;

namespace System.Web
{
    public class SessionHelper
    {
        public static DynamicMenu GetSessionValue()
        {
            return HttpContext.Current.Session["dynamicMenu"] as DynamicMenu;
        }

        public static void SetSessionValue(DynamicMenu menu)
        {
            HttpContext.Current.Session.Add("dynamicMenu", menu);
        }

        public static void SetSessionValue(DynamicMenu menu, int timeoutInMinutes)
        {
            HttpContext.Current.Session.Timeout = timeoutInMinutes;
            HttpContext.Current.Session.Add("dynamicMenu", menu);
        }

        public static void ClearSessionValue()
        {
            HttpContext.Current.Session.Remove("dynamicMenu");
        }
    }
}
