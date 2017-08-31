using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Extratistico.Areas.Cadastros.Models.Entidades;
using Extratistico.Areas.Cadastros.Models.Repositorios;

namespace Extratistico.Areas.Cadastros.Controllers
{
    public class EstabelecimentoController : Controller
    {
        //
        // GET: /Cadastros/Estabelecimento/

        EstabelecimentoRepository estabelecimentoRepository = new EstabelecimentoRepository();
        CategoriaRepository categoriaRepository = new CategoriaRepository();

        [HandleError]
        [DynamicPermission(Name = "Estabelecimento", ControllerDescription = "Estabelecimento")]
        public ActionResult Index()
        {
            string username = SessionHelper.GetSessionValue().username;
            var e = estabelecimentoRepository.Select(username);
            return View(e);
        }

        [HandleError]
        [DynamicPermission(ControllerDescription = "Estabelecimento")]
        public ActionResult Editar(string nome)
        {
            string username = SessionHelper.GetSessionValue().username;
            return View(estabelecimentoRepository.SelectSingle(nome, username));
        }

        [HttpPost]
        [HandleError]
        [DynamicPermission(ControllerDescription = "Estabelecimento")]
        public ActionResult Editar(EstabelecimentoVM estabelecimentoVM)
        {
            string username = SessionHelper.GetSessionValue().username;
            if (ModelState.IsValid)
            {
                estabelecimentoRepository.Editar(estabelecimentoVM, username);
                estabelecimentoRepository.Commit();
                return RedirectToAction("Index", "Estabelecimento");
            }
            else
            {
                return View(estabelecimentoVM);
            }
        }

        [HandleError]
        [DynamicPermission(ControllerDescription = "Estabelecimento")]
        public ActionResult Cadastrar(string nome)
        {
            string username = SessionHelper.GetSessionValue().username;
            List<SelectListItem> categorias = new List<SelectListItem>();
            var c = categoriaRepository.GetCategorias(username);
                foreach (var item in c)
	            {
		            categorias.Add(new SelectListItem { Text = item, Value= item });
	            }

            ViewBag.Categorias = categorias;
            ViewBag.Nome = nome;
            return View();
        }

        [HttpPost]
        [HandleError]
        [DynamicPermission(ControllerDescription = "Estabelecimento")]
        public ActionResult Cadastrar(EstabelecimentoVM estabelecimentoVM, FormCollection form)
        {
            string username = SessionHelper.GetSessionValue().username;
            var tipo = form["Tipos"];
            if (tipo == null)
            {
                ModelState.AddModelError("Tipos", "É necessário definir uma categoria e tipo para o estabelecimento!");
            }
            if (ModelState.IsValid)
            {
                estabelecimentoVM.Tipo = int.Parse(tipo);
                estabelecimentoRepository.Cadastrar(estabelecimentoVM, username);
                estabelecimentoRepository.Commit();
                return RedirectToAction("Index", "Estabelecimento");
            }
            else
            {
                List<SelectListItem> categorias = new List<SelectListItem>();
                var c = categoriaRepository.GetCategorias(username);
                foreach (var item in c)
                {
                    categorias.Add(new SelectListItem { Text = item, Value = item });
                }

                ViewBag.Categorias = categorias;
                return View(estabelecimentoVM);
            }
        }

        [HandleError]
        [DynamicPermission(ControllerDescription = "Estabelecimento")]
        public ActionResult Excluir(string nome)
        {
            string username = SessionHelper.GetSessionValue().username;
            estabelecimentoRepository.Excluir(nome, username);
            estabelecimentoRepository.Commit();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult NomesCategoria(string categoria)
        {
            string username = SessionHelper.GetSessionValue().username;
            var t = estabelecimentoRepository.GetNomes(categoria, username);
            List<object> o = new List<object>();
            foreach (var item in t)
            {
                o.Add(new { Value = item.Value, Text = item.Key });
            }
            return Json(o, JsonRequestBehavior.AllowGet);
        }

    }
}
