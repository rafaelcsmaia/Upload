using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.OracleClient;
using System.Data.Common;
using System.Linq;
//using Devart.Data.PostgreSql;
using MySql.Data.MySqlClient;
using System.Data.SqlServerCe;

namespace Extratistico.Classes.DataContext
{
    class DbConnectionFactory
    {
        public static DbConnection getDbConnection()
        {
            //Codigo para o AppHarbor!
            //var connectionString = ConfigurationManager.AppSettings["MYSQL_CONNECTION_STRING"];
            string connectionString = "server=6dca2b28-04bc-4f05-99e9-a4c200c82af7.mysql.sequelizer.com;database=db6dca2b2804bc4f0599e9a4c200c82af7;uid=glkypyllkirkaavi;pwd=Pc7BqxcHQtitRP4ikFxMdGWcvHWFvnNgyeCb5YFZzXprXhTSxGnCLmphn4gQJCnB";
            //var uri = new Uri(uriString);
            //var connectionString = new SqlConnectionStringBuilder
            //{
            //    DataSource = uri.Host,
            //    InitialCatalog = uri.AbsolutePath.Trim('/'),
            //    UserID = uri.UserInfo.Split(':').First(),
            //    Password = uri.UserInfo.Split(':').Last(),
            //}.ConnectionString;
            return new MySqlConnection(connectionString);
            //===============================================

            //switch (ConfigurationManager.ConnectionStrings["Default"].ProviderName)
            //{
            //    case "SQLServer":
            //        return new SqlCeConnection(ConfigurationManager.ConnectionStrings["Default"].ConnectionString);
            //    case "Oracle":
            //        return new OracleConnection(ConfigurationManager.ConnectionStrings["Default"].ConnectionString);
            //    case "MySQL":
            //        return new MySqlConnection(ConfigurationManager.ConnectionStrings["Default"].ConnectionString);
            //    //case "Devart.Data.PostgreSql":
            //    //    return new PgSqlConnection(ConfigurationManager.ConnectionStrings["Default"].ConnectionString);
            //    default:
            //        //throw new Exception("SGBD não implementado!");
            //        return null;
            //}
        }
    }
}
