using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Extratistico.Classes.DataContext;
using Extratistico.Models.Entidades;
using System.Data;

namespace Extratistico.Models.Repositorios
{
    public class SMTPRepository
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

        public SMTP SelectSingle()
        {
            DataRow row = dataContext.SelectSingle("select *from smtp");
            if (row != null)
            {
                return new SMTP()
                {
                    Host = row["Host"].ToString(),
                    Porta = Convert.ToInt32(row["Porta"]),
                    Username = row["Username"].ToString(),
                    Password = row["Password"].ToString(),
                    SSLEnabled = (Convert.ToInt32(row["SSLEnabled"]) == 1 ? true : false)
                };
            }
            else
            {
                return null;
            }
        }

        public int Add(SMTP s)
        {
            int count = Convert.ToInt32(dataContext.ExecuteScalar("select count(*) from smtp"));
            if (count == 0)
            {
                return dataContext.ExecuteNonQuery(String.Format("insert into smtp(host, porta, username, password, sslenabled) values('{0}',{1},'{2}','{3}',{4})",
               s.Host, s.Porta, s.Username, s.Password, s.SSLEnabled ? 1 : 0));
            }
            else
            {
                return dataContext.ExecuteNonQuery(String.Format("update smtp set host='{0}', porta={1}, username='{2}', password='{3}', sslenabled={4}",
               s.Host, s.Porta, s.Username, s.Password, s.SSLEnabled ? 1 : 0));
            }           
        }
    }
}