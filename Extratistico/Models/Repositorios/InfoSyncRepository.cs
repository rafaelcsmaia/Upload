using Extratistico.Models.EntidadesWebAPI;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Extratistico.Models.Repositorios
{
    public class InfoSyncRepository
    {
        public IEnumerable<InfoSync> GetInfoSync()
        {
            List<InfoSync> resp = new List<InfoSync>();
            MySqlConnection cnn = new MySqlConnection("server=6dca2b28-04bc-4f05-99e9-a4c200c82af7.mysql.sequelizer.com;database=db6dca2b2804bc4f0599e9a4c200c82af7;uid=glkypyllkirkaavi;pwd=Pc7BqxcHQtitRP4ikFxMdGWcvHWFvnNgyeCb5YFZzXprXhTSxGnCLmphn4gQJCnB");
            MySqlCommand cmd = new MySqlCommand(string.Format("SELECT *from INFOSYNC where date_format(data,'%d/%m/%Y') = date_format(CURRENT_TIMESTAMP(),'%d/%m/%Y')"), cnn);
            cnn.Open();
            MySqlDataReader r = cmd.ExecuteReader();
            while (r.Read())
            {
                InfoSync i = new InfoSync();
                i.message= r["MESSAGE"].ToString();
                i.status = bool.Parse(r["STATUS"].ToString()) ? 1 : 0;
                resp.Add(i);
            }
            cnn.Close();
            return resp;
        }        
    }
}