using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Extratistico.Models.EntidadesWebAPI
{
    public class ProjecaoSemana
    {
        public string Semana { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fim { get; set; }
        public List<ProjecaoCategoria> Categorias { get; set; }
    }
}