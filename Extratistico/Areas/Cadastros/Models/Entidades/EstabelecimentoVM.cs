using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Extratistico.Areas.Cadastros.Models.Entidades
{
    public class EstabelecimentoVM
    {
        [DisplayName("Nome")]
        [Required(ErrorMessage = "O nome é obrigatório!")]
        [StringLength(100, ErrorMessage = "O nome não pode ter mais de 100 caracteres!")]
        public string Nome { get; set; }

        public int Tipo { get; set; }

        [DisplayName("Descrição")]
        [StringLength(120, ErrorMessage = "A descrição não pode ter mais de 120 caracteres!")]
        public string Descricao { get; set; }

        public string DescricaoTipo { get; set; }
        public string Categoria { get; set; }
    }
}