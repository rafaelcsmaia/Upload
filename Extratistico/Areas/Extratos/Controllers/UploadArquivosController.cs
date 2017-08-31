using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Extratistico.Areas.Extratos.Models.Entidades;
using Extratistico.Areas.Extratos.Models.Repositorios;

namespace Extratistico.Areas.Extratos.Controllers
{
    public class UploadArquivosController : Controller
    {
        //
        // GET: /Interface_SAP/UploadArquivos/
        UploadArquivosRepository uploadArquivosRepository = new UploadArquivosRepository();
        ExtratoRepository extratoRepository = new ExtratoRepository();

        [HandleError]
        [DynamicPermission(Name = "Upload de Arquivos", ControllerDescription = "Upload de Arquivos")]
        public ActionResult Index()
        {
            string username = SessionHelper.GetSessionValue().username;
            var vm = new UploadableVM();
            vm.Types = uploadArquivosRepository.UploadableTypes();
            vm.Batches = uploadArquivosRepository.UploadBatches(username);
            return View(vm);
        }

        [HttpPost]
        [DynamicPermission(Name = "Upload de Arquivos", ControllerDescription = "Upload de Arquivos")]
        public ActionResult Index(UploadableVM uploadableVM, HttpPostedFileBase file)
        {
            string username = SessionHelper.GetSessionValue().username;
            if (file == null || file.ContentLength == 0)
            {
                ModelState.AddModelError("file", "É necessário escolher um arquivo para o upload!");
            }
            if (uploadableVM.TipoSelecionado == null || uploadableVM.TipoSelecionado.Length == 0)
            {
                ModelState.AddModelError("TipoSelecionado", "É necessário escolher um tipo de upload!");
            }

            if (ModelState.IsValid)
            {
                IUploadable uploadable = (IUploadable)Activator.CreateInstance(Assembly.GetExecutingAssembly().FullName, uploadableVM.TipoSelecionado[0]).Unwrap();
                uploadable.UploadData(file.InputStream, username);
                //using (StreamReader sr = new StreamReader(file.InputStream))
                //{
                //    string line;
                //    while ((line = sr.ReadLine()) != null)
                //    {
                //        uploadable.UploadData(line);
                //    }
                //}
                return View("Log", uploadable.log);
            }
            else
            {
                uploadableVM.Types = uploadArquivosRepository.UploadableTypes();
                uploadableVM.Batches = uploadArquivosRepository.UploadBatches(username);
                return View(uploadableVM);
            }            
        }

        [HandleError]
        [DynamicPermission(Name = "Detalhes Lote", ControllerDescription = "Upload de Arquivos")]
        public ActionResult DetalhesLote(string tipo, string data)
        {
            string username = SessionHelper.GetSessionValue().username;
            var vm = extratoRepository.ExtratoLoteCarga(tipo, data, username);
            ViewBag.data = data;
            ViewBag.tipo = tipo;
            return View(vm);
        }

        [HandleError]
        [DynamicPermission(Name = "Excluir Lote", ControllerDescription = "Upload de Arquivos")]
        public ActionResult ExcluirLote(string tipo, string data)
        {
            string username = SessionHelper.GetSessionValue().username;
            extratoRepository.ExcluirExtratoLote(username,tipo, data);
            extratoRepository.Commit();
            return RedirectToAction("Index");
        }

        [HandleError]
        [DynamicPermission(Name = "Excluir Registro", ControllerDescription = "Upload de Arquivos")]
        public ActionResult Excluir(string tipo, string documento, string data)
        {
            string username = SessionHelper.GetSessionValue().username;
            extratoRepository.ExcluirExtratoCodigo(username, tipo, documento);
            extratoRepository.Commit();
            return RedirectToAction("DetalhesLote", new { @tipo=tipo, @data=data });
        }

        public ActionResult Log(UploadLog uploadLog)
        {
            return View(uploadLog);
        }

    }
}
