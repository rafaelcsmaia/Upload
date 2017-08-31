using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Extratistico.Classes.Helpers;
using Extratistico.Models.Entidades;
using Extratistico.Models.Repositorios;

namespace Extratistico.Areas.Administrativo.Controllers
{
    [HandleError]
    public class ContaController : Controller
    {
        UsuarioRepository usuarioRepository = new UsuarioRepository();
        PermissaoRepository permissaoRepository = new PermissaoRepository();
        PerfilRepository perfilRepository = new PerfilRepository();

        public ActionResult Detalhes(string username)
        {
            if (SessionHelper.GetSessionValue().username == username)
            {
                AutenticacaoViewModel autenticacao = new AutenticacaoViewModel();
                autenticacao.PerfilList = perfilRepository.Select(username).ToList();
                autenticacao.PerfilArrayBool = new List<bool>();
                Usuario usuario = usuarioRepository.SelectSingle(username);
                autenticacao.UserName = usuario.Username;
                autenticacao.Email = usuario.Email;
                autenticacao.Senha = usuario.Password;
                autenticacao.Nome = usuario.Nome;
                foreach (var item in autenticacao.PerfilList)
                {
                    autenticacao.PerfilArrayBool.Add(true);
                }
                return View(autenticacao);
            }
            else
            {
                return RedirectToAction("NotAllowed", "Error");
            }
        }

        public ActionResult Editar(string username)
        {
            if (SessionHelper.GetSessionValue().username == username)
            {
                var usuario = usuarioRepository.SelectSingle(username);
                ContaViewModel contaVM = new ContaViewModel()
                {
                    UserName = usuario.Username,
                    Email = usuario.Email
                };
                return View(contaVM);
            }
            else
            {
                return RedirectToAction("NotAllowed", "Error");
            }
        }

        [HttpPost]
        public ActionResult Editar(ContaViewModel contaVM)
        {
            if (SessionHelper.GetSessionValue().username == contaVM.UserName)
            {
                if (contaVM.Senha != contaVM.ConfirmarSenha && contaVM.Senha != string.Empty)
                    ModelState.AddModelError("Senha", "O campo senha e confirmação de senha estão divergentes.");
                
                if (ModelState.IsValid)
                {
                    Usuario u = usuarioRepository.SelectSingle(contaVM.UserName);
                    u.Email = contaVM.Email;
                    if (contaVM.Senha != string.Empty)
                    {
                        u.Password = HashHelper.ComputeHash(contaVM.Senha);
                    }
                    usuarioRepository.Edit(u);
                    usuarioRepository.Commit();
                    return RedirectToAction("Detalhes", new { username = contaVM.UserName });
                }
                else
                {
                    return View(contaVM);
                }
            }
            else
            {
                return RedirectToAction("NotAllowed", "Error");
            }
        }

    }
}
