using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Extratistico.Classes.Attributes;

namespace Extratistico.Models.Entidades
{
    public class Autenticacao
    {
        public string Usuario { get; set; }

        public int Perfil { get; set; }
    }
}
