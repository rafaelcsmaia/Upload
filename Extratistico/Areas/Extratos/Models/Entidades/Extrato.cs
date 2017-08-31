using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Extratistico.Areas.Extratos.Models.Entidades
{
    public class Extrato
    {
        public string Documento { get; set; }
        public DateTime Data { get; set; }
        public string Operacao { get; set; }
        public string Estabelecimento { get; set; }
        public int TipoOperacao { get; set; }
        public string TipoEstabelecimento { get; set; }
        public string Categoria { get; set; }
        public double Credito { get; set; }
        public double Debito { get; set; }
        public double DebitoUS { get; set; }
        public double CotacaoDolar { get; set; }
        public bool Sobrescrito { get; set; }
        public string NumeroCartao { get; set; }
        public string Username { get; set; }

        public override string ToString()
        {
            return Data.ToShortDateString() + ";" + Documento + ";" + Operacao + ";" + Estabelecimento + ";" + Credito + ";" + Debito;
        }
    }
}