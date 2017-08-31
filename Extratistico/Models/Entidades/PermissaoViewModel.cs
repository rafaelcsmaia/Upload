using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Extratistico.Classes.Attributes;

namespace Extratistico.Models.Entidades
{
    public class PermissaoViewModel
    {
        [DisplayName("Perfil")]
        public List<Perfil> ListPerfil { get; set; }

        [DisplayName("CodigoDaPermissao")]
        public int CodigoDaPermissao { get; set; }

        [DisplayName("Descrição")]
        [Required(ErrorMessage = "Descrição é um campo obrigatório.")]
        [StringLength(30, ErrorMessage = "O campo descrição não pode ter mais que 30 caracteres.")]
        public string Descricao { get; set; }

        [DisplayName("Funcionalidades Habilitadas")]
        public Dictionary<Funcionalidade, bool> funcionalidade { get; set; }
    }
}
