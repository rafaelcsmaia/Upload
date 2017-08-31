using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Extratistico.Models.Entidades;
using Extratistico.Models.Repositorios;
using Extratistico.Classes.Helpers;

namespace Extratistico.Controllers
{
    [HandleError]
    public class LoginController : Controller
    {
        //
        // GET: /Login/
        private PerfilRepository dbPerfil = new PerfilRepository();
        private UsuarioRepository dbUsuario = new UsuarioRepository();
        private SGBDRepository sgbdRepository = new SGBDRepository();
        private FuncionalidadesRepository dbFunc = new FuncionalidadesRepository();

        [HandleError]
        public ActionResult Index()
        {
            return View();
            if (sgbdRepository.SelectSingle() != null)
            {
                return View();
            }
            else
            {
                SessionHelper.ClearSessionValue();
                SessionHelper.SetSessionValue(new DynamicMenu()
                {
                    username = "TempUser",
                    funcionalidades = FuncionalidadesRepository.SelectSGBD()
                });
                return RedirectToAction("Index", "SGBD", new { area = "Configurações" });
            }            
        }

        [HttpPost]
        [HandleError]
        public ActionResult Index(Login login)
        {
            Usuario u = dbUsuario.SelectSingle(login.Username);
            if (u == null || !u.Password.Equals(HashHelper.ComputeHash(login.Password)))
            {
                ModelState.AddModelError("", "Login Inválido");
                return View(login);
            }

            if (!u.Status)
            {
                ModelState.AddModelError("", "Usuário Desativado, contate o administrador do sistema");
                return View(login);
            }

            DynamicMenu menu = new DynamicMenu();
            menu.username = login.Username;
            menu.perfis = dbPerfil.Select(login.Username).Select(p => p.Codigo).ToArray();
            if (menu.username.ToUpper() == "ADMIN")
            {
                menu.funcionalidades = FuncionalidadesRepository.SelectAll();
            }
            else
            {
                menu.funcionalidades = dbFunc.Select(menu.perfis);
            }
            
            //var funcionalidades = dbFunc.Select(menu.perfis);
            //foreach (var funcionalidade in funcionalidades)
            //{
            //    Funcionalidade f = new Funcionalidade();
            //    f.Action = funcionalidade.Action;
            //    f.Area = funcionalidade.Area;
            //    f.Controller = funcionalidade.Controller;
            //    f.Descricao = funcionalidade.Descricao;
            //    menu.funcionalidades.Add(f);
            //}
            SessionHelper.SetSessionValue(menu, 30);
            return RedirectToAction("Index", "Home");
        }


        public ActionResult LogOut()
        {
            SessionHelper.ClearSessionValue();
            return RedirectToAction("Index");
        }

    }
}
    
