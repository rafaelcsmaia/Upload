using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Extratistico.Classes.Helpers;
using Extratistico.Models.Entidades;
using Extratistico.Models.Repositorios;

namespace Extratistico.Controllers
{
    public class RecuperarSenhaController : Controller
    {
        //
        // GET: /RecuperarSenha/

        UsuarioRepository usuarioRepository = new UsuarioRepository();
        SMTPRepository smtpRepository = new SMTPRepository();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Usuario usuario)
        {
            Usuario u = usuarioRepository.SelectSingle(usuario.Username);
            if (u == null)
            {
                ModelState.AddModelError("", "Usuário inexistente!");
                return View(usuario);
            }
            if (u.Email != usuario.Email)
            {
                ModelState.AddModelError("", "O email informado não corresponde ao cadastrado!");
                return View(usuario);
            }
            SMTP smtp = smtpRepository.SelectSingle();
            if (smtp == null)
            {
                ModelState.AddModelError("", "Serviço SMTP indisponível no momento!");
                return View(usuario);
            }

            string password = HashHelper.GeneratePassword(10, true);
            u.Password = HashHelper.ComputeHash(password);
            usuarioRepository.Edit(u);
            usuarioRepository.Commit();
            MailMessage mail = new MailMessage();
            SmtpClient client = new SmtpClient(smtp.Host , 587);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(smtp.Username , smtp.Password);
            mail.To.Add(u.Email);
            //mail.From = new MailAddress("autosap@ericsson.com");
            mail.From = new MailAddress("noreply@Extratistico.com", "Extratistico");
            mail.Subject = "Troca de senha";
            mail.Body = string.Format("Nova senha:" + password);
            try
            {
                client.Send(mail);
            }
            catch (Exception e)
            {                
                throw;
            }
            
            return Content("Sua nova senha foi enviada para seu email.");
        }
    }
}
