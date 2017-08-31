using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Extratistico.Classes.Attributes;

namespace Extratistico.Models.Entidades
{
    public class Permissao
    {
        [PrimaryKey(AutoIncrement=false)]
        public int Perfil { get; set; }

        [PrimaryKey(AutoIncrement = false)]
        public int Funcionalidade { get; set; }
    }
}
