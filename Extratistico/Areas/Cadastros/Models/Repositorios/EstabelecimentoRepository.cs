using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Extratistico.Areas.Cadastros.Models.Entidades;
using Extratistico.Classes.DataContext;

namespace Extratistico.Areas.Cadastros.Models.Repositorios
{
    public class EstabelecimentoRepository : IRepository
    {
        public List<EstabelecimentoVM> Select(string username)
        {
            DataTable table = dataContext.Select(String.Format("select e.nome, e.tipo, e.descricao, t.descricao as descricaotipo, t.categoria " + 
                      "from estabelecimento as e inner join " + 
                      "tipo_operacao as t on t.codigo = e.tipo where e.username='{0}'", username));
            List<EstabelecimentoVM> extratos = new List<EstabelecimentoVM>();
            foreach (DataRow row in table.Rows)
            {
                extratos.Add(new EstabelecimentoVM()
                {
                    Nome = row["Nome"].ToString(),
                    Tipo = Convert.ToInt32(row["Tipo"]),
                    Descricao = row["Descricao"].ToString(),
                    DescricaoTipo = row["DescricaoTipo"].ToString(),
                    Categoria = row["Categoria"].ToString(),
                });
            }
            return extratos;
        }

        public EstabelecimentoVM SelectSingle(string nome, string username)
        {
            DataRow row = dataContext.SelectSingle(String.Format("select e.nome, e.tipo, e.descricao, t.descricao as descricaotipo, t.categoria " +
                      "from estabelecimento as e inner join " +
                      "tipo_operacao as t on t.codigo = e.tipo where e.nome='{0}' and e.username='{1}'", nome, username));
            if (row != null)
            {
                EstabelecimentoVM e = new EstabelecimentoVM()
                {
                    Nome = row["Nome"].ToString(),
                    Tipo = Convert.ToInt32(row["Tipo"]),
                    Descricao = row["Descricao"].ToString(),
                    DescricaoTipo = row["DescricaoTipo"].ToString(),
                    Categoria = row["Categoria"].ToString(),
                };
                return e;
            }
            else
            {
                return null;
            }
        }

        public int Cadastrar(EstabelecimentoVM  e, string username)
        {
            return dataContext.ExecuteNonQuery(String.Format(
            "insert into estabelecimento(nome, descricao, tipo, username) values('{0}','{1}',{2}, '{3}')",
            e.Nome,e.Descricao, e.Tipo , username));
        }
        
        public int Editar(EstabelecimentoVM e, string username)
        {
            return dataContext.ExecuteNonQuery(String.Format("update estabelecimento set descricao='{1}',tipo={2} where nome='{0}' and username='{3}'",
            e.Nome, e.Descricao, e.Tipo, username));
        }

        public int Excluir(string nome, string username)
        {
            return dataContext.ExecuteNonQuery(String.Format("delete from estabelecimento where nome='{0}' and username='{1}'", nome, username));
        }

        public Dictionary<string, int> GetNomes(string categoria, string username)
        {
            Dictionary<string, int> tipos = new Dictionary<string, int>();
            DataTable table = dataContext.Select(String.Format("select     nome + ' (' + ifnull (descricao, 'N/A') + ')' as estabelecimento, tipo " +
                                "from         estabelecimento " +
                                "where     (tipo in " +
                                "(select     codigo " +
                                "from          tipo_operacao " +
                                "where      (categoria = '{0}'))) and username='{1}'" +
                                "order by nome", categoria, username));
            foreach (DataRow row in table.Rows)
            {
                tipos.Add(row["ESTABELECIMENTO"].ToString(), int.Parse(row["TIPO"].ToString()));
            }
            return tipos;
        }
    }
}