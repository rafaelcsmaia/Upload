using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Extratistico.Areas.Extratos.Models.Entidades
{
    public class DinheiroVM
    {
        [Required(ErrorMessage="A data do gasto é obrigatória!")]
        public DateTime Data { get; set; }

        [DisplayName("Operação")]
        [Required(ErrorMessage = "A operação é obrigatória")]
        [StringLength(100, ErrorMessage="A operação não pode ter mais de 100 caracteres!")]
        public string Operacao { get; set; }

        [StringLength(100, ErrorMessage = "O Estabelecimento não pode ter mais de 100 caracteres!")]
        public string Estabelecimento { get; set; }

        public int Tipo { get; set; }

        [Required(ErrorMessage = "O valor gasto é obrigatório!")]
        public double Valor { get; set; }

        public int Ano { get; set; }

        public int Mes { get; set; }

        public int Dia { get; set; }

        public void CriarDataManual()
        {
            Data = new DateTime(Ano, Mes, Dia);
        }
    }
}