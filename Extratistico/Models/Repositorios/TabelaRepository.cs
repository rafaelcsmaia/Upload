using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Extratistico.Classes.DataContext;
using System.Data;
using Extratistico.Models.Entidades;

namespace Extratistico.Models.Repositorios
{
    public class TabelaRepository
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

        public bool Create(string path, string tipo)
        {
            bool resp = true;
            string sql = System.IO.File.ReadAllText(path + tipo.Split('.')[tipo.Split('.').Length - 1].ToString() + ".sql");
            foreach (var command in sql.Split(';'))
            {
                if (command.Trim() != string.Empty)
                {
                    try
                    {
                        dataContext.ExecuteNonQuery(command);
                        dataContext.Commit();
                    }
                    catch (Exception e)
                    {
                        resp = false;
                    }
                }
            }
            return resp;
        }
            
    }

    public enum TABELAS { USUARIO, PERFIL, FUNCIONALIDADE, AUTENTICACAO, PERMISSAO, SMTP }
}