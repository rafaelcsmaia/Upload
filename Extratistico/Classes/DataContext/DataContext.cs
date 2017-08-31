using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data;
using System.Reflection;

namespace Extratistico.Classes.DataContext
{
    /// <summary>
    ///  Classe para operações inter-bases de dados - DataContext™
    ///  Última edição 12/05/2012 -  Rafael Maia
    /// </summary>
    public class DataContext : IDisposable 
    {
        protected DbConnection dbConnection;
        protected DbCommand dbCommand;
        protected DbTransaction dbTransaction;
        protected DbDataReader dbDataReader;

        public DataContext()
        {
            if ((dbConnection = DbConnectionFactory.getDbConnection()) != null)
            {
                dbCommand = dbConnection.CreateCommand();
            }           
        }


        /// <summary>
        /// Retorna o tipo de base de dados escolhida.
        /// </summary>
        /// <returns></returns>
        public string GetProvider()
        {
            return dbConnection.GetType().Name;
        }

        /// <summary>
        /// Realiza um teste de conexão simples com a base de dados
        /// </summary>
        /// <param name="connectionString">String de conexão com a base de dados</param>
        /// <param name="providerName">Tipo do banco de dados escolhido</param>
        /// <returns></returns>
        public static bool TestConnection(string connectionString, string providerName)
        {
            DbConnection dbConnection;
            try
            {
                switch (providerName)
                {
                    case "SQLServer":
                        dbConnection = new System.Data.SqlServerCe.SqlCeConnection(connectionString);
                        break;
                    case "Oracle":
                        dbConnection = new System.Data.OracleClient.OracleConnection(connectionString);
                        break;
                    case "MySQL":
                        dbConnection = new MySql.Data.MySqlClient.MySqlConnection(connectionString);
                        break;
                    default:
                        throw new Exception("SGBD não implementado!");
                }


                dbConnection.Open();
                dbConnection.Close();
                dbConnection.Dispose();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// Salva todas as operações realizadas desde o início da transaction.
        /// </summary>
        public void Commit()
        {
            if (dbTransaction != null)
            {
                dbTransaction.Commit();
                dbTransaction = null;
            }
        }

        /// <summary>
        /// Cancela todas as operações realizadas desde o início da transaction.
        /// </summary>
        public void Rollback()
        {
            if (dbTransaction != null)
            {
                dbTransaction.Rollback();
                dbTransaction = null;
            }
        }

        /// <summary>
        /// Inicia uma transaction na base de dados.
        /// </summary>
        public void BeginTransaction()
        {
            if (dbTransaction == null)
            {
                dbTransaction = dbConnection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                dbCommand.Transaction = dbTransaction;
            }
        }

        /// <summary>
        ///  Abre a conexão com a base de dados.
        /// </summary>
        public void Connect()
        {
            if (dbConnection != null)
            {
                if (dbConnection.State != System.Data.ConnectionState.Open)
                {
                    try
                    {
                        dbConnection.Open();
                    }
                    catch (DbException ex)
                    {
                        Exception objException = new Exception("Connect: " + "\r\nException: " + ex.Message, ex);
                        throw objException;
                    }
                }
            }
            else
            {
                throw new Exception("Erro de configuração da base de dados!");
            }
        }

        /// <summary>
        ///  Fecha a conexão com a base de dados. Caso haja uma transaction aberta, todas as alterações serão salvas.
        /// </summary>
        public void Disconnect()
        {
            if (dbConnection.State != System.Data.ConnectionState.Closed)
            {
                Commit();
                dbConnection.Close();
            }
        }

        /// <summary>
        /// Executa uma instrução na base de dados.
        /// </summary>
        /// <param name="sql">Instrução SQL para execução.</param>
        /// <returns>Número de linhas afetadas pela instrução.</returns>
        public int ExecuteNonQuery(String sql)
        {
            try
            {
                Connect();
                BeginTransaction();
                dbCommand.CommandText = sql;
                return dbCommand.ExecuteNonQuery();
            }
            catch (DbException ex)
            {
                Exception objException = new Exception("ExecuteNonQuery: " + sql + "\r\nException: " + ex.Message, ex);
                throw objException;
            }
        }

        /// <summary>
        /// Executa uma pesquisa na base de dados que retorna uma única célula.
        /// </summary>
        /// <param name="sql">Instrução SQL para execução.</param>
        /// <returns>Objeto correspondente à pesquisa, situado na primeira coluna da primeira linha.</returns>
        public object ExecuteScalar(String sql)
        {
            try
            {
                Connect();
                dbCommand.CommandText = sql;
                return dbCommand.ExecuteScalar();
            }
            catch (DbException ex)
            {
                Exception objException = new Exception("ExecuteScalar: " + sql + "\r\nException: " + ex.Message, ex);
                throw objException;
            }
        }

        /// <summary>
        /// Realiza uma consulta na base de dados que retorna uma lista.
        /// </summary>
        /// <param name="sql">Instrução SQL para execução.</param>
        /// <returns>DataTable contendo o resultado da consulta.</returns>
        public DataTable Select(String sql)
        {
            DataTable result = new DataTable();
            try
            {
                Connect();
                dbCommand.CommandText = sql;
                dbDataReader = dbCommand.ExecuteReader();
                result.Load(dbDataReader);
                return result;
            }
            catch (DbException ex)
            {
                Exception objException = new Exception("Select: " + sql + "\r\nException: " + ex.Message, ex);
                throw objException;
            }
            finally
            {
                if (dbDataReader != null && !dbDataReader.IsClosed)
                    dbDataReader.Close();
            }
        }

        /// <summary>
        /// Realiza uma consulta na base de dados que retorna um único registro.
        /// </summary>
        /// <param name="sql">Instrução SQL para execução.</param>
        /// <returns>DataTable contendo o resultado da consulta.</returns>
        public DataRow SelectSingle(String sql)
        {
            DataTable result = new DataTable();
            try
            {
                Connect();
                dbCommand.CommandText = sql;
                dbDataReader = dbCommand.ExecuteReader(CommandBehavior.SingleRow);
                result.Load(dbDataReader);
                if (result.Rows.Count != 0)
                {
                    return result.Rows[0];
                }
                else
                {
                    return null;
                }
            }
            catch (DbException ex)
            {
                Exception objException = new Exception("SelectSingle: " + sql + "\r\nException: " + ex.Message, ex);
                throw objException;
            }
            finally
            {
                if (dbDataReader!=null && !dbDataReader.IsClosed)
                    dbDataReader.Close();
            }
        }

        ///// <summary>
        ///// Instancia um objeto do tipo DbConnection que será utilizado por todos os métodos de acesso à base de dados
        ///// </summary>
        ///// <param name="connectionString">String de conexão para o objeto de conexão a ser criado.</param>
        ///// <returns>Objeto de conexão de acordo com o provedor definido no arquivo de configurações.</returns>
        //public abstract DbConnection GetConnection(String connectionString);

        #region IDisposable Members

        public void Dispose()
        {
            Disconnect();
            dbConnection.Dispose();
            dbCommand.Dispose();
            if(dbTransaction!=null)
                dbTransaction.Dispose();
            if(dbDataReader!=null)
                dbDataReader.Dispose();
        }

        #endregion
    }
}
