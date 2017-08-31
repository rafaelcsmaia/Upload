using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Extratistico.Models.Entidades;
using Extratistico.Models.Repositorios;

namespace Extratistico.Areas.Administrativo.Controllers
{
    [HandleError]
    public class PerfilController : Controller
    {
        PerfilRepository perfilRepository = new PerfilRepository();
        PermissaoRepository permissaoRepository = new PermissaoRepository();
        FuncionalidadesRepository funcionalidadesRepository = new FuncionalidadesRepository();

        [DynamicPermission(Name = "Gerenciar Perfis", ControllerDescription = "Gerenciar Perfis")]
        public ActionResult Index()
        {
            var perfil = perfilRepository.Select().OrderBy(p => p.Codigo);
            return View(perfil);
        }

        [HttpPost]
        [DynamicPermission]
        public ActionResult Index(FormCollection form)
        {
            var result = form["enabled_ul"];
            return RedirectToAction("Index");
        }

        //[DynamicPermission]
        //public ActionResult Cadastrar()
        //{
        //    var list = funcionalidadesRepository.Select();
        //    ViewBag["Perfil"] = perfilRepository.Select();
        //    return View(list);
        //}

        //[HttpPost]
        //[DynamicPermission]
        //public ActionResult Cadastrar(Perfil perfil)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        perfilRepository.Add(perfil);

        //        return RedirectToAction("Index");
        //    }
        //    ViewBag["Perfil"] = perfilRepository.Select();
        //    return View();
        //}
    }
}
