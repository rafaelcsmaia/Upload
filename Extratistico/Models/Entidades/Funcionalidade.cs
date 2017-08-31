using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Extratistico.Classes.Attributes;

namespace Extratistico.Models.Entidades
{
    public class Funcionalidade
    {
        public int Codigo { get; set; }

        public string Controller{ get; set; }

        public string ControllerDescription { get; set; }

        public string Action { get; set; }

        public string Area { get; set; }

        public string Descricao { get; set; }

    }
}
