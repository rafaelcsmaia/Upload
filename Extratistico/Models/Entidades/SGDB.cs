using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Extratistico.Models.Entidades
{
    public class SGBD
    {
        [DisplayName("Tipo")]
        [Required(ErrorMessage = "Tipo é um campo obrigatório.")]
        [StringLength(50, ErrorMessage = "O campo Tipo não pode ter mais do que 50 caracteres")] //Quando coloquei estava com no maximo 15 caracteres o método cadastrar do controller SGBDController nao executava
        public string Tipo { get; set; }

        [DisplayName("Connection String")]
        [StringLength(255, ErrorMessage = "O campo Connection String não pode ter mais do que 255 caracteres")]
        public string ConnectionString { get; set; }

        public bool Tabelas { get; set; }

        public bool Admin { get; set; }
    }
}