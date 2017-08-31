using System.Web.Mvc;

namespace Extratistico.Areas.Configurações
{
    public class ConfiguraçõesAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Configurações";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Configurações_default",
                "Configurações/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
