using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Extratistico.Classes.Estatistica
{
    public class ProjectedDate
    {
        public DateTime Date { get; set; }
        public double Saldo { get; set; }
        public List<ProjectedCategory> Valores { get; set; }
    }

    public struct ProjectedCategory
    {
        public string Categoria { get; set; }
        public string Tipo { get; set; }
        public double Valor { get; set; }
    }
}