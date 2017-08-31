using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Extratistico.Models.Entidades;
using System.Data;
using Extratistico.Classes.DataContext;

namespace Extratistico.Models.Repositorios
{
    public class PermissaoRepository
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

        public Permissao SelectSingle(int perfil, int funcionalidade)
        {
            DataRow row = dataContext.SelectSingle(String.Format("select *from permissao where perfil={0} and funcionalidade={1}", perfil, funcionalidade));
            return new Permissao()
            {
                Funcionalidade = Convert.ToInt32(row["Funcionalidade"]),
                Perfil = Convert.ToInt32(row["Perfil"]) 
            };            
        }

        public List<Permissao> Select(int perfil)
        {
            DataTable table = dataContext.Select(String.Format("select *from permissao where perfil={0}", perfil));
            List<Permissao> permissoes = new List<Permissao>();
            foreach (DataRow row in table.Rows)
            {
                permissoes.Add(new Permissao()
                {
                    Funcionalidade = Convert.ToInt32(row["Funcionalidade"]),
                    Perfil = Convert.ToInt32(row["Perfil"])
                });
            }
            return permissoes;
        }

        public int Add(Permissao p)
        {
            return dataContext.ExecuteNonQuery(String.Format("insert into permissao(funcionalidade,perfil) values({0},{1})",p.Funcionalidade,p.Perfil));
        }

        public int Delete(int perfil, int funcionalidade)
        {
            return dataContext.ExecuteNonQuery(String.Format("delete from permissao where funcionalidade={0} and perfil={1}", funcionalidade,perfil));
        }

        public int Delete(int perfil)
        {
            return dataContext.ExecuteNonQuery(String.Format("delete from permissao where perfil={0}",  perfil));
        }
    }
}
