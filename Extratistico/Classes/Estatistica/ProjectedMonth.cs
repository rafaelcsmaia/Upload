using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Extratistico.Classes.Estatistica
{
    public class ProjectedMonth
    {
        public string MonthName { get; set; }
        public List<ProjectedDate> Dates { get; set; }
    }
}