using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Extratistico.Models.Repositorios;
using Extratistico.Models.Entidades;
using Extratistico.Classes.DataContext;
using Extratistico.Classes.Helpers;

namespace Extratistico.Areas.Configurações.Controllers
{
    [HandleError]
    public class SGBDController : Controller
    {
        SGBDRepository sgbdRepository = new SGBDRepository();
        TabelaRepository tabelaRepository;
        UsuarioRepository usuarioRepository;
        FuncionalidadesRepository funcionalidadeRepository;
        //
        // GET: /Configurações/Database/
        [HandleError]
        [DynamicPermission(Name = "Gerenciar Base de Dados", ControllerDescription = "Gerenciar Base de Dados")]
        public ActionResult Index()
        {
            SGBD sgbd = sgbdRepository.SelectSingle();
            if (sgbd != null)
            {
                return View(sgbd);
            }
            else
            {
                return RedirectToAction("Configurar");
            }
            
        }

        [HandleError]
        [DynamicPermission(ControllerDescription = "Gerenciar Base de Dados")]
        public ActionResult Configurar()
        {
            return View("Configurar");
        }

        [HandleError]
        [DynamicPermission(Name = "Configurar", ControllerDescription = "Gerenciar Base de Dados")]
        public ActionResult Cadastrar(String tipo)//criar o objeto aqui dentro, nao na pagina!)
        {
            return View("Cadastrar", new SGBD { Tipo = tipo, ConnectionString = ""});
        }

        [HttpPost]
        [HandleError]
        [DynamicPermission(Name = "Configurar", ControllerDescription = "Gerenciar Base de Dados")]
        public ActionResult Cadastrar(SGBD sgbd)
        {
            if (sgbd.ConnectionString == null)
            {
                ModelState.AddModelError("ConnectionString", "Connection String é um campo obrigatório.");
                return View(sgbd);
            }
            else
            {
                //Inserindo SGBD na Base de Dados
                sgbdRepository.Insert(sgbd);
                
                return RedirectToAction("CriarTabelas",sgbd); //Arrumar para Voltar ao Login
            }
        }

        [HandleError]
        public ActionResult CriarTabelas(SGBD sgbd)
        {
            tabelaRepository = new TabelaRepository();
            sgbd.Tabelas = tabelaRepository.Create(HttpContext.Server.MapPath("~/App_Data/Database/"), sgbd.Tipo);
            usuarioRepository = new UsuarioRepository();
            try
            {
                usuarioRepository.Add(new Usuario
                {
                    Nome = "Administrador do Sistema",
                    Email = "admin@Extratistico.com",
                    Password = HashHelper.ComputeHash("admin"),
                    Status = true,
                    Username = "Admin"
                });
                usuarioRepository.Commit();
                sgbd.Admin = true;
            }
            catch (Exception)
            {
                sgbd.Admin = false;
            }
            funcionalidadeRepository = new FuncionalidadesRepository();
            funcionalidadeRepository.Update();

            if (sgbd.Admin)
            {
                SessionHelper.ClearSessionValue();                
                SessionHelper.SetSessionValue(new DynamicMenu()
                {
                    username="Admin",
                    funcionalidades= FuncionalidadesRepository.SelectAll()
                });
            }           

            return View("Index", sgbd); //Arrumar para Voltar ao Login
        }

        [HandleError]
        public ActionResult TestarConexao(string connString, string provider)
        {
            switch (provider)
            {
                case "MySQL":
                    return Json(DataContext.TestConnection(connString, provider), JsonRequestBehavior.AllowGet);
                    break;
                case "Oracle":
                    return Json(DataContext.TestConnection(connString, provider), JsonRequestBehavior.AllowGet);
                    break;
                case "SQLServer":
                    return Json(DataContext.TestConnection(connString, provider), JsonRequestBehavior.AllowGet);
                    break;
                default:
                    throw new Exception("SGBD não implementado!");
            }
            
        } 
    }
}
