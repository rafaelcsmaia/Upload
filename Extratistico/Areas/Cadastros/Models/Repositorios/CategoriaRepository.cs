using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Extratistico.Areas.Cadastros.Models.Entidades;
using Extratistico.Classes.DataContext;

namespace Extratistico.Areas.Cadastros.Models.Repositorios
{
    public class CategoriaRepository : IRepository
    {
        public List<CategoriaVM> Select(string username)
        {
            DataTable table = dataContext.Select(String.Format("select codigo, descricao, categoria, desconsiderar from tipo_operacao where username='{0}'", username));
            List<CategoriaVM> categorias = new List<CategoriaVM>();
            foreach (DataRow row in table.Rows)
            {
                categorias.Add(new CategoriaVM()
                {
                    Codigo = Convert.ToInt32(row["Codigo"]),
                    Descricao = row["Descricao"].ToString(),
                    Categoria = row["Categoria"].ToString(),
                    Desconsiderar = int.Parse(row["Desconsiderar"].ToString()) == 0 ? false : true
                });
            }
            return categorias;
        }

        public CategoriaVM SelectSingle(int codigo, string username)
        {
            DataRow row = dataContext.SelectSingle(String.Format("select codigo, descricao, categoria, desconsiderar from tipo_operacao " +
                      "where codigo={0} and username='{1}'", codigo, username));
            if (row != null)
            {
                CategoriaVM c = new CategoriaVM()
                {
                    Codigo = Convert.ToInt32(row["Codigo"]),
                    Descricao = row["Descricao"].ToString(),
                    Categoria = row["Categoria"].ToString(),
                    Desconsiderar = int.Parse(row["Desconsiderar"].ToString()) == 0 ? false : true
                };
                return c;
            }
            else
            {
                return null;
            }
        }

        public List<string> GetCategorias(string username)
        {
            List<string> categorias = new List<string>();
            DataTable table = dataContext.Select(String.Format("select distinct categoria from tipo_operacao where username='{0}' order by categoria", username));
            foreach (DataRow row in table.Rows)
            {
                categorias.Add(row["categoria"].ToString());
            }
            return categorias;
        }

        public Dictionary<int, string> GetTipos(string categoria, string username)
        {
            Dictionary<int, string> tipos = new Dictionary<int, string>();
            DataTable table = dataContext.Select(String.Format("select codigo, descricao from tipo_operacao where categoria='{0}' and username='{1}' order by descricao", categoria, username));
            foreach (DataRow row in table.Rows)
            {
                tipos.Add(int.Parse(row["CODIGO"].ToString()), row["descricao"].ToString());
            }
            return tipos;
        }

        public int Cadastrar(CategoriaVM c, string username)
        {
            return dataContext.ExecuteNonQuery(String.Format(
            "insert into tipo_operacao(descricao, categoria, desconsiderar, username) values('{0}','{1}',{2},'{3}')",
            c.Descricao, c.Categoria, c.Desconsiderar ? 1 : 0,username));
        }

        public int Editar(CategoriaVM c, string username)
        {
            return dataContext.ExecuteNonQuery(String.Format("update tipo_operacao set descricao='{1}',categoria='{2}', desconsiderar={3} where codigo={0} and username='{4}'",
                c.Codigo, c.Descricao, c.Categoria, c.Desconsiderar ? 1 : 0, username));
        }

        public int Excluir(int codigo, string username)
        {
            return dataContext.ExecuteNonQuery(String.Format("delete from tipo_operacao where codigo={0} and username='{1}'", codigo, username));
        }
    }
}