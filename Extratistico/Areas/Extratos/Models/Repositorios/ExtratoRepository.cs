using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using Extratistico.Areas.Extratos.Models.Entidades;
using Extratistico.Classes.DataContext;
using Extratistico.Classes.Estatistica;
using Extratistico.Areas.Cadastros.Models.Entidades;
using Extratistico.Models.EntidadesWebAPI;

namespace Extratistico.Areas.Extratos.Models.Repositorios
{
    public class ExtratoRepository : IRepository
    {


        public int ExcluirExtratoCodigo(string username, string tipo, string documento)
        {
            return dataContext.ExecuteNonQuery(string.Format("delete from {0} where username='{1}' and documento='{2}'",
                tipo.ToUpper() == "CC" ? "extrato_credito" : tipo.ToUpper() == "D" ? "extrato_dinheiro" : "extrato", username, documento));
        }

        public int ExcluirExtratoLote(string username, string tipo, string dataLote)
        {
            return dataContext.ExecuteNonQuery(string.Format("delete from {0} where username='{1}' and lote_carga= convert('{2}', datetime)",
                tipo.ToUpper() == "CC" ? "extrato_credito" : "extrato", username, dataLote));
        }

        public int CadastrarGastoDinheiro(DinheiroVM dinheiroVM, string username)
        {
            return dataContext.ExecuteNonQuery(string.Format("insert into extrato_dinheiro(data, operacao, credito, debito, estabelecimento, tipo_operacao, username) " +
                "values(STR_TO_DATE('{0}','%d/%m/%Y'),'{1}',{2},{3},'{4}', {5}, '{6}')",
                dinheiroVM.Data.ToString("dd/MM/yyyy"),
                dinheiroVM.Operacao,
                dinheiroVM.Valor > 0 ? dinheiroVM.Valor : 0,
                dinheiroVM.Valor < 0 ? dinheiroVM.Valor : 0,
                dinheiroVM.Estabelecimento,
                dinheiroVM.Tipo,
                username));
        }

        public double SaldoAnual(int ano, string username)
        {
            List<object> o = new List<object>();
            double total;
            double.TryParse(dataContext.ExecuteScalar(String.Format("select sum(debito) + sum(debito_us * cotacao_dolar) + sum(credito) as valor from( " +
                    "select e.username, e.documento, e.data,  " +
                    "ifnull(case when t2 .descricao = '' then t2 .nome else t2 .descricao end,e.operacao) as operacao,  " +
                    "ifnull (case when t .descricao = '' then t .nome else t .descricao end,  e.estabelecimento) as estabelecimento, " +
                     "e.credito, e.debito, e.debito_us, e.cotacao_dolar, e.sobrescrito,  " +
                     "ifnull(e.tipo,ifnull(t.tipo,t2.tipo)) as tipo, " +
                     "e.procedencia, " +
                    "ifnull(o.descricao, ifnull(o2.descricao,ifnull(o3.descricao,'A Definir'))) as tipooperacao, " +
                    "ifnull(o.categoria, ifnull(o2.categoria,ifnull(o3.categoria,'A Definir'))) as categoria ," +
                    "ifnull(o.desconsiderar, ifnull(o2.desconsiderar,ifnull(o3.desconsiderar,0))) as desconsiderar " +
                    "from (select     username,documento, data, operacao, estabelecimento, credito, debito, 0 as debito_us, 0 as cotacao_dolar, sobrescrito, tipo_operacao as tipo, 'CD' as procedencia  " +
                    "from         extrato as e  " +
                    "union  " +
                    "select     username, documento, data, operacao, estabelecimento, 0 as credito, debito, debito_us,cotacao_dolar, 0 as expr1, tipo_operacao as tipo, 'CC' as procedencia  " +
                    "from         extrato_credito as c  " +
                    "union  " +
                    "select     username, cast(documento as char(100)) as documento, data, operacao, estabelecimento, credito, debito, 0 as debito_us, 0 as cotacao_dolar, 0 as expr1, tipo_operacao as tipo, 'D' as procedencia  " +
                    "from         extrato_dinheiro as d) e left join   " +
                    "estabelecimento t on t.nome=e.estabelecimento left join  " +
                    "estabelecimento t2 on t2.nome=e.operacao left join  " +
                    "tipo_operacao o on o.codigo=e.tipo left join  " +
                    "tipo_operacao o2 on o2.codigo=t.tipo left join  " +
                    "tipo_operacao o3 on o3.codigo=t2.tipo) e " +
                    "where categoria <> 'Saque' and e.data between convert('{0}-01-01 00:00:00', datetime) and convert('{0}-12-31 23:59:59', datetime) and e.desconsiderar=0 and username='{1}' "
                    , ano, username)).ToString(), out total);
            return total;
        }

        public CategorizarExtratoVM SelectSingle(string documento, string username)
        {
            DataRow row = dataContext.SelectSingle(string.Format("select * from( " +
                    "select e.username, e.documento, e.data,  " +
                    "ifnull(case when t2 .descricao = '' then t2 .nome else t2 .descricao end,e.operacao) as operacao,  " +
                    "ifnull (case when t .descricao = '' then t .nome else t .descricao end,  e.estabelecimento) as estabelecimento, " +
                     "e.credito, e.debito, e.debito_us, e.cotacao_dolar, e.sobrescrito,  " +
                     "ifnull(e.tipo,ifnull(t.tipo,t2.tipo)) as tipo, " +
                     "e.tipo as tipoextrato, " +
                     "e.procedencia, " +
                    "ifnull(o.descricao, ifnull(o2.descricao,ifnull(o3.descricao,'A Definir'))) as tipooperacao, " +
                    "ifnull(o.categoria, ifnull(o2.categoria,ifnull(o3.categoria,'A Definir'))) as categoria ," +
                    "ifnull(o.desconsiderar, ifnull(o2.desconsiderar,ifnull(o3.desconsiderar,0))) as desconsiderar " +
                    "from (select     username,documento, data, operacao, estabelecimento, credito, debito, 0 as debito_us, 0 as cotacao_dolar, sobrescrito, tipo_operacao as tipo, 'CD' as procedencia  " +
                    "from         extrato as e  " +
                    "union  " +
                    "select     username, documento, data, operacao, estabelecimento, 0 as credito, debito, debito_us,cotacao_dolar, 0 as expr1, tipo_operacao as tipo, 'CC' as procedencia  " +
                    "from         extrato_credito as c  " +
                    "union  " +
                    "select     username, cast(documento as char(100)) as documento, data, operacao, estabelecimento, credito, debito, 0 as debito_us, 0 as cotacao_dolar, 0 as expr1, tipo_operacao as tipo, 'D' as procedencia  " +
                    "from         extrato_dinheiro as d) e left join   " +
                    "estabelecimento t on t.nome=e.estabelecimento left join  " +
                    "estabelecimento t2 on t2.nome=e.operacao left join  " +
                    "tipo_operacao o on o.codigo=e.tipo left join  " +
                    "tipo_operacao o2 on o2.codigo=t.tipo left join  " +
                    "tipo_operacao o3 on o3.codigo=t2.tipo) e " +
                    "where documento='{0}' and e.desconsiderar=0 and username='{1}'",
                    documento, username));
            CategorizarExtratoVM e = new CategorizarExtratoVM();
            if (row != null)
            {
                return new CategorizarExtratoVM()
                {
                    Documento = row["documento"].ToString(),
                    Estabelecimento = row["Estabelecimento"].ToString(),
                    Operacao = row["Operacao"].ToString(),
                    Data = DateTime.Parse(row["Data"].ToString()),
                    Valor = double.Parse(row["Debito"].ToString()) + double.Parse(row["Credito"].ToString(), format) + (double.Parse(row["Debito_US"].ToString(), format) * double.Parse(row["Cotacao_Dolar"].ToString(), format)),
                    Tipo = row["Tipo"].ToString(),
                    TipoExtrato = row["TipoExtrato"].ToString(),
                    TipoOperacao = row["TipoOperacao"].ToString(),
                    Categoria = row["Categoria"].ToString(),
                    Procedencia = row["Procedencia"].ToString()
                };
            }
            else
            {
                return null;
            }
        }

        public int Categorizar(CategorizarExtratoVM e, string username)
        {
            if (e.Procedencia == "D")
            {
                return dataContext.ExecuteNonQuery(String.Format("update extrato_dinheiro set tipo_operacao={1} where documento={0} and username='{2}'",
                e.Documento, e.Tipo, username));
            }
            else if (e.Procedencia == "CC")
            {
                return dataContext.ExecuteNonQuery(String.Format("update extrato_credito set tipo_operacao={1} where documento='{0}' and username='{2}'",
                e.Documento, e.Tipo, username));
            }
            else
            {
                return dataContext.ExecuteNonQuery(String.Format("update extrato set tipo_operacao={1} where documento='{0}' and username='{2}' ",
                e.Documento, e.Tipo, username));
            }
        }

        public List<object> GastosCategorias(DateTime dtIni, DateTime dtFim, string username)
        {
            List<object> o = new List<object>();
            DataTable table = dataContext.Select(String.Format("select categoria, sum(debito * -1) + sum(debito_us * -1 * cotacao_dolar) as valor from( " +
                    "select e.username, e.documento, e.data,  " +
                    "ifnull(case when t2 .descricao = '' then t2 .nome else t2 .descricao end,e.operacao) as operacao,  " +
                    "ifnull (case when t .descricao = '' then t .nome else t .descricao end,  e.estabelecimento) as estabelecimento, " +
                     "e.credito, e.debito, e.debito_us, e.cotacao_dolar, e.sobrescrito,  " +
                     "ifnull(e.tipo,ifnull(t.tipo,t2.tipo)) as tipo, " +
                     "e.procedencia, " +
                    "ifnull(o.descricao, ifnull(o2.descricao,ifnull(o3.descricao,'A Definir'))) as tipooperacao, " +
                    "ifnull(o.categoria, ifnull(o2.categoria,ifnull(o3.categoria,'A Definir'))) as categoria ," +
                    "ifnull(o.desconsiderar, ifnull(o2.desconsiderar,ifnull(o3.desconsiderar,0))) as desconsiderar " +
                    "from (select     username,documento, data, operacao, estabelecimento, credito, debito, 0 as debito_us, 0 as cotacao_dolar, sobrescrito, tipo_operacao as tipo, 'CD' as procedencia  " +
                    "from         extrato as e  " +
                    "union  " +
                    "select    username, documento, data, operacao, estabelecimento, 0 as credito, debito, debito_us,cotacao_dolar, 0 as expr1, tipo_operacao as tipo, 'CC' as procedencia  " +
                    "from         extrato_credito as c  " +
                    "union  " +
                    "select     username, cast(documento as char(100)) as documento, data, operacao, estabelecimento, credito, debito, 0 as debito_us, 0 as cotacao_dolar, 0 as expr1, tipo_operacao as tipo, 'D' as procedencia  " +
                    "from         extrato_dinheiro as d) e left join   " +
                    "estabelecimento t on t.nome=e.estabelecimento left join  " +
                    "estabelecimento t2 on t2.nome=e.operacao left join  " +
                    "tipo_operacao o on o.codigo=e.tipo left join  " +
                    "tipo_operacao o2 on o2.codigo=t.tipo left join  " +
                    "tipo_operacao o3 on o3.codigo=t2.tipo) e " +
                    "where categoria <> 'Saque' and  (debito < 0 or debito_us < 0) and  e.data between convert('{0}-{1}-{2} 00:00:00', datetime) and convert('{3}-{4}-{5} 23:59:59', datetime) and e.desconsiderar=0 and username='{6}' " +
                    "GROUP BY categoria", dtIni.Year, dtIni.Month, dtIni.Day, dtFim.Year, dtFim.Month, dtFim.Day, username));
            o.Add(new object[] { "Categoria", "Valor" });
            foreach (DataRow row in table.Rows)
            {
                o.Add(new object[] { row[0].ToString(), double.Parse(row[1].ToString(), format) });
            }
            return o;
        }

        public List<object> RecebimentosCategorias(DateTime dtIni, DateTime dtFim, string username)
        {
            List<object> o = new List<object>();
            DataTable table = dataContext.Select(String.Format("select categoria, sum(credito) as valor from( " +
                    "select e.username, e.documento, e.data,  " +
                    "ifnull(case when t2 .descricao = '' then t2 .nome else t2 .descricao end,e.operacao) as operacao,  " +
                    "ifnull (case when t .descricao = '' then t .nome else t .descricao end,  e.estabelecimento) as estabelecimento, " +
                     "e.credito, e.debito, e.debito_us, e.cotacao_dolar, e.sobrescrito,  " +
                     "ifnull(e.tipo,ifnull(t.tipo,t2.tipo)) as tipo, " +
                     "e.procedencia, " +
                    "ifnull(o.descricao, ifnull(o2.descricao,ifnull(o3.descricao,'A Definir'))) as tipooperacao, " +
                    "ifnull(o.categoria, ifnull(o2.categoria,ifnull(o3.categoria,'A Definir'))) as categoria ," +
                    "ifnull(o.desconsiderar, ifnull(o2.desconsiderar,ifnull(o3.desconsiderar,0))) as desconsiderar " +
                    "from (select     username,documento, data, operacao, estabelecimento, credito, debito, 0 as debito_us, 0 as cotacao_dolar, sobrescrito, tipo_operacao as tipo, 'CD' as procedencia  " +
                    "from         extrato as e  " +
                    "union  " +
                    "select   username,   documento, data, operacao, estabelecimento, 0 as credito, debito, debito_us,cotacao_dolar, 0 as expr1, tipo_operacao as tipo, 'CC' as procedencia  " +
                    "from         extrato_credito as c  " +
                    "union  " +
                    "select    username, cast(documento as char(100)) as documento, data, operacao, estabelecimento, credito, debito, 0 as debito_us, 0 as cotacao_dolar, 0 as expr1, tipo_operacao as tipo, 'D' as procedencia  " +
                    "from         extrato_dinheiro as d) e left join   " +
                    "estabelecimento t on t.nome=e.estabelecimento left join  " +
                    "estabelecimento t2 on t2.nome=e.operacao left join  " +
                    "tipo_operacao o on o.codigo=e.tipo left join  " +
                    "tipo_operacao o2 on o2.codigo=t.tipo left join  " +
                    "tipo_operacao o3 on o3.codigo=t2.tipo) e " +
                    "where  credito > 0 and  e.data between convert('{0}-{1}-{2} 00:00:00', datetime) and convert('{3}-{4}-{5} 23:59:59', datetime) and e.desconsiderar=0 and username='{6}' " +
                    "group by categoria", dtIni.Year, dtIni.Month, dtIni.Day, dtFim.Year, dtFim.Month, dtFim.Day, username));
            o.Add(new object[] { "Categoria", "Valor" });
            foreach (DataRow row in table.Rows)
            {
                o.Add(new object[] { row[0].ToString(), double.Parse(row[1].ToString(), format) });
            }
            return o;
        }

        public List<object> GastosDetalhesCategorias(DateTime dtIni, DateTime dtFim, string categoria, string username)
        {
            List<object> o = new List<object>();
            DataTable table = dataContext.Select(String.Format("select tipooperacao, sum(debito * -1) + sum(debito_us * -1 * cotacao_dolar) as valor from( " +
                    "select e.username, e.documento, e.data,  " +
                    "ifnull(case when t2 .descricao = '' then t2 .nome else t2 .descricao end,e.operacao) as operacao,  " +
                    "ifnull (case when t .descricao = '' then t .nome else t .descricao end,  e.estabelecimento) as estabelecimento, " +
                     "e.credito, e.debito, e.debito_us, e.cotacao_dolar, e.sobrescrito,  " +
                     "ifnull(e.tipo,ifnull(t.tipo,t2.tipo)) as tipo, " +
                     "e.procedencia, " +
                    "ifnull(o.descricao, ifnull(o2.descricao,ifnull(o3.descricao,'A Definir'))) as tipooperacao, " +
                    "ifnull(o.categoria, ifnull(o2.categoria,ifnull(o3.categoria,'A Definir'))) as categoria, " +
                    "ifnull(o.desconsiderar, ifnull(o2.desconsiderar,ifnull(o3.desconsiderar,0))) as desconsiderar " +
                    "from (select     username,documento, data, operacao, estabelecimento, credito, debito, 0 as debito_us, 0 as cotacao_dolar, sobrescrito, tipo_operacao as tipo, 'CD' as procedencia  " +
                    "from         extrato as e  " +
                    "union  " +
                    "select    username, documento, data, operacao, estabelecimento, 0 as credito, debito, debito_us,cotacao_dolar, 0 as expr1, tipo_operacao as tipo, 'CC' as procedencia  " +
                    "from         extrato_credito as c  " +
                    "union  " +
                    "select    username, cast(documento as char(100)) as documento, data, operacao, estabelecimento, credito, debito, 0 as debito_us, 0 as cotacao_dolar, 0 as expr1, tipo_operacao as tipo, 'D' as procedencia  " +
                    "from         extrato_dinheiro as d) e left join   " +
                    "estabelecimento t on t.nome=e.estabelecimento left join  " +
                    "estabelecimento t2 on t2.nome=e.operacao left join  " +
                    "tipo_operacao o on o.codigo=e.tipo left join  " +
                    "tipo_operacao o2 on o2.codigo=t.tipo left join  " +
                    "tipo_operacao o3 on o3.codigo=t2.tipo) e " +
                    "where categoria <> 'Saque' and  (debito < 0 or debito_us < 0) and  e.data between convert('{0}-{1}-{2} 00:00:00', datetime) and convert('{3}-{4}-{5} 23:59:59', datetime) and categoria='{6}' and e.desconsiderar=0 and username='{7}' " +
                    "group by tipooperacao", dtIni.Year, dtIni.Month, dtIni.Day, dtFim.Year, dtFim.Month, dtFim.Day, categoria, username));
            o.Add(new object[] { "Tipo", "Valor" });
            foreach (DataRow row in table.Rows)
            {
                o.Add(new object[] { row[0].ToString(), double.Parse(row[1].ToString(), format) });
            }
            return o;
        }

        public List<object> RecebimentosDetalhesCategorias(DateTime dtIni, DateTime dtFim, string categoria, string username)
        {
            List<object> o = new List<object>();
            DataTable table = dataContext.Select(String.Format("select tipooperacao, sum(credito) as valor from( " +
                    "select e.username, e.documento, e.data,  " +
                    "ifnull(case when t2 .descricao = '' then t2 .nome else t2 .descricao end,e.operacao) as operacao,  " +
                    "ifnull (case when t .descricao = '' then t .nome else t .descricao end,  e.estabelecimento) as estabelecimento, " +
                     "e.credito, e.debito, e.debito_us, e.cotacao_dolar, e.sobrescrito,  " +
                     "ifnull(e.tipo,ifnull(t.tipo,t2.tipo)) as tipo, " +
                     "e.procedencia, " +
                    "ifnull(o.descricao, ifnull(o2.descricao,ifnull(o3.descricao,'A Definir'))) as tipooperacao, " +
                    "ifnull(o.categoria, ifnull(o2.categoria,ifnull(o3.categoria,'A Definir'))) as categoria, " +
                    "ifnull(o.desconsiderar, ifnull(o2.desconsiderar,ifnull(o3.desconsiderar,0))) as desconsiderar " +
                    "from (select     username,documento, data, operacao, estabelecimento, credito, debito, 0 as debito_us, 0 as cotacao_dolar, sobrescrito, tipo_operacao as tipo, 'CD' as procedencia  " +
                    "from         extrato as e  " +
                    "union  " +
                    "select    username, documento, data, operacao, estabelecimento, 0 as credito, debito, debito_us,cotacao_dolar, 0 as expr1, tipo_operacao as tipo, 'CC' as procedencia  " +
                    "from         extrato_credito as c  " +
                    "union  " +
                    "select    username, cast(documento as char(100)) as documento, data, operacao, estabelecimento, credito, debito, 0 as debito_us, 0 as cotacao_dolar, 0 as expr1, tipo_operacao as tipo, 'D' as procedencia  " +
                    "from         extrato_dinheiro as d) e left join   " +
                    "estabelecimento t on t.nome=e.estabelecimento left join  " +
                    "estabelecimento t2 on t2.nome=e.operacao left join  " +
                    "tipo_operacao o on o.codigo=e.tipo left join  " +
                    "tipo_operacao o2 on o2.codigo=t.tipo left join  " +
                    "tipo_operacao o3 on o3.codigo=t2.tipo) e " +
                    "where credito > 0 and  e.data between convert('{0}-{1}-{2} 00:00:00', datetime) and convert('{3}-{4}-{5} 23:59:59', datetime) and categoria='{6}' and e.desconsiderar=0 and username='{7}'" +
                    "group by tipooperacao", dtIni.Year, dtIni.Month, dtIni.Day, dtFim.Year, dtFim.Month, dtFim.Day, categoria, username));
            o.Add(new object[] { "Tipo", "Valor" });
            foreach (DataRow row in table.Rows)
            {
                o.Add(new object[] { row[0].ToString(), double.Parse(row[1].ToString(), format) });
            }
            return o;
        }

        public List<object> ReportResumoAnual(int ano, string username)
        {
            CultureInfo brasil = new CultureInfo("pt-BR");
            List<object> o = new List<object>();
            List<JoinHelper> iDebitos = new List<JoinHelper>();
            List<JoinHelper> iTotal = new List<JoinHelper>();

            double saldoAcumulado = 0;
            try
            {
                saldoAcumulado = double.Parse(dataContext.ExecuteScalar(string.Format("select     ifnull(saldo,0) + ifnull(dinheiro,0) as saldo from         valores_iniciais where username='{0}'", username)).ToString(), format);
            }
            catch (Exception)
            {

            }


            int yearLooper = 2014;
            while (yearLooper < ano)
            {
                saldoAcumulado += SaldoAnual(yearLooper, username);
                yearLooper++;
            }

            DataTable debitos = dataContext.Select(String.Format("select     month( e.data) as mes, sum(debito * -1) + sum(debito_us * -1 * cotacao_dolar) as valor " +
"from( " +
                    "select e.username, e.documento, e.data,  " +
                    "ifnull(case when t2 .descricao = '' then t2 .nome else t2 .descricao end,e.operacao) as operacao,  " +
                    "ifnull (case when t .descricao = '' then t .nome else t .descricao end,  e.estabelecimento) as estabelecimento, " +
                     "e.credito, e.debito,e.debito_us, e.cotacao_dolar, e.sobrescrito, " +
                     "ifnull(e.tipo,ifnull(t.tipo,t2.tipo)) as tipo, " +
                     "e.procedencia, " +
                    "ifnull(o.descricao, ifnull(o2.descricao,ifnull(o3.descricao,'A Definir'))) as tipooperacao, " +
                    "ifnull(o.categoria, ifnull(o2.categoria,ifnull(o3.categoria,'A Definir'))) as categoria, " +
                    "ifnull(o.desconsiderar, ifnull(o2.desconsiderar,ifnull(o3.desconsiderar,0))) as desconsiderar " +
                    "from (select     username,documento, data, operacao, estabelecimento, credito, debito, 0 as debito_us, 0 as cotacao_dolar, sobrescrito, tipo_operacao as tipo, 'CD' as procedencia " +
                    "from         extrato as e " +
                    "union  " +
                    "select    username, documento, data, operacao, estabelecimento, 0 as credito, debito, debito_us,cotacao_dolar, 0 as expr1, tipo_operacao as tipo, 'CC' as procedencia  " +
                    "from         extrato_credito as c  " +
                    "union " +
                    "select    username, cast(documento as char(100)) as documento, data, operacao, estabelecimento, credito, debito, 0 as debito_us, 0 as cotacao_dolar, 0 as expr1, tipo_operacao as tipo, 'D' as procedencia " +
                    "from         extrato_dinheiro as d) e left join  " +
                    "estabelecimento t on t.nome=e.estabelecimento left join  " +
                    "estabelecimento t2 on t2.nome=e.operacao left join  " +
                    "tipo_operacao o on o.codigo=e.tipo left join  " +
                    "tipo_operacao o2 on o2.codigo=t.tipo left join  " +
                    "tipo_operacao o3 on o3.codigo=t2.tipo) e " +
"where   categoria <> 'Saque' and  debito<0 and   e.data between convert('{0}-01-01 00:00:00', datetime) and convert('{0}-12-31 23:59:59', datetime) and e.desconsiderar=0 and username='{1}' group by month( e.data) order by month( e.data)"
                        , ano, username));
            foreach (DataRow row in debitos.Rows)
            {
                iDebitos.Add(new JoinHelper()
                {
                    mes = row[0].ToString(),
                    debito = double.Parse(row[1].ToString(), format),
                    credito = 0
                });
            }

            DataTable creditos = dataContext.Select(String.Format("select     month( e.data) as mes, sum(credito) as valor " +
"from( " +
                    "select e.username,e.documento, e.data,  " +
                    "ifnull(case when t2 .descricao = '' then t2 .nome else t2 .descricao end,e.operacao) as operacao,  " +
                    "ifnull (case when t .descricao = '' then t .nome else t .descricao end,  e.estabelecimento) as estabelecimento, " +
                     "e.credito, e.debito,e.debito_us, e.cotacao_dolar, e.sobrescrito, " +
                     "ifnull(e.tipo,ifnull(t.tipo,t2.tipo)) as tipo, " +
                     "e.procedencia, " +
                    "ifnull(o.descricao, ifnull(o2.descricao,ifnull(o3.descricao,'A Definir'))) as tipooperacao, " +
                    "ifnull(o.categoria, ifnull(o2.categoria,ifnull(o3.categoria,'A Definir'))) as categoria ," +
                    "ifnull(o.desconsiderar, ifnull(o2.desconsiderar,ifnull(o3.desconsiderar,0))) as desconsiderar " +
                    "from (select     username,documento, data, operacao, estabelecimento, credito, debito, 0 as debito_us, 0 as cotacao_dolar, sobrescrito, tipo_operacao as tipo, 'CD' as procedencia " +
                    "from         extrato as e " +
                    "union  " +
                    "select    username, documento, data, operacao, estabelecimento, 0 as credito, debito, debito_us,cotacao_dolar, 0 as expr1, tipo_operacao as tipo, 'CC' as procedencia  " +
                    "from         extrato_credito as c  " +
                    "union " +
                    "select    username, cast(documento as char(100)) as documento, data, operacao, estabelecimento, credito, debito, 0 as debito_us, 0 as cotacao_dolar, 0 as expr1, tipo_operacao as tipo, 'D' as procedencia " +
                    "from         extrato_dinheiro as d) e left join  " +
                    "estabelecimento t on t.nome=e.estabelecimento left join  " +
                    "estabelecimento t2 on t2.nome=e.operacao left join  " +
                    "tipo_operacao o on o.codigo=e.tipo left join  " +
                    "tipo_operacao o2 on o2.codigo=t.tipo left join  " +
                    "tipo_operacao o3 on o3.codigo=t2.tipo) e " +
"where   credito>0 and    e.data between convert('{0}-01-01 00:00:00', datetime) and convert('{0}-12-31 23:59:59', datetime) and e.desconsiderar=0 and username='{1}' group by month( e.data) order by month( e.data)"
                        , ano, username));

            foreach (DataRow row in creditos.Rows)
            {
                if (iDebitos.Where(x => x.mes == row[0].ToString()).Count() == 0)
                {
                    iTotal.Add(new JoinHelper()
                    {
                        mes = new DateTime(2014, int.Parse(row[0].ToString()), 1).ToString("MMMM", brasil),
                        debito = 0,
                        credito = double.Parse(row[1].ToString(), format),
                        saldo = double.Parse(row[1].ToString(), format) + saldoAcumulado
                    });
                    saldoAcumulado = double.Parse(row[1].ToString(), format) + saldoAcumulado;
                }
                else
                {
                    iTotal.Add(new JoinHelper()
                    {
                        mes = new DateTime(2014, int.Parse(row[0].ToString()), 1).ToString("MMMM", brasil),
                        debito = iDebitos.First(x => x.mes == row[0].ToString()).debito,
                        credito = double.Parse(row[1].ToString(), format),
                        saldo = double.Parse(row[1].ToString(), format) - iDebitos.First(x => x.mes == row[0].ToString()).debito + saldoAcumulado
                    });
                    saldoAcumulado = double.Parse(row[1].ToString(), format) - iDebitos.First(x => x.mes == row[0].ToString()).debito + saldoAcumulado;
                }
            }




            o.Add(new object[] { "Mes", "Debito", "Credito", "Saldo" });
            foreach (JoinHelper d in iTotal)
            {
                o.Add(new object[] { d.mes, d.debito, d.credito, d.saldo });
            }
            return o;
        }

        public List<ExtratoVM> ExtratoCategoria(string categoria, DateTime dtIni, DateTime dtFim, string username)
        {
            DataTable table = dataContext.Select(String.Format("select * from (select e.username, e.documento, e.data,   " +
                    "ifnull(case when t2 .descricao = '' then t2 .nome else t2 .descricao end,e.operacao) as operacao,   " +
                    "ifnull (case when t .descricao = '' then t .nome else t .descricao end,  e.estabelecimento) as estabelecimento,  " +
                    "e.credito, e.debito, e.debito_us, e.cotacao_dolar, e.sobrescrito,  " +
                     "ifnull(e.tipo,ifnull(t.tipo,t2.tipo)) as tipo,  " +
                     "e.procedencia,  " +
                    "ifnull(o.descricao, ifnull(o2.descricao,ifnull(o3.descricao,'A Definir'))) as tipooperacao,  " +
                    "ifnull(o.categoria, ifnull(o2.categoria,ifnull(o3.categoria,'A Definir'))) as categoria  ," +
                    "ifnull(o.desconsiderar, ifnull(o2.desconsiderar,ifnull(o3.desconsiderar,0))) as desconsiderar " +
                    "from (select     username,documento, data, operacao, estabelecimento, credito, debito, 0 as debito_us, 0 as cotacao_dolar, sobrescrito, tipo_operacao as tipo, 'CD' as procedencia  " +
                    "from         extrato as e  " +
                    "union  " +
                    "select    username, documento, data, operacao, estabelecimento, 0 as credito, debito, debito_us,cotacao_dolar, 0 as expr1, tipo_operacao as tipo, 'CC' as procedencia  " +
                    "from         extrato_credito as c  " +
                    "union  " +
                    "select    username, cast(documento as char(100)) as documento, data, operacao, estabelecimento, credito, debito, 0 as debito_us, 0 as cotacao_dolar, 0 as expr1, tipo_operacao as tipo, 'D' as procedencia  " +
                    "from         extrato_dinheiro as d) e left join   " +
                    "estabelecimento t on t.nome=e.estabelecimento left join   " +
                    "estabelecimento t2 on t2.nome=e.operacao left join   " +
                    "tipo_operacao o on o.codigo=e.tipo left join   " +
                    "tipo_operacao o2 on o2.codigo=t.tipo left join " +
                    "tipo_operacao o3 on o3.codigo=t2.tipo) e " +
            "where categoria <> 'Saque' and (credito > 0 or debito < 0 or debito_us < 0) and e.data between convert('{0}-{1}-{2} 00:00:00', datetime) and convert('{3}-{4}-{5} 23:59:59', datetime) and e.categoria='{6}' and e.desconsiderar=0 and username='{7}' order by e.data desc ",
 dtIni.Year, dtIni.Month, dtIni.Day, dtFim.Year, dtFim.Month, dtFim.Day, categoria, username));
            List<ExtratoVM> extratos = new List<ExtratoVM>();
            CultureInfo brasil = new CultureInfo("pt-BR");
            foreach (DataRow row in table.Rows)
            {
                extratos.Add(new ExtratoVM()
                {
                    Documento = row["documento"].ToString(),
                    Estabelecimento = row["Estabelecimento"].ToString(),
                    Operacao = row["Operacao"].ToString(),
                    Data = DateTime.Parse(row["Data"].ToString()),
                    Dia = DateTime.Parse(row["Data"].ToString()).Day.ToString(),
                    Mes = DateTime.Parse(row["Data"].ToString()).ToString("MMM", brasil).ToUpper(),
                    Ano = DateTime.Parse(row["Data"].ToString()).Year.ToString(),
                    Valor = double.Parse(row["Debito"].ToString(), format) + double.Parse(row["Credito"].ToString(), format) + (double.Parse(row["Debito_US"].ToString(), format) * double.Parse(row["Cotacao_Dolar"].ToString(), format)),
                    TipoOperacao = row["TIPOOPERACAO"].ToString(),
                    Categoria = row["categoria"].ToString(),
                    Procedencia = row["procedencia"].ToString()
                });
            }
            return extratos;
        }


        //       public List<ExtratoVM> RecebimentosCategoria(string categoria, DateTime dtIni, DateTime dtFim)
        //       {
        //           DataTable table = dataContext.Select(String.Format("SELECT * FROM (select E.DOCUMENTO, E.DATA,   " +
        //                   "IFNULL(CASE WHEN T2 .DESCRICAO = '' THEN T2 .NOME ELSE T2 .DESCRICAO END,E.OPERACAO) AS OPERACAO,   " +
        //                   "IFNULL (CASE WHEN T .DESCRICAO = '' THEN T .NOME ELSE T .DESCRICAO END,  E.ESTABELECIMENTO) AS ESTABELECIMENTO,  " +
        //                   "E.CREDITO, E.DEBITO, E.SOBRESCRITO,  " +
        //                    "IFNULL(E.TIPO,IFNULL(T.TIPO,T2.TIPO)) AS TIPO,  " +
        //                    "E.PROCEDENCIA,  " +
        //                   "IFNULL(O.DESCRICAO, IFNULL(O2.DESCRICAO,IFNULL(O3.DESCRICAO,'A Definir'))) AS TIPOOPERACAO,  " +
        //                   "IFNULL(O.CATEGORIA, IFNULL(O2.CATEGORIA,IFNULL(O3.CATEGORIA,'A Definir'))) AS CATEGORIA  ," +
        //                   "IFNULL(O.DESCONSIDERAR, IFNULL(O2.DESCONSIDERAR,IFNULL(O3.DESCONSIDERAR,0))) AS DESCONSIDERAR " +
        //                   "from (SELECT     DOCUMENTO, DATA, OPERACAO, ESTABELECIMENTO, CREDITO, DEBITO, SOBRESCRITO, TIPO_OPERACAO as TIPO, 'CD' as PROCEDENCIA  " +
        //                   "FROM         EXTRATO AS E  " +
        //                   "UNION  " +
        //                   "SELECT     CAST(DOCUMENTO as char(100)) as DOCUMENTO, DATA, OPERACAO, ESTABELECIMENTO, CREDITO, DEBITO, 0 AS Expr1, TIPO_OPERACAO as TIPO, 'D' as PROCEDENCIA  " +
        //                   "FROM         EXTRATO_DINHEIRO AS D) E LEFT JOIN   " +
        //                   "ESTABELECIMENTO T on T.NOME=E.ESTABELECIMENTO LEFT JOIN   " +
        //                   "ESTABELECIMENTO T2 on T2.NOME=E.OPERACAO LEFT JOIN   " +
        //                   "TIPO_OPERACAO O on O.CODIGO=E.TIPO LEFT JOIN   " +
        //                   "TIPO_OPERACAO O2 on O2.CODIGO=T.TIPO LEFT JOIN " +
        //                   "TIPO_OPERACAO O3 on O3.CODIGO=T2.TIPO) E " +
        //           "WHERE CREDITO > 0 AND e.data BETWEEN CONVERT(DATETIME, '{0}-{1}-{2} 00:00:00', 102) AND CONVERT(DATETIME, '{3}-{4}-{5} 23:59:59', 102) and E.CATEGORIA='{6}' AND E.DESCONSIDERAR=0 order by E.DATA desc ",
        //dtIni.Year, dtIni.Month, dtIni.Day, dtFim.Year, dtFim.Month, dtFim.Day, categoria));
        //           List<ExtratoVM> extratos = new List<ExtratoVM>();
        //           CultureInfo brasil = new CultureInfo("pt-BR");
        //           foreach (DataRow row in table.Rows)
        //           {
        //               extratos.Add(new ExtratoVM()
        //               {
        //                   Documento = row["documento"].ToString(),
        //                   Estabelecimento = row["Estabelecimento"].ToString(),
        //                   Operacao = row["Operacao"].ToString(),
        //                   Data = DateTime.Parse(row["Data"].ToString()),
        //                   Dia = DateTime.Parse(row["Data"].ToString()).Day.ToString(),
        //                   Mes = DateTime.Parse(row["Data"].ToString()).ToString("MMM", brasil).ToUpper(),
        //                   Ano = DateTime.Parse(row["Data"].ToString()).Year.ToString(),
        //                   Valor = double.Parse(row["Debito"].ToString()),
        //                   TipoOperacao = row["TIPOOPERACAO"].ToString(),
        //                   Categoria = row["categoria"].ToString(),
        //                   Procedencia = row["procedencia"].ToString()
        //               });
        //           }
        //           return extratos;
        //       }

        public List<ExtratoVM> ExtratoUnificado(string username)
        {
            DataTable table = dataContext.Select(String.Format("select * from (select e.username, e.documento, e.data,   " +
                    "ifnull(case when t2 .descricao = '' then t2 .nome else t2 .descricao end,e.operacao) as operacao,   " +
                    "ifnull (case when t .descricao = '' then t .nome else t .descricao end,  e.estabelecimento) as estabelecimento,  " +
                    "e.credito, e.debito, e.debito_us, e.cotacao_dolar, e.sobrescrito,  " +
                     "ifnull(e.tipo,ifnull(t.tipo,t2.tipo)) as tipo,  " +
                     "e.procedencia,  " +
                    "ifnull(o.descricao, ifnull(o2.descricao,ifnull(o3.descricao,'A Definir'))) as tipooperacao,  " +
                    "ifnull(o.categoria, ifnull(o2.categoria,ifnull(o3.categoria,'A Definir'))) as categoria  ," +
                    "ifnull(o.frequencia, ifnull(o2.frequencia,ifnull(o3.frequencia,'N/A'))) as frequencia  ,	" +
                    "ifnull(o.desconsiderar, ifnull(o2.desconsiderar,ifnull(o3.desconsiderar,0))) as desconsiderar " +
                    "from (select     username,documento, data, operacao, estabelecimento, credito, debito, 0 as debito_us, 0 as cotacao_dolar, sobrescrito, tipo_operacao as tipo, 'CD' as procedencia  " +
                    "from         extrato as e  " +
                    "union  " +
                    "select    username, documento, data, operacao, estabelecimento, 0 as credito, debito, debito_us,cotacao_dolar, 0 as expr1, tipo_operacao as tipo, 'CC' as procedencia  " +
                    "from         extrato_credito as c  " +
                    "union  " +
                    "select    username, cast(documento as char(100)) as documento, data, operacao, estabelecimento, credito, debito, 0 as debito_us, 0 as cotacao_dolar, 0 as expr1, tipo_operacao as tipo, 'D' as procedencia  " +
                    "from         extrato_dinheiro as d) e left join   " +
                    "estabelecimento t on t.nome=e.estabelecimento left join   " +
                    "estabelecimento t2 on t2.nome=e.operacao left join   " +
                    "tipo_operacao o on o.codigo=e.tipo left join   " +
                    "tipo_operacao o2 on o2.codigo=t.tipo left join " +
                    "tipo_operacao o3 on o3.codigo=t2.tipo) e " +
            "where username='{0}' order by e.data desc ", username));
            //"where categoria <> 'Saque' and e.desconsiderar=0 and username='{0}' order by e.data desc ",username));
            List<ExtratoVM> extratos = new List<ExtratoVM>();
            CultureInfo brasil = new CultureInfo("pt-BR");
            foreach (DataRow row in table.Rows)
            {
                extratos.Add(new ExtratoVM()
                {
                    Documento = row["documento"].ToString(),
                    Estabelecimento = row["Estabelecimento"].ToString(),
                    Operacao = row["Operacao"].ToString(),
                    Data = DateTime.Parse(row["Data"].ToString()),
                    Dia = DateTime.Parse(row["Data"].ToString()).Day.ToString(),
                    Mes = DateTime.Parse(row["Data"].ToString()).ToString("MMM", brasil).ToUpper(),
                    Ano = DateTime.Parse(row["Data"].ToString()).Year.ToString(),
                    Valor = double.Parse(row["Debito"].ToString(), format) + double.Parse(row["Credito"].ToString(), format) + (double.Parse(row["Debito_US"].ToString(), format) * double.Parse(row["Cotacao_Dolar"].ToString(), format)), //Gambiarra, pois sempre um deles é zero, logo a soma diz se será crédito ou débito
                    TipoOperacao = row["TIPOOPERACAO"].ToString(),
                    Categoria = row["categoria"].ToString(),
                    Procedencia = row["procedencia"].ToString(),
                    Frequencia = row["frequencia"].ToString()
                });
            }
            return extratos;
        }


        public double CreditoCategorizadoTotal(string username)
        {
            return double.Parse(dataContext.ExecuteScalar(string.Format("select     ifnull(sum(credito),0) + ifnull(sum(debito),0) as expr1 " +
            "from (select e.username, credito, debito, ifnull(o.desconsiderar, ifnull(o2.desconsiderar,ifnull(o3.desconsiderar,0))) as desconsiderar, " +
            "ifnull(o.categoria, ifnull(o2.categoria,ifnull(o3.categoria,'A Definir'))) as categoria from extrato e left join   " +
            "estabelecimento t on t.nome=e.estabelecimento left join   " +
            "estabelecimento t2 on t2.nome=e.operacao left join   " +
            "tipo_operacao o on o.codigo=e.tipo_operacao left join   " +
            "tipo_operacao o2 on o2.codigo=t.tipo left join " +
            "tipo_operacao o3 on o3.codigo=t2.tipo) e where categoria = 'Cartão de Crédito Categorizado' and username='{0}'", username)).ToString(), format); ;
        }

        public double CartaoCreditoTotal(string username)
        {
            return double.Parse(dataContext.ExecuteScalar(string.Format("select     ifnull(sum(debito),0) + ifnull(sum(debito_us * cotacao_dolar),0) as expr1 " +
                "from (select e.username, debito, debito_us, cotacao_dolar, ifnull(o.desconsiderar, ifnull(o2.desconsiderar,ifnull(o3.desconsiderar,0))) as desconsiderar from extrato_credito e left join   " +
                    "estabelecimento t on t.nome=e.estabelecimento left join   " +
                    "estabelecimento t2 on t2.nome=e.operacao left join   " +
                    "tipo_operacao o on o.codigo=e.tipo_operacao left join   " +
                    "tipo_operacao o2 on o2.codigo=t.tipo left join " +
                    "tipo_operacao o3 on o3.codigo=t2.tipo) e where e.desconsiderar = 0 and username='{0}'", username)).ToString(), format);
        }

        public double SaldoTotal(string username)
        {
            return double.Parse(dataContext.ExecuteScalar(string.Format("select     ifnull(sum(credito),0) + ifnull(sum(debito),0) as expr1 " +
                "from (select e.username, credito, debito, ifnull(o.desconsiderar, ifnull(o2.desconsiderar,ifnull(o3.desconsiderar,0))) as desconsiderar from extrato e left join   " +
                    "estabelecimento t on t.nome=e.estabelecimento left join   " +
                    "estabelecimento t2 on t2.nome=e.operacao left join   " +
                    "tipo_operacao o on o.codigo=e.tipo_operacao left join   " +
                    "tipo_operacao o2 on o2.codigo=t.tipo left join " +
                    "tipo_operacao o3 on o3.codigo=t2.tipo) e where e.desconsiderar = 0 and username='{0}'", username)).ToString(), format) + double.Parse(dataContext.ExecuteScalar(string.Format("SELECT     IFNULL(SUM(saldo),0) as Expr1 " +
                "from valores_iniciais where username='{0}'", username)).ToString(), format);
        }


        public double DinheiroTotal(string username)
        {

            return double.Parse(dataContext.ExecuteScalar(string.Format("select     ifnull(sum(credito),0) + ifnull(sum(debito),0) as expr1 " +
                "from extrato_dinheiro where username='{0}'", username)).ToString(), format) + double.Parse(dataContext.ExecuteScalar(string.Format("select ifnull(sum(dinheiro),0) as expr1 " +
                "from valores_iniciais where username='{0}'", username)).ToString(), format) + double.Parse(dataContext.ExecuteScalar(string.Format("select ifnull (sum(debito * -1),0) as expr1 from (select e.username, e.documento, e.data,   " +
                    "ifnull(case when t2 .descricao = '' then t2 .nome else t2 .descricao end,e.operacao) as operacao,   " +
                    "ifnull (case when t .descricao = '' then t .nome else t .descricao end,  e.estabelecimento) as estabelecimento,  " +
                    "e.credito, e.debito, e.sobrescrito,  " +
                     "ifnull(e.tipo,ifnull(t.tipo,t2.tipo)) as tipo,  " +
                     "e.procedencia,  " +
                    "ifnull(o.descricao, ifnull(o2.descricao,ifnull(o3.descricao,'A Definir'))) as tipooperacao,  " +
                    "ifnull(o.categoria, ifnull(o2.categoria,ifnull(o3.categoria,'A Definir'))) as categoria  ," +
                    "ifnull(o.desconsiderar, ifnull(o2.desconsiderar,ifnull(o3.desconsiderar,0))) as desconsiderar " +
                    "from (select    username, documento, data, operacao, estabelecimento, credito, debito, sobrescrito, tipo_operacao as tipo, 'CD' as procedencia  " +
                    "from         extrato as e  " +
                    "union  " +
                    "select    username, cast(documento as char(100)) as documento, data, operacao, estabelecimento, credito, debito, 0 as expr1, tipo_operacao as tipo, 'D' as procedencia  " +
                    "from         extrato_dinheiro as d) e left join   " +
                    "estabelecimento t on t.nome=e.estabelecimento left join   " +
                    "estabelecimento t2 on t2.nome=e.operacao left join   " +
                    "tipo_operacao o on o.codigo=e.tipo left join   " +
                    "tipo_operacao o2 on o2.codigo=t.tipo left join " +
                    "tipo_operacao o3 on o3.codigo=t2.tipo) e " +
            "where categoria = 'Saque' and username='{0}' and e.desconsiderar=0", username)).ToString(), format);
        }

        public List<ExtratoVM> ExtratoLoteCarga(string tipo, string data, string username)
        {
            DataTable table = dataContext.Select(String.Format("select *from {0} where lote_carga= convert('{1}', datetime) and username='{2}' order by data desc",
                tipo.ToUpper() == "CC" ? "extrato_credito" : "extrato", data, username));
            List<ExtratoVM> extratos = new List<ExtratoVM>();
            CultureInfo brasil = new CultureInfo("pt-BR");
            foreach (DataRow row in table.Rows)
            {
                extratos.Add(new ExtratoVM()
                {
                    Documento = row["documento"].ToString(),
                    Estabelecimento = row["Estabelecimento"].ToString(),
                    Operacao = row["Operacao"].ToString(),
                    Valor = double.Parse(row["Debito"].ToString(), format) + (tipo.ToUpper() == "CD" ? double.Parse(row["Credito"].ToString(), format) : 0) + (tipo.ToUpper() == "CC" ? double.Parse(row["Debito_US"].ToString(), format) : 0),
                    Procedencia = tipo.ToUpper(),
                    Data = DateTime.Parse(row["Data"].ToString())
                });
            }
            return extratos;
        }

        public List<object> ReportProjecao(string username, int dias, List<FrequenciaVM> frequencias)
        {
            List<object> o = new List<object>();
            double saldoAtual = SaldoTotal(username) + CreditoCategorizadoTotal(username) + DinheiroTotal(username);
            ProjetorEstatistico p = new ProjetorEstatistico(ExtratoUnificado(username), frequencias, CartaoCreditoTotal(username) - CreditoCategorizadoTotal(username));
            p.CalcularProjecao(dias);

            List<string> categorias = new List<string>();
            categorias.Add("Dia");
            categorias.Add("Saldo");
            foreach (var item in p.projecao.Select(x => x.Valores))
            {
                foreach (var cat in item.Select(x => x.Categoria).Distinct())
                {
                    if (!categorias.Contains(cat))
                    {
                        categorias.Add(cat);
                    }
                }
            }

            o.Add(categorias.ToArray());
            foreach (var item in p.projecao)
            {
                Queue<object> valores = new Queue<object>();
                foreach (var c in categorias)
                {
                    if (c == "Dia")
                    {
                        valores.Enqueue(item.Date.Month.ToString() + "/" + item.Date.Day.ToString());
                    }
                    else if (c == "Saldo")
                    {
                        saldoAtual += item.Valores.Sum(x => x.Valor);
                        valores.Enqueue(saldoAtual);
                    }
                    else
                    {
                        double totCat = 0;
                        foreach (var d in item.Valores.Where(x => x.Categoria == c))
                        {
                            totCat += d.Valor;
                        }
                        valores.Enqueue(totCat);
                    }
                }
                o.Add(valores.ToArray());
            }
            return o;
        }


        //QUERIES EXCLUSIVAS DO MOBILE!===========================================================
        public List<ResumoCategoria> ResumoDespesasCategoria(DateTime dtIni, DateTime dtFim, string username)
        {
            List<ResumoCategoria> result = new List<ResumoCategoria>();
            DataTable table = dataContext.Select(String.Format("select categoria, sum(debito * -1) + sum(debito_us * -1 * cotacao_dolar) as valor from( " +
                    "select e.username, e.documento, e.data,  " +
                    "ifnull(case when t2 .descricao = '' then t2 .nome else t2 .descricao end,e.operacao) as operacao,  " +
                    "ifnull (case when t .descricao = '' then t .nome else t .descricao end,  e.estabelecimento) as estabelecimento, " +
                     "e.credito, e.debito, e.debito_us, e.cotacao_dolar, e.sobrescrito,  " +
                     "ifnull(e.tipo,ifnull(t.tipo,t2.tipo)) as tipo, " +
                     "e.procedencia, " +
                    "ifnull(o.descricao, ifnull(o2.descricao,ifnull(o3.descricao,'A Definir'))) as tipooperacao, " +
                    "ifnull(o.categoria, ifnull(o2.categoria,ifnull(o3.categoria,'A Definir'))) as categoria ," +
                    "ifnull(o.desconsiderar, ifnull(o2.desconsiderar,ifnull(o3.desconsiderar,0))) as desconsiderar " +
                    "from (select     username,documento, data, operacao, estabelecimento, credito, debito, 0 as debito_us, 0 as cotacao_dolar, sobrescrito, tipo_operacao as tipo, 'CD' as procedencia  " +
                    "from         extrato as e  " +
                    "union  " +
                    "select    username, documento, data, operacao, estabelecimento, 0 as credito, debito, debito_us,cotacao_dolar, 0 as expr1, tipo_operacao as tipo, 'CC' as procedencia  " +
                    "from         extrato_credito as c  " +
                    "union  " +
                    "select     username, cast(documento as char(100)) as documento, data, operacao, estabelecimento, credito, debito, 0 as debito_us, 0 as cotacao_dolar, 0 as expr1, tipo_operacao as tipo, 'D' as procedencia  " +
                    "from         extrato_dinheiro as d) e left join   " +
                    "estabelecimento t on t.nome=e.estabelecimento left join  " +
                    "estabelecimento t2 on t2.nome=e.operacao left join  " +
                    "tipo_operacao o on o.codigo=e.tipo left join  " +
                    "tipo_operacao o2 on o2.codigo=t.tipo left join  " +
                    "tipo_operacao o3 on o3.codigo=t2.tipo) e " +
                    "where categoria <> 'Saque' and  (debito < 0 or debito_us < 0) and  e.data between convert('{0}-{1}-{2} 00:00:00', datetime) and convert('{3}-{4}-{5} 23:59:59', datetime) and e.desconsiderar=0 and username='{6}' " +
                    "GROUP BY categoria", dtIni.Year, dtIni.Month, dtIni.Day, dtFim.Year, dtFim.Month, dtFim.Day, username));

            foreach (DataRow row in table.Rows)
            {
                result.Add(new ResumoCategoria()
                {
                    Categoria = row[0].ToString(),
                    Valor = double.Parse(row[1].ToString(), format),
                    Detalhes = ResumoDespesasDetalhesCategorias(dtIni, dtFim, row[0].ToString(), username)
                });
                
            }

            return result;
        }

        public List<ResumoTipo> ResumoDespesasDetalhesCategorias(DateTime dtIni, DateTime dtFim, string categoria, string username)
        {
            List<ResumoTipo> result = new List<ResumoTipo>();
            DataTable table = dataContext.Select(String.Format("select tipooperacao, sum(debito * -1) + sum(debito_us * -1 * cotacao_dolar) as valor from( " +
                    "select e.username, e.documento, e.data,  " +
                    "ifnull(case when t2 .descricao = '' then t2 .nome else t2 .descricao end,e.operacao) as operacao,  " +
                    "ifnull (case when t .descricao = '' then t .nome else t .descricao end,  e.estabelecimento) as estabelecimento, " +
                     "e.credito, e.debito, e.debito_us, e.cotacao_dolar, e.sobrescrito,  " +
                     "ifnull(e.tipo,ifnull(t.tipo,t2.tipo)) as tipo, " +
                     "e.procedencia, " +
                    "ifnull(o.descricao, ifnull(o2.descricao,ifnull(o3.descricao,'A Definir'))) as tipooperacao, " +
                    "ifnull(o.categoria, ifnull(o2.categoria,ifnull(o3.categoria,'A Definir'))) as categoria, " +
                    "ifnull(o.desconsiderar, ifnull(o2.desconsiderar,ifnull(o3.desconsiderar,0))) as desconsiderar " +
                    "from (select     username,documento, data, operacao, estabelecimento, credito, debito, 0 as debito_us, 0 as cotacao_dolar, sobrescrito, tipo_operacao as tipo, 'CD' as procedencia  " +
                    "from         extrato as e  " +
                    "union  " +
                    "select    username, documento, data, operacao, estabelecimento, 0 as credito, debito, debito_us,cotacao_dolar, 0 as expr1, tipo_operacao as tipo, 'CC' as procedencia  " +
                    "from         extrato_credito as c  " +
                    "union  " +
                    "select    username, cast(documento as char(100)) as documento, data, operacao, estabelecimento, credito, debito, 0 as debito_us, 0 as cotacao_dolar, 0 as expr1, tipo_operacao as tipo, 'D' as procedencia  " +
                    "from         extrato_dinheiro as d) e left join   " +
                    "estabelecimento t on t.nome=e.estabelecimento left join  " +
                    "estabelecimento t2 on t2.nome=e.operacao left join  " +
                    "tipo_operacao o on o.codigo=e.tipo left join  " +
                    "tipo_operacao o2 on o2.codigo=t.tipo left join  " +
                    "tipo_operacao o3 on o3.codigo=t2.tipo) e " +
                    "where categoria <> 'Saque' and  (debito < 0 or debito_us < 0) and  e.data between convert('{0}-{1}-{2} 00:00:00', datetime) and convert('{3}-{4}-{5} 23:59:59', datetime) and categoria='{6}' and e.desconsiderar=0 and username='{7}' " +
                    "group by tipooperacao", dtIni.Year, dtIni.Month, dtIni.Day, dtFim.Year, dtFim.Month, dtFim.Day, categoria, username));
            foreach (DataRow row in table.Rows)
            {
                result.Add(new ResumoTipo()
                {
                    Descricao = row[0].ToString(),
                    Valor = double.Parse(row[1].ToString(), format)
                });                
            }
            return result;
        }

        public List<ProjectedDate> WebApiReportProjecao(string username, int dias, List<FrequenciaVM> frequencias)
        {
            ProjetorEstatistico p = new ProjetorEstatistico(ExtratoUnificado(username), frequencias, CartaoCreditoTotal(username) - CreditoCategorizadoTotal(username));
            p.CalcularProjecao(dias);
            double saldoAtual = SaldoTotal(username) + CreditoCategorizadoTotal(username) + DinheiroTotal(username);
            foreach (var item in p.projecao)
            {
                saldoAtual += item.Valores.Sum(x => x.Valor);
                item.Saldo = Math.Round(saldoAtual,2);
            }
            return p.projecao;
        }
    }

    //*****************************************qry extrato master***************************************
    //    select E.DOCUMENTO, E.DATA, 
    //IFNULL(CASE WHEN T2 .DESCRICAO = '' THEN T2 .NOME ELSE T2 .DESCRICAO END,E.OPERACAO), 
    //IFNULL (CASE WHEN T .DESCRICAO = '' THEN T .NOME ELSE T .DESCRICAO END,  E.ESTABELECIMENTO),
    // E.CREDITO, E.DEBITO, E.SOBRESCRITO,
    // IFNULL(E.TIPO,IFNULL(T.TIPO,T2.TIPO)) AS TIPO,
    // E.PROCEDENCIA,
    //IFNULL(O.DESCRICAO, IFNULL(O2.DESCRICAO,O3.DESCRICAO)) AS TIPOOPERACAO,
    //IFNULL(O.CATEGORIA, IFNULL(O2.CATEGORIA,O3.CATEGORIA)) AS CATEGORIA
    //from (SELECT     DOCUMENTO, DATA, OPERACAO, ESTABELECIMENTO, CREDITO, DEBITO, SOBRESCRITO, TIPO_OPERACAO as TIPO, 'CD' as PROCEDENCIA
    //FROM         EXTRATO AS E
    //UNION
    //SELECT     DOCUMENTO, DATA, OPERACAO, ESTABELECIMENTO, CREDITO, DEBITO, 0 AS Expr1, TIPO_OPERACAO as TIPO, 'D' as PROCEDENCIA
    //FROM         EXTRATO_DINHEIRO AS D) E LEFT JOIN 
    //ESTABELECIMENTO T on T.NOME=E.ESTABELECIMENTO LEFT JOIN 
    //ESTABELECIMENTO T2 on T2.NOME=E.OPERACAO LEFT JOIN 
    //TIPO_OPERACAO O on O.CODIGO=E.TIPO LEFT JOIN 
    //TIPO_OPERACAO O2 on O2.CODIGO=T.TIPO LEFT JOIN 
    //TIPO_OPERACAO O3 on O3.CODIGO=T2.TIPO
    struct JoinHelper
    {
        public string mes;
        public double debito;
        public double credito;
        public double saldo;
    }
}