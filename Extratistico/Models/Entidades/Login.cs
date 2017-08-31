using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Extratistico.Classes.Attributes;
using System.ComponentModel;

namespace Extratistico.Models.Entidades
{
    public class Login
    {
        [DisplayName("Login")]
        public string Username { get; set; }

        [DisplayName("Senha")]
        public string Password { get; set; }
    }
}
