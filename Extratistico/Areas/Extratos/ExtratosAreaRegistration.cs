using System.Web.Mvc;

namespace Extratistico.Areas.Extratos
{
    public class ExtratosAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Extratos";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Extratos_default",
                "Extratos/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
