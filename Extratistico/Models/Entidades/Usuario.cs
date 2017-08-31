using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Extratistico.Classes.Attributes;

namespace Extratistico.Models.Entidades
{
    public class Usuario
    {
        [DisplayName("Login")]
        [Required(ErrorMessage = "Login é um campo obrigatório.")]
        [StringLength(10, ErrorMessage = "O campo login não pode ter mais do que 10 caracteres.")]
        public string Username { get; set; }

        [DisplayName("Nome")]
        [Required(ErrorMessage = "Nome é um campo obrigatório.")]
        [StringLength(50, ErrorMessage = "O campo Nome não pode ter mais do que 50 caracteres.")]
        public string Nome { get; set; }

        [DisplayName("Senha")]
        [Required(ErrorMessage = "Senha é um campo obrigatório.")]
        [StringLength(10, ErrorMessage = "O campo senha não pode ter mais do que 10 caracteres.")]
        public string Password { get; set; }

        [DisplayName("Está Ativo?")]
        public bool Status { get; set; }

        [DisplayName("Email")]
        [Required(ErrorMessage = "Email é um campo obrigatório.")]
        [StringLength(60, ErrorMessage = "O campo email não pode ter mais do que 60 caracteres.")]
        public string Email { get; set; }
    }
}
