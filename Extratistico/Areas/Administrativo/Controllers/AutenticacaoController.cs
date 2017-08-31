using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Extratistico.Models.Entidades;
using Extratistico.Models.Repositorios;
using Extratistico.Classes.Helpers;



namespace Extratistico.Areas.Administrativo.Controllers
{
    [HandleError]
    public class AutenticacaoController : Controller
    {
        PerfilRepository perfilRepository = new PerfilRepository();
        UsuarioRepository usuarioRepository = new UsuarioRepository();
        AutenticacaoRepository autenticacaoRepository = new AutenticacaoRepository();
        //
        // GET: /Autenticacao/

        [DynamicPermission(ControllerDescription = "Gerenciar Usuários")]
        public ActionResult Cadastrar()
        {
            AutenticacaoViewModel autenticacao = new AutenticacaoViewModel();
            autenticacao.PerfilList = perfilRepository.Select().ToList();
            autenticacao.PerfilArrayBool = new List<bool>();
            foreach (var item in autenticacao.PerfilList)
            {
                autenticacao.PerfilArrayBool.Add(false);
            }
            return View(autenticacao);
        }

        [HttpPost]
        [DynamicPermission(ControllerDescription = "Gerenciar Usuários")]
        public ActionResult Cadastrar(AutenticacaoViewModel autenticacaoVMToAdd)
        {
            if (usuarioRepository.Exists(autenticacaoVMToAdd.UserName))
            {
                ModelState.AddModelError("UserName", "O usuário informado já existe.");
            }

            if (autenticacaoVMToAdd.Senha != autenticacaoVMToAdd.ConfirmarSenha)
            {
                ModelState.AddModelError("ConfirmarSenha", "Senha e confirmação de senha divergentes.");
            }
            if (autenticacaoVMToAdd.PerfilArrayBool == null)
            {
                ModelState.AddModelError("PerfilList", "É necessário criar pelo menos um perfil antes de cadastrar um usuário!");
            }else if (autenticacaoVMToAdd.PerfilArrayBool.Where(x=>x).Count() == 0)
            {
                ModelState.AddModelError("PerfilList", "Ao menos um perfil deve ser habilitado.");
            }
            if (autenticacaoVMToAdd.UserName == "Admin")
            {
                ModelState.AddModelError("UserName", "O nome de usuário 'Admin' é reservado para uso no sistema.");
            }

            if (ModelState.IsValid)
            {
                AutenticacaoViewModel autenticacaoVM = autenticacaoVMToAdd;
                autenticacaoVM.PerfilList = perfilRepository.Select();

                //Instancia um novo usuario
                Usuario userToAdd = new Usuario();

                //Atribui os dados da view ao usuario
                userToAdd.Nome = autenticacaoVM.Nome;
                userToAdd.Username = autenticacaoVM.UserName;
                userToAdd.Email = autenticacaoVM.Email;
                userToAdd.Password = HashHelper.ComputeHash(autenticacaoVM.Senha);
                userToAdd.Status = autenticacaoVM.Status;

                //Chama o metodo Add() para adicionar o usuario ao banco
                usuarioRepository.Add(userToAdd);
                usuarioRepository.Commit();

                for (int i = 0; i < autenticacaoVM.PerfilList.Count; i++)
                {
                    if (autenticacaoVM.PerfilArrayBool[i])
                    {
                        //Cria as novas autenticações para o usuário
                        Autenticacao autenticacaoToAdd = new Autenticacao();

                        autenticacaoToAdd.Perfil = autenticacaoVM.PerfilList[i].Codigo;
                        autenticacaoToAdd.Usuario = autenticacaoVM.UserName;

                        //Chama o metodo Add() para inserir as autenticações no banco
                        autenticacaoRepository.Add(autenticacaoToAdd);
                    }
                }
                autenticacaoRepository.Commit();
                return RedirectToAction("Index", "Usuario");
            }
            else
            {
                autenticacaoVMToAdd.PerfilList = perfilRepository.Select();
                return View(autenticacaoVMToAdd);
            }
        }

        [DynamicPermission(ControllerDescription = "Gerenciar Usuários")]
        public ActionResult Editar(string arg0)
        {
            AutenticacaoViewModel autenticacao = new AutenticacaoViewModel();

            List<Autenticacao> listAut = autenticacaoRepository.Select((arg0));

            autenticacao.PerfilList = perfilRepository.Select();

            autenticacao.PerfilArrayBool = new List<bool>(autenticacao.PerfilList.Count);

            int contArrayBool = 0;

            foreach (var item in autenticacao.PerfilList)
            {
                for (int i = 0; i < listAut.Count; i++)
                {
                    if (item.Codigo == listAut[i].Perfil)
                    {
                        autenticacao.PerfilArrayBool.Add(true);
                        break;
                    }
                    else
                    {
                        if (i == listAut.Count - 1)
                        {
                            autenticacao.PerfilArrayBool.Add(false);
                        }
                    }
                }
                contArrayBool++;
            }

            Usuario user = usuarioRepository.SelectSingle(arg0);
            autenticacao.Nome = user.Nome;
            autenticacao.Email = user.Email;
            autenticacao.Senha = user.Password;
            autenticacao.Status = user.Status;
            autenticacao.UserName = user.Username;

            if (autenticacao.PerfilArrayBool.Count == 0)
            {
                for (int i = 0; i < autenticacao.PerfilList.Count; i++)
                {
                    autenticacao.PerfilArrayBool.Add(false);
                }
            }
            return View(autenticacao);
        }

        [HttpPost]
        [DynamicPermission(ControllerDescription = "Gerenciar Usuários")]
        public ActionResult Editar(AutenticacaoViewModel autenticacaoVMToEdit, string arg0)
        {
            //Adiciona uma validação manual, pois esta não tem como ser feita através de annotations
            if (autenticacaoVMToEdit.PerfilArrayBool.Where(x => x).Count() == 0)
            {
                ModelState.AddModelError("PerfilList", "Ao menos um perfil deve ser habilitado.");
            }
            //Valida se o Model está válido, de acordo com os annotations
            if (ModelState.IsValid)
            {
                AutenticacaoViewModel autenticacaoVM = autenticacaoVMToEdit;
                autenticacaoVM.PerfilList = perfilRepository.Select();

                foreach (var item in autenticacaoRepository.Select((arg0)))
                {
                    //Chama as deleções por usuário e perfil
                    autenticacaoRepository.Delete(item.Usuario,item.Perfil);
                }
                //Dá o commit na base de dados
                autenticacaoRepository.Commit();

                //Instancia um novo Usuario
                Usuario userToEdit = new Usuario();

                //Atribui os valores ao usuario
                userToEdit.Nome = autenticacaoVM.Nome;
                userToEdit.Email = autenticacaoVM.Email;
                userToEdit.Password = HashHelper.ComputeHash(autenticacaoVM.Senha);
                userToEdit.Status = autenticacaoVM.Status;
                userToEdit.Username = autenticacaoVMToEdit.UserName;

                //Chama o método Edit() para editar o usuário
                usuarioRepository.Edit(userToEdit);
                usuarioRepository.Commit();
                for (int i = 0; i < autenticacaoVM.PerfilList.Count; i++)
                {
                    if (autenticacaoVM.PerfilArrayBool[i])
                    {
                        //Cria as autenticações novamente para o usuário
                        Autenticacao autenticacaoToEdit = new Autenticacao();

                        autenticacaoToEdit.Perfil = autenticacaoVM.PerfilList[i].Codigo;
                        autenticacaoToEdit.Usuario = autenticacaoVM.UserName;

                        //Insere a autenticação na base de dados
                        autenticacaoRepository.Add(autenticacaoToEdit);
                    }
                }
                autenticacaoRepository.Commit();
                return RedirectToAction("Index", "Usuario");
            }
            else
            {
                autenticacaoVMToEdit.PerfilList = perfilRepository.Select();
                return View(autenticacaoVMToEdit);
            }
        }

        //[DynamicPermission(ControllerDescription = "Usuario")]
        //public ActionResult Excluir(string arg0)
        //{
        //    usuarioRepository.Delete(arg0);
        //    usuarioRepository.Commit();
        //    string script = "location.href='/Administrativo/Usuario'";
        //    return JavaScript(script);
        //}
    }
}
