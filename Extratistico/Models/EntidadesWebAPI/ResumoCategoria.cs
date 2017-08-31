using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Extratistico.Models.EntidadesWebAPI
{
    public class ResumoCategoria
    {
        public String Categoria { get; set; }
        public double Valor { get; set; }
        public List<ResumoTipo> Detalhes { get; set; }        
    }
}