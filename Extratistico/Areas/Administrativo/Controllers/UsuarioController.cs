using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Extratistico.Models.Repositorios;

namespace Extratistico.Areas.Administrativo.Controllers
{
    [HandleError]
    public class UsuarioController : Controller
    {
        //
        // GET: /Administrativo/Usuario/

        UsuarioRepository usuarioRepository = new UsuarioRepository();
        PerfilRepository perfilRepository = new PerfilRepository();

        [DynamicPermission(Name = "Gerenciar Usuários", ControllerDescription  = "Gerenciar Usuários")]
        public ActionResult Index()
        {
            var usuarios = usuarioRepository.Select(SessionHelper.GetSessionValue().username).OrderBy(u => u.Username);
            return View(usuarios);
        }

        [DynamicPermission(ControllerDescription = "Gerenciar Usuários")]
        public ActionResult Excluir(string arg0)
        {
            usuarioRepository.Delete(arg0);
            usuarioRepository.Commit();
            return RedirectToAction("Index");
            //string script = "location.href='/Administrativo/Usuario'";
            //return JavaScript(script);
        }
    }
}
