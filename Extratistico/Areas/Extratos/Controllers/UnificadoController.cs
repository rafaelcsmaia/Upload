using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Extratistico.Areas.Cadastros.Models.Repositorios;
using Extratistico.Areas.Extratos.Models.Entidades;
using Extratistico.Areas.Extratos.Models.Repositorios;

namespace Extratistico.Areas.Extratos.Controllers
{
    public class UnificadoController : Controller
    {
        //
        // GET: /Extratos/Unificado/

        ExtratoRepository extratoRepository = new ExtratoRepository();
        CategoriaRepository categoriaRepository = new CategoriaRepository();

        [HandleError]
        [DynamicPermission(Name = "Extrato Unificado", ControllerDescription = "Unificado")]
        public ActionResult Index()
        {
            string username = SessionHelper.GetSessionValue().username;
            var e = extratoRepository.ExtratoUnificado(username);
            return View(e);
        }

        [HandleError]
        [DynamicPermission(Name = "Excluir Registro", ControllerDescription = "Unificado")]
        public ActionResult Excluir(string tipo, string documento,string categoria, int? ano, int? mes)//Recebe ano e mes para redirecionar para categoria se precisar
        {
            string username = SessionHelper.GetSessionValue().username;
            extratoRepository.ExcluirExtratoCodigo(username, tipo, documento);
            extratoRepository.Commit();
            if (ano == null || mes == null || categoria==string.Empty)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Categoria", new {@categoria=categoria, @ano=ano, @mes = mes });
            }
            
        }

        public ActionResult Categoria(string categoria, int ano, int mes)
        {
            string username = SessionHelper.GetSessionValue().username;
            DateTime dtIni = new DateTime(ano, mes == 0 ? 1 : mes, 1);
            DateTime dtFim = new DateTime(ano, mes == 0 ? 12 : mes, DateTime.DaysInMonth(ano, mes == 0 ? 12 : mes));
            var e = extratoRepository.ExtratoCategoria(categoria, dtIni, dtFim, username);
            return View(e);
        }

        //public ActionResult Tipo(string categoria, string tipo, int ano, int mes)
        //{
        //    DateTime dtIni = new DateTime(ano, mes, 1);
        //    DateTime dtFim = new DateTime(ano, mes, DateTime.DaysInMonth(ano, mes));
        //    return View();
        //}

        public ActionResult Detalhes(string documento)
        {
            string username = SessionHelper.GetSessionValue().username;
            var e = extratoRepository.SelectSingle(documento, username);
            return View(e);
        }

        public ActionResult Categorizar(string documento)
        {
            string username = SessionHelper.GetSessionValue().username;
            List<SelectListItem> categorias = new List<SelectListItem>();
            var c = categoriaRepository.GetCategorias(username);
            foreach (var item in c)
            {
                categorias.Add(new SelectListItem { Text = item, Value = item });
            }

            ViewBag.Categorias = categorias;
            var e = extratoRepository.SelectSingle(documento, username);
            return View(e);
        }

        [HttpPost]
        public ActionResult Categorizar(CategorizarExtratoVM categorizarExtratoVM, FormCollection form)
        {
            string username = SessionHelper.GetSessionValue().username;
            var tipo = form["Tipos"];
            if (tipo == null)
            {
                ModelState.AddModelError("Tipos", "É necessário definir uma categoria e tipo para a despesa!");
            }
            if (ModelState.IsValid)
            {
                categorizarExtratoVM.Tipo = tipo;
                extratoRepository.Categorizar(categorizarExtratoVM, username);
                extratoRepository.Commit();
                return RedirectToAction("Detalhes", "Unificado",
                    new { area="Extratos", documento = categorizarExtratoVM.Documento });
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
                return View(categorizarExtratoVM);
            }
        }


        public ActionResult Cadastrar()
        {
            string username = SessionHelper.GetSessionValue().username;
            List<SelectListItem> categorias = new List<SelectListItem>();
            var c = categoriaRepository.GetCategorias(username );
            foreach (var item in c)
            {
                categorias.Add(new SelectListItem { Text = item, Value = item });
            }

            ViewBag.Categorias = categorias;
            return View();
        }

        [HttpPost]
        public ActionResult Cadastrar(DinheiroVM dinheiroVM, FormCollection form)
        {
            string username = SessionHelper.GetSessionValue().username;
            var tipo = form["Tipos"];
            if (tipo == null)
            {
                ModelState.AddModelError("Tipos", "É necessário definir uma categoria e tipo para o estabelecimento!");
            }
            if (ModelState.IsValid)
            {
                dinheiroVM.Tipo = int.Parse(tipo);
                extratoRepository.CadastrarGastoDinheiro(dinheiroVM, username);
                extratoRepository.Commit();
                return RedirectToAction("Index", "Unificado");
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
                return View(dinheiroVM);
            }
        }
    }
}
