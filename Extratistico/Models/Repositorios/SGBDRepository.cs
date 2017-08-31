using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Extratistico.Classes.DataContext;
using Extratistico.Models.Entidades;
using System.Data;
using System.Configuration;
using System.Web.Configuration;

namespace Extratistico.Models.Repositorios
{
    public class SGBDRepository
    {
        private Configuration config = WebConfigurationManager.OpenWebConfiguration("~");

        public SGBD SelectSingle()
        {
            //Lendo arquivo Web.config
            string conexao = System.Configuration.ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
            string tipo = System.Configuration.ConfigurationManager.ConnectionStrings["Default"].ProviderName;

            if (conexao == string.Empty || tipo == string.Empty)
            {
                return null;
            }

            return new SGBD()
            {
                Tipo = tipo,
                ConnectionString = conexao
            };
        }

        //public List<SGBD> Select()
        //{
        //    DataTable table = dataContext.Select(String.Format("Select * from sgbd"));
        //    List<SGBD> listasgbd = new List<SGBD>();
        //    foreach (DataRow row in table.Rows)
        //    {
        //        listasgbd.Add(new SGBD()
        //        {
        //            Tipo = row["Tipo"].ToString(),
        //            ConnectionString = row["ConnectionString"].ToString()
        //        });
        //    }
        //    return listasgbd;
        //}

        public int Insert(SGBD sgbd)
        {
            //Escrevendo no arquivo Web.config
            config.ConnectionStrings.ConnectionStrings["Default"].ConnectionString = sgbd.ConnectionString;
            config.ConnectionStrings.ConnectionStrings["Default"].ProviderName = sgbd.Tipo;
            //Salvando Alterações
            config.Save(ConfigurationSaveMode.Modified);
            return 0;
        }
    }
}