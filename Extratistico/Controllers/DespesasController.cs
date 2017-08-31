using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Extratistico.Areas.Cadastros.Models.Repositorios;
using Extratistico.Areas.Extratos.Models.Entidades;
using Extratistico.Areas.Extratos.Models.Repositorios;

namespace Extratistico.Controllers
{
    public class DespesasController : Controller
    {
        //
        // GET: /Despesas/

        ExtratoRepository extratoRepository = new ExtratoRepository();
        CategoriaRepository categoriaRepository = new CategoriaRepository();

        public ActionResult Index()
        {
            return View();
        }

        

    }
}
