using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Extratistico.Models.Entidades
{
    public class SMTP
    {
        [DisplayName("Host")]
        [Required(ErrorMessage = "Host é um campo obrigatório.")]
        [StringLength(60, ErrorMessage = "O campo Host não pode ter mais do que 60 caracteres.")]
        public string Host { get; set; }

        [DisplayName("Porta")]
        [Required(ErrorMessage = "Porta é um campo obrigatório.")]
        public int Porta { get; set; }

        [DisplayName("Login")]
        [Required(ErrorMessage = "Login é um campo obrigatório.")]
        [StringLength(100, ErrorMessage = "O campo Login não pode ter mais do que 100 caracteres.")]
        public string Username { get; set; }

        [DisplayName("Senha")]
        [Required(ErrorMessage = "Senha é um campo obrigatório.")]
        [StringLength(25, ErrorMessage = "O campo Senha não pode ter mais do que 25 caracteres.")]
        public string Password { get; set; }

        [DisplayName("SSL Habilitado")]
        public bool SSLEnabled { get; set; }
    }
}