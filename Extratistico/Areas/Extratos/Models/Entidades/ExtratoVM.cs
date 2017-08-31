using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Extratistico.Areas.Extratos.Models.Entidades
{
    public class ExtratoVM
    {
        public string Documento { get; set; }
        public DateTime Data { get; set; }
        public string Dia { get; set; }
        public string Mes { get; set; }
        public string Ano { get; set; }
        public string Operacao { get; set; }
        public string Estabelecimento { get; set; }
        public string TipoOperacao{ get; set; }
        public string Procedencia { get; set; }
        public string Categoria { get; set; }
        public double Valor { get; set; }
        public string Frequencia { get; set; }
    }
}