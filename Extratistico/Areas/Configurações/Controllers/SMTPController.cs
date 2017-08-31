using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Extratistico.Models.Repositorios;
using Extratistico.Models.Entidades;
using System.Net.Mail;
using System.Net;
using Extratistico.Classes.Helpers;

namespace Extratistico.Areas.Configurações.Controllers
{
    [HandleError]
    public class SMTPController : Controller
    {
        //
        // GET: /Configurações/SMTP/

        SMTPRepository smtpRepository = new SMTPRepository();
        UsuarioRepository usuarioRepository = new UsuarioRepository();

        [HandleError]
        [DynamicPermission(Name = "Configurar servidor SMTP", ControllerDescription = "Configurar servidor SMTP")]
        public ActionResult Index()
        {
            SMTP smtp = smtpRepository.SelectSingle();
            if (smtp != null)
            {
                return View(smtp);
            }
            else
            {
                return RedirectToAction("Configurar");
            }
        }

        [HandleError]
        [DynamicPermission(ControllerDescription = "Configurar servidor SMTP")]
        public ActionResult Configurar()
        {
            SMTP smtp = smtpRepository.SelectSingle();
            if (smtp != null)
            {
                return View(smtp);
            }
            else
            {
                smtp = new SMTP();
                return View(smtp);
            }
        }

        [HandleError]
        [HttpPost]
        [DynamicPermission(ControllerDescription = "Configurar servidor SMTP")]
        public ActionResult Configurar(SMTP smtp)
        {
            if (!ModelState.IsValid)
            {
                return View(smtp);
            }
            smtpRepository.Add(smtp);
            smtpRepository.Commit();
            return RedirectToAction("Index");
        }

    }
}
