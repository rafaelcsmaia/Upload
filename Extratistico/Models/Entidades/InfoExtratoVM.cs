using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Extratistico.Models.Entidades
{
    public class InfoExtratoVM
    {
        public double DisponivelTotal { get; set; }
        public double SaldoTotal { get; set; }
        public double DinheiroTotal { get; set; }
        public double CartaoTotal { get; set; }
    }
}