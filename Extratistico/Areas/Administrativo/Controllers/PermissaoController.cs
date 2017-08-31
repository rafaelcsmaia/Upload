using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Extratistico.Models.Entidades;
using System.Data;
using Extratistico.Models.Repositorios;

namespace Extratistico.Areas.Administrativo.Controllers
{
    [HandleError]
    public class PermissaoController : Controller
    {
        //
        // GET: /Permissao/
        PermissaoRepository permissaoRepository = new PermissaoRepository();
        PerfilRepository perfilRepository = new PerfilRepository();
        FuncionalidadesRepository funcionalidadesRepository = new FuncionalidadesRepository();

        [DynamicPermission(ControllerDescription="Gerenciar Perfis")]
        public ActionResult Cadastrar()
        {
            PermissaoViewModel permissaoViewModel = new PermissaoViewModel();
            permissaoViewModel.funcionalidade = funcionalidadesRepository.Select();
            return View(permissaoViewModel);
        }

        [HttpPost]
        [DynamicPermission(ControllerDescription = "Gerenciar Perfis")]
        public ActionResult Cadastrar(PermissaoViewModel permissaoVM, Dictionary<int,bool> funcionalidades)
        {
            if (funcionalidades.Values.Where(x => x).Count() == 0)
            {
                ModelState.AddModelError("funcionalidade", "Ao menos uma funcionalidade deve ser habilitada.");
            }
            if (ModelState.IsValid)
            {
                Perfil perfilToAdd = new Perfil();

                perfilToAdd.Descricao = permissaoVM.Descricao;

                perfilRepository.Add(perfilToAdd);
                perfilRepository.Commit();
                perfilToAdd=perfilRepository.SelectSingle(perfilToAdd.Descricao);
                foreach (var item in funcionalidades.Where(x=>x.Value))
                {
                    Permissao p = new Permissao();
                    p.Perfil = perfilToAdd.Codigo;
                    p.Funcionalidade = item.Key;
                    permissaoRepository.Add(p);
                }
                permissaoRepository.Commit();
                return RedirectToAction("Index","Perfil");
            }
            else
            {
                permissaoVM.funcionalidade = new Dictionary<Funcionalidade, bool>();
                foreach (var f in funcionalidadesRepository.Select())
                {
                    permissaoVM.funcionalidade.Add(f.Key,funcionalidades.SingleOrDefault(x => x.Key == f.Key.Codigo).Value);
                }
                return View(permissaoVM);
            }
        }

        [DynamicPermission(ControllerDescription = "Gerenciar Perfis")]
        public ActionResult Editar(int arg0)
        {
            PermissaoViewModel permissaoViewModel = new PermissaoViewModel();
            permissaoViewModel.funcionalidade = new Dictionary<Funcionalidade, bool>();
            List<Permissao> listPermissao = permissaoRepository.Select(arg0);

            foreach (var f in funcionalidadesRepository.Select())
            {
                permissaoViewModel.funcionalidade.Add(f.Key, listPermissao.Select(x => x.Funcionalidade).Contains(f.Key.Codigo));
            } 

            Perfil perfil = perfilRepository.SelectSingle(arg0);
            permissaoViewModel.Descricao = perfil.Descricao;
            permissaoViewModel.CodigoDaPermissao = perfil.Codigo;
            return View(permissaoViewModel);
        }

        [HttpPost]
        [DynamicPermission(ControllerDescription = "Gerenciar Perfis")]
        public ActionResult Editar(PermissaoViewModel permissaoViewModel, Dictionary<int, bool> funcionalidades)
        {
            if (funcionalidades.Values.Where(x => x).Count() == 0)
            {
                ModelState.AddModelError("funcionalidade", "Ao menos uma funcionalidade deve ser habilitada.");
            }
            if (ModelState.IsValid)
            {
                permissaoRepository.Delete(permissaoViewModel.CodigoDaPermissao);
                permissaoRepository.Commit();
                foreach (var item in funcionalidades.Where(x => x.Value))
                {
                    Permissao p = new Permissao();
                    p.Perfil = permissaoViewModel.CodigoDaPermissao;
                    p.Funcionalidade = item.Key;
                    permissaoRepository.Add(p);
                }
                permissaoRepository.Commit();
                return RedirectToAction("Index", "Perfil");
            }
            else
            {
                //Dictionaries dentro de models nao sao "bindadas" na pagina e por isso vem nulas, devem ser criadas novamente.
                permissaoViewModel.funcionalidade = new Dictionary<Funcionalidade, bool>();
                foreach (var f in funcionalidadesRepository.Select())
                {
                    permissaoViewModel.funcionalidade.Add(f.Key, funcionalidades.SingleOrDefault(x => x.Key == f.Key.Codigo).Value);
                }
                return View(permissaoViewModel);
            }
        }

        [DynamicPermission(ControllerDescription = "Gerenciar Perfis")]
        public ActionResult Excluir(int arg0)
        {
            if (!perfilRepository.HasRelationships(arg0))
            {
                permissaoRepository.Delete(arg0);
                permissaoRepository.Commit();
                perfilRepository.Delete(arg0);                
                perfilRepository.Commit();
                string script = "location.href='/Administrativo/Perfil/Index'";
                return JavaScript(script);
            }
            else
            {
                string script = "alert('O perfil possui usuários relacionados a ele e não pode ser excluído.')";
                return JavaScript(script);
            }
        }
    }
}
