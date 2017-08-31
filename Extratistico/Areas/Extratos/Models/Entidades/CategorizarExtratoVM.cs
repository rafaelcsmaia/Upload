using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Extratistico.Areas.Extratos.Models.Entidades
{
    public class CategorizarExtratoVM
    {
        [DisplayName("Número do Documento")]
        public string Documento { get; set; }
        public DateTime Data { get; set; }

        public string Operacao { get; set; }
        public string Estabelecimento { get; set; }
        public string Tipo { get; set; }
        public double Valor { get; set; }
        public string Procedencia { get; set; }
        //public string DescricaoOperacao { get; set; }
        //public string DescricaoEstabelecimento { get; set; } 
        
        public string TipoExtrato { get; set; }
        public string TipoOperacao { get; set; }
        //public string TipoEstabelecimento { get; set; }

        public string Categoria { get; set; }
        //public string CategoriaOperacao { get; set; }
        //public string CategoriaEstabelecimento { get; set; }        
    }
}