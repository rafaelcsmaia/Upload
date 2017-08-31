using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Extratistico.Classes.Helpers;

namespace Extratistico.Areas.Extratos.Models.Entidades
{
    public class CreditoBradesco : IUploadable
    {
        public override void PreRoutineExecution(string procedureSQL)
        {
            //throw new NotImplementedException();
        }

        public override void PostRoutineExecution(string procedureSQL)
        {
            //throw new NotImplementedException();
        }


        /// <summary>
        /// DATA;OPERACAO ou ESTABELECIMENTO;DOCUMENTO;CREDITO;DEBITO
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public override List<Extrato> ProcessData(string username)
        {
            List<Extrato> extratos = new List<Extrato>();
            Extrato e = null;
            double dolar = 0;
            string numeroCartao = Data[5].Split(';')[3];
            
            foreach (var item in Data)
            {
                try
                {
                    string[] split = item.Split(';');
                    if (split[0] == "Cota��o do d�lar utilizada:")
                    {
                        dolar = double.Parse(split[1], format);
                    }
                    if (split.Length >= 6 && (!split[1].ToString().ToUpper().Contains("SALDO ANTERIOR")) && (split[2].ToString() != "0" || split[3].ToString() != "0"))
                    {                        
                        split[2] = split[2].Replace((char)34, ' ');//.Replace(".", "").Replace(",", ".").Trim();
                        split[3] = split[3].Replace((char)34, ' ');//.Replace(".", "").Replace(",", ".").Trim();

                            e = new Extrato()
                            {
                                Data = new DateTime(int.Parse(split[0].Split('/')[2]), int.Parse(split[0].Split('/')[1]), int.Parse(split[0].Split('/')[0]), 0, 0, 0),
                                Operacao = split[1].Trim(),
                                Estabelecimento = split[1].Trim(),
                                Debito = double.Parse(split[3] == string.Empty ? "0" : split[3], format),
                                DebitoUS = double.Parse(split[2] == string.Empty ? "0" : split[2], format),
                                NumeroCartao=numeroCartao,
                                Username=username
                            };
                            extratos.Add(e);                    
                    }

                }
                catch (Exception ex)
                {
                    log.TotalRecords += 1;
                    log.Log.Add(new LogData()
                    {
                        Data = item.ToString(),
                        ErrorMessage = ex.Message,
                        rowNumber = log.TotalRecords
                    });                    
                    log.FailureRecords += 1;
                }
            }

                        //Verificar se ocorre uma duyplicata de hash de código, caso haja, é porque aconteceu de ter mesmo codigo, data e valor (acontece com duas recargas telefonicas no mesmo dia)
            //Para tal, é adicionado '-numero' na frente de cada duplicata, permitindo que a mesma entre no sistema
            string documento = string.Empty;
            int count = 1;
            foreach (var item in extratos.OrderBy(x=>x.Documento))
            {
                item.CotacaoDolar = dolar;
                item.Documento = HashHelper.ComputeHash(item.Estabelecimento + item.Data.ToShortDateString() + item.Debito.ToString() + item.DebitoUS.ToString() + item.NumeroCartao.ToString());
                if (documento == string.Empty)
                {
                    documento = item.Documento;
                    continue;
                }

                if (documento == item.Documento)
                {
                    item.Documento = item.Documento + "-" + count.ToString();
                }
                else
                {
                    documento = item.Documento;
                    count = 1;
                }
            }

            return extratos;
        }

        public override string ExtratoToSQL(Extrato e)
        {
            return string.Format("insert into extrato_credito(DOCUMENTO,DATA,OPERACAO,ESTABELECIMENTO,DEBITO, DEBITO_US, NUMERO_CARTAO, COTACAO_DOLAR, USERNAME) values('{0}',STR_TO_DATE('{1}','%d/%m/%Y'), '{2}', '{3}',-{4},-{5},'{6}',{7},'{8}')", e.Documento, e.Data.ToString("dd/MM/yyyy"), e.Operacao, e.Estabelecimento, e.Debito.ToString().Replace(",", "."), e.DebitoUS.ToString().Replace(",", "."), e.NumeroCartao, e.CotacaoDolar.ToString().Replace(",", "."), e.Username);
        }
    }
}