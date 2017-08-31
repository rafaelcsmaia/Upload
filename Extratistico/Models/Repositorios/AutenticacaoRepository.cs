using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Extratistico.Models.Entidades;
using Extratistico.Classes.DataContext;
using System.Data;

namespace Extratistico.Models.Repositorios
{

    public class AutenticacaoRepository
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

        public Autenticacao SelectSingle(string usuario, int perfil)
        {
            DataRow row = dataContext.SelectSingle(String.Format("select *from autenticacao where usuario='{0}' and perfil={1}", usuario, perfil));
            return new Autenticacao()
            {
                Perfil = Convert.ToInt32(row["Funcionalidade"]),
                Usuario = row["Usuario"].ToString()
            };            
        }

        public List<Autenticacao> Select(string usuario)
        {
            DataTable table = dataContext.Select(String.Format("select *from autenticacao where usuario='{0}'", usuario));
            List<Autenticacao> autenticacoes = new List<Autenticacao>();
            foreach (DataRow row in table.Rows)
            {
                autenticacoes.Add(new Autenticacao()
                {
                    Perfil = Convert.ToInt32(row["Perfil"]),
                    Usuario = row["Usuario"].ToString()
                });            
            }
            return autenticacoes;
        }

        public int Add(Autenticacao a)
        {
            return dataContext.ExecuteNonQuery(String.Format("insert into autenticacao(usuario,perfil) values('{0}',{1})", a.Usuario, a.Perfil));
        }

        public int Delete(String usuario, int perfil)
        {
            return dataContext.ExecuteNonQuery(string.Format("delete from autenticacao where usuario='{0}' and perfil={1}", usuario, perfil));
        }

    }
}
