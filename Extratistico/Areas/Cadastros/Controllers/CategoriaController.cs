using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Extratistico.Areas.Cadastros.Models.Entidades;
using Extratistico.Areas.Cadastros.Models.Repositorios;

namespace Extratistico.Areas.Cadastros.Controllers
{
    public class CategoriaController : Controller
    {
        //
        // GET: /Cadastros/Categoria/

        CategoriaRepository categoriaRepository = new CategoriaRepository();

        [HandleError]
        [DynamicPermission(Name="Categorias",ControllerDescription = "Categorias")]
        public ActionResult Index()
        {
            string username = SessionHelper.GetSessionValue().username;
            var c = categoriaRepository.Select(username);
            return View(c);
        }

        [HandleError]
        [DynamicPermission(ControllerDescription = "Categorias")]
        public ActionResult Editar(int codigo)
        {
            string username = SessionHelper.GetSessionValue().username;
            return View(categoriaRepository.SelectSingle(codigo,username));
        }

        [HttpPost]
        [HandleError]
        [DynamicPermission(ControllerDescription = "Categorias")]
        public ActionResult Editar(CategoriaVM categoriaVM)
        {
            string username = SessionHelper.GetSessionValue().username;
            if (ModelState.IsValid)
            {
                categoriaRepository.Editar(categoriaVM, username);
                categoriaRepository.Commit();
                return RedirectToAction("Index", "Categoria");
            }
            else
            {
                return View(categoriaVM);
            }
        }

        [HandleError]
        [DynamicPermission(ControllerDescription = "Categorias")]
        public ActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        [HandleError]
        [DynamicPermission(ControllerDescription = "Categorias")]
        public ActionResult Cadastrar(CategoriaVM categoriaVM)
        {
            string username = SessionHelper.GetSessionValue().username;
            if (ModelState.IsValid)
            {
                categoriaRepository.Cadastrar(categoriaVM, username);
                categoriaRepository.Commit();
                return RedirectToAction("Index", "Categoria");
            }
            else
            {
                return View(categoriaVM);
            }
        }

        [HandleError]
        [DynamicPermission(ControllerDescription = "Categorias")]
        public ActionResult Excluir(int codigo)
        {
            string username = SessionHelper.GetSessionValue().username;
            categoriaRepository.Excluir(codigo, username);
            categoriaRepository.Commit();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult TiposCategoria(string categoria)
        {
            string username = SessionHelper.GetSessionValue().username;
            var t = categoriaRepository.GetTipos(categoria, username);
            List<object> o = new List<object>();
            foreach (var item in t)
            {
                o.Add(new {Value = item.Key , Text=item.Value});
            }
            return Json(o, JsonRequestBehavior.AllowGet);
        }
    }
}
