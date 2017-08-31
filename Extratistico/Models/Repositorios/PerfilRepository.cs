using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Data;
using Extratistico.Classes.DataContext;
using Extratistico.Models.Entidades;

namespace Extratistico.Models.Repositorios
{
    public class PerfilRepository
    {
        private DataContext dataContext = new DataContext();

        public void Commit()
        {
            dataContext.Commit();
        }

        public void RollBack()
        {
            dataContext.Rollback();
        }

        public List<Perfil> Select()
        {
            DataTable table = dataContext.Select("select *from perfil order by descricao");
            List<Perfil> perfis = new List<Perfil>();
            foreach (DataRow row in table.Rows)
            {
                perfis.Add(new Perfil()
                {
                    Codigo = Convert.ToInt32(row["Codigo"]),
                    Descricao = row["Descricao"].ToString()
                });
            }
            return perfis;
        }

        public List<Perfil> Select(string UsuarioUsername)
        {
            DataTable table = dataContext.Select(String.Format("select p.* from perfil p inner join autenticacao a on a.perfil=p.codigo where a.usuario='{0}'", UsuarioUsername));
            List<Perfil> perfis = new List<Perfil>();
            foreach (DataRow row in table.Rows)
            {
                perfis.Add(new Perfil()
                {
                    Codigo = Convert.ToInt32(row["Codigo"]),
                    Descricao = row["Descricao"].ToString()
                });
            }
            return perfis;
        }

        public Perfil SelectSingle(int codigo)
        {
            DataRow row = dataContext.SelectSingle(String.Format("select *from perfil where codigo={0}", codigo));
            return new Perfil()
            {
                Codigo = Convert.ToInt32(row["Codigo"]),
                Descricao = row["Descricao"].ToString()
            };
        }

        public Perfil SelectSingle(string descricao)
        {
            DataRow row = dataContext.SelectSingle(String.Format("select *from perfil where descricao='{0}'", descricao));
            return new Perfil()
            {
                Codigo = Convert.ToInt32(row["Codigo"]),
                Descricao = row["Descricao"].ToString()
            };
        }

        public int Add(Perfil perfil)
        {
            int codigo = Convert.ToInt32(dataContext.ExecuteScalar("select case when max(codigo) is null then 1 else max(codigo) + 1 end as codigo from perfil"));
            return dataContext.ExecuteNonQuery(String.Format("insert into perfil(codigo,descricao) values({0},'{1}')",codigo, perfil.Descricao));
        }

        public int Edit(Perfil perfil)
        {
            return dataContext.ExecuteNonQuery(String.Format("update perfil set descricao='{0}' where codigo={1}", perfil.Descricao,perfil.Codigo));
        }

        public int Delete(int codigo)
        {
            return dataContext.ExecuteNonQuery(String.Format("delete from perfil where codigo={0}", codigo));
        }

        public bool HasRelationships(int codigo)
        {
            return (Convert.ToInt32(dataContext.ExecuteScalar(String.Format("select count(*) from perfil p inner join autenticacao a on a.perfil=p.codigo where p.codigo={0}",codigo))) > 0);
        }
    }
}
