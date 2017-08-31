using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace Extratistico.Models.Entidades
{
    public class Tabela
    {
        [DisplayName("Nome")]
        public string Nome { get; set; }

        [DisplayName("Criada?")]
        public string Criada { get; set; }
    }
}