using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Extratistico.Areas.Extratos.Models.Repositorios;
using Extratistico.Classes.DataContext;
using Extratistico.Models.Entidades;
using Extratistico.Classes.Estatistica;
using Extratistico.Areas.Cadastros.Models.Repositorios;

namespace Extratistico.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        ExtratoRepository extratoRepository = new ExtratoRepository();
        FrequenciaRepository frequenciaRepository = new FrequenciaRepository();

        [DynamicPermission]
        public ActionResult Index()
        {
            string username = SessionHelper.GetSessionValue().username;
            var e = new InfoExtratoVM();
            e.SaldoTotal = Math.Round(extratoRepository.SaldoTotal(username) + extratoRepository.CreditoCategorizadoTotal(username),2);
            e.DinheiroTotal = Math.Round(extratoRepository.DinheiroTotal(username), 2);
            e.DisponivelTotal = Math.Round(extratoRepository.SaldoTotal(username) + extratoRepository.CartaoCreditoTotal(username) + e.DinheiroTotal,2);
            e.CartaoTotal = Math.Round(extratoRepository.CartaoCreditoTotal(username) - extratoRepository.CreditoCategorizadoTotal(username),2);
            return View(e);
        }

        [HttpPost]
        public ActionResult ReportDespesasCategorias(int ano, int mes)
        {
            string username = SessionHelper.GetSessionValue().username;
            DateTime dtIni, dtFim;
            if (mes != 0)
            {
                dtIni = new DateTime(ano, mes, 1);
                dtFim = new DateTime(ano, mes, DateTime.DaysInMonth(ano, mes));
            }
            else
            {
                dtIni = new DateTime(ano, 1, 1);
                dtFim = new DateTime(ano, 12, DateTime.DaysInMonth(ano, 12));
            }
            
            return Json(extratoRepository.GastosCategorias(dtIni, dtFim, username), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ReportRecebimentosCategorias(int ano, int mes)
        {
            string username = SessionHelper.GetSessionValue().username;
            DateTime dtIni, dtFim;
            if (mes != 0)
            {
                dtIni = new DateTime(ano, mes, 1);
                dtFim = new DateTime(ano, mes, DateTime.DaysInMonth(ano, mes));
            }
            else
            {
                dtIni = new DateTime(ano, 1, 1);
                dtFim = new DateTime(ano, 12, DateTime.DaysInMonth(ano, 12));
            }

            return Json(extratoRepository.RecebimentosCategorias(dtIni, dtFim, username), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DespesasDetalhes(string categoria, int ano, int mes)
        {
            string username = SessionHelper.GetSessionValue().username;
            DateTime dtIni, dtFim;
            if (mes != 0)
            {
                dtIni = new DateTime(ano, mes, 1);
                dtFim = new DateTime(ano, mes, DateTime.DaysInMonth(ano, mes));
            }
            else
            {
                dtIni = new DateTime(ano, 1, 1);
                dtFim = new DateTime(ano, 12, DateTime.DaysInMonth(ano, 12));
            }
            return Json(extratoRepository.GastosDetalhesCategorias(dtIni, dtFim, categoria, username), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult RecebimentosDetalhes(string categoria, int ano, int mes)
        {
            string username = SessionHelper.GetSessionValue().username;
            DateTime dtIni, dtFim;
            if (mes != 0)
            {
                dtIni = new DateTime(ano, mes, 1);
                dtFim = new DateTime(ano, mes, DateTime.DaysInMonth(ano, mes));
            }
            else
            {
                dtIni = new DateTime(ano, 1, 1);
                dtFim = new DateTime(ano, 12, DateTime.DaysInMonth(ano, 12));
            }
            return Json(extratoRepository.RecebimentosDetalhesCategorias (dtIni, dtFim, categoria, username), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ReportResumoAnual(int ano)
        {
            string username = SessionHelper.GetSessionValue().username;
            return Json(extratoRepository.ReportResumoAnual(ano, username), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ReportProjecao(int dias)
        {
            string username = SessionHelper.GetSessionValue().username;

            return Json(extratoRepository.ReportProjecao(username,dias, frequenciaRepository.Select(username)), JsonRequestBehavior.AllowGet);
        }
    }
}
