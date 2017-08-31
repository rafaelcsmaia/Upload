using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Extratistico.Models.Entidades{
   
    public class AutenticacaoViewModel
    {
        [DisplayName("CodigoDoUsuario")]
        public int CodigoDoUsuario { get; set; }

        [DisplayName("Nome")]
        [Required(ErrorMessage = "Nome é um campo obrigatório.")]
        [StringLength(20, ErrorMessage = "O campo 'Nome' não pode ter mais que 20 caracteres.")]
        public string Nome { get; set; }

        [DisplayName("Username")]
        [Required(ErrorMessage = "UserName é um campo obrigatório.")]
        [StringLength(20, ErrorMessage = "O campo 'Username' não pode ter mais que 20 caracteres.")]
        public string UserName { get; set; }

        [DisplayName("Email")]
        [Required(ErrorMessage = "Email é um campo obrigatório.")]
        [StringLength(60, ErrorMessage = "O campo email não pode ter mais do que 60 caracteres.")]
        public string Email { get; set; }

        [DisplayName("Senha")]
        [Required(ErrorMessage = "Senha é um campo obrigatório.")]
        [StringLength(20, ErrorMessage = "O campo 'Senha' não pode ter mais que 20 caracteres.")]
        public string Senha { get; set; }

        [DisplayName("Confirmar Senha")]
        public string ConfirmarSenha { get; set; }

        [DisplayName("Ativar Usuário")]
        public bool Status { get; set; }

        [DisplayName("PerfilArrayBool")]
        public List<bool> PerfilArrayBool { get; set; }

        [DisplayName("Perfis Habilitados")]
        public List<Perfil> PerfilList {get; set;}
    }
}
