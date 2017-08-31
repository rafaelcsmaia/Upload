using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Extratistico.Controllers
{
    public class ErrorController : Controller
    {
        //
        // GET: /Error/

        public ActionResult NotAllowed()
        {
            if (HttpContext.Request.IsAjaxRequest())
            {
                return JavaScript("alert('Você não possui permissões suficientes para realizar esta função, entre em contato com o administrador do sistema.');");
            }
            else
            {
                return View();
            }
        }
    }
}
