using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Extratistico.Areas.Cadastros.Models.Entidades
{
    public class FrequenciaVM
    {
        public int CodigoTipo { get; set; }
        public string Frequencia { get; set; }
        public string TipoDesc { get; set; }
        public string CategoriaDesc { get; set; }
        public string Mes { get; set; }
        public string Dia { get; set; }
        public bool DiaUtil { get; set; }
        public double Percentual { get; set; }
    }
}