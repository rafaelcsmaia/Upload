using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Extratistico.Areas.Cadastros.Models.Entidades;
using Extratistico.Classes.DataContext;

namespace Extratistico.Areas.Cadastros.Models.Repositorios
{
    public class FrequenciaRepository : IRepository
    {
        public List<FrequenciaVM> Select(string username)
        {
            DataTable table = dataContext.Select(String.Format("select *from frequencia f inner join tipo_operacao c on c.codigo=f.codigo_tipo " + 
                      "where c.username='{0}'", username));
            List<FrequenciaVM> frequencias = new List<FrequenciaVM>();
            foreach (DataRow row in table.Rows)
            {
                frequencias.Add(new FrequenciaVM()
                {
                    CodigoTipo = Convert.ToInt32(row["codigo_tipo"]),
                    Mes = row["mes"].ToString(),
                    Dia = row["dia_mes"].ToString(),
                    DiaUtil = true,
                    Percentual = Convert.ToDouble(row["percentual"]),
                    TipoDesc = row["descricao"].ToString(),
                    CategoriaDesc  = row["categoria"].ToString(),
                    Frequencia = row["frequencia"].ToString()
                });
            }
            return frequencias;
        }
    }
}