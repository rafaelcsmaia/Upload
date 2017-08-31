using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Extratistico.Models.Repositorios;

namespace Extratistico.Areas.Configurações.Controllers
{
    public class FuncionalidadeController : Controller
    {
        //
        // GET: /Configurações/Funcionalidade/
        FuncionalidadesRepository funcionalidadeRepository = new FuncionalidadesRepository();

        [HandleError]
        [DynamicPermission(Name = "Atualizar Funcionalidades", ControllerDescription = "Atualizar Funcionalidades")]
        public ActionResult Index()
        {
            return View(funcionalidadeRepository.SelectNew());
        }

        [HandleError]
        [DynamicPermission(ControllerDescription = "Atualizar Funcionalidades")]
        public ActionResult Atualizar()
        {
            funcionalidadeRepository.Update();
            return View("Index",funcionalidadeRepository.SelectNew());
        }

    }
}
