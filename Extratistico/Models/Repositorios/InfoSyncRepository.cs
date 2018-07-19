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
            MySqlCommand cmd = new MySqlCommand(string.Format("SELECT a.message, a.status, a.data, a.id from INFOSYNC a inner join (SELECT max(status) as status, id from INFOSYNC group by id) b on a.id=b.id and a.status=b.status where date_format(data,'%d/%m/%Y') = date_format(CURRENT_TIMESTAMP(),'%d/%m/%Y')"), cnn);
            cnn.Open();
            MySqlDataReader r = cmd.ExecuteReader();
            while (r.Read())
            {
                InfoSync i = new InfoSync();
                i.message= r["MESSAGE"].ToString();
                i.status = bool.Parse(r["STATUS"].ToString()) ? 1 : 0;
                i.id = r["ID"].ToString();
                resp.Add(i);
            }
            cnn.Close();
            return resp;
        }        
    }
}