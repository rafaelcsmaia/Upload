using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Extratistico.Areas.Cadastros.Models.Entidades
{
    public class CategoriaVM
    {
        public int Codigo { get; set; }

        [DisplayName("Tipo")]
        [Required(ErrorMessage = "Tipo é um campo obrigatório.")]
        [StringLength(100, ErrorMessage = "O campo Tipo não pode ter mais do que 100 caracteres")]
        public string Descricao { get; set; }

        [DisplayName("Categoria")]
        [Required(ErrorMessage = "Categoria é um campo obrigatório.")]
        [StringLength(100, ErrorMessage = "O campo Categoria não pode ter mais do que 100 caracteres")]
        public string Categoria { get; set; }

        [DisplayName("Desconsiderar")]
        public bool Desconsiderar { get; set; }
    }
}