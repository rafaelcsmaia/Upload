using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Extratistico.Classes.Helpers;

namespace Extratistico.Areas.Extratos.Models.Entidades
{
    public class Bradesco : IUploadable
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
            foreach (var item in Data)
            {
                try
                {

                    string[] split = item.Split(';');

                    if (split.Length >= 6)
                    {
                        split[3] = split[3].Replace((char)34, ' ');//.Replace(".", "").Replace(",", ".").Trim();
                        split[4] = split[4].Replace((char)34, ' ');//.Replace(".", "").Replace(",", ".").Trim();

                        //Aqui é uma linha de extrato válida!
                        if (e == null)
                        {
                            //Nova linha de extrato!
                            e = new Extrato()
                            {
                                Data = new DateTime(int.Parse("20" + split[0].Split('/')[2]), int.Parse(split[0].Split('/')[1]), int.Parse(split[0].Split('/')[0]), 0, 0, 0),
                                Operacao = split[1].Trim(),
                                Documento = split[2].Trim(),
                                Credito = double.Parse(split[3] == string.Empty ? "0" : split[3],format),
                                Debito = double.Parse(split[4] == string.Empty ? "0" : split[4], format),
                                Username = username
                            };
                        }
                        else
                        {
                            //Nova linha de extrato, Não houve descrição na antiga, logo esta ficará pendente
                            e.Documento = HashHelper.ComputeHash(e.Documento + e.Data.ToShortDateString() + e.Credito.ToString() + e.Debito.ToString());
                            extratos.Add(e);
                            e = new Extrato()
                            {
                                Data = new DateTime(int.Parse("20" + split[0].Split('/')[2]), int.Parse(split[0].Split('/')[1]), int.Parse(split[0].Split('/')[0]), 0, 0, 0),
                                Operacao = split[1].Trim(),
                                Documento = split[2].Trim(),
                                Credito = double.Parse(split[3] == string.Empty ? "0" : split[3], format),
                                Debito = double.Parse(split[4] == string.Empty ? "0" : split[4], format),
                                Username = username 
                            };
                        }

                    }
                    else
                    {
                        if (split.Length > 1)
                        {
                            if (split[1] != string.Empty && split[1] != "Total" && e != null)
                            {
                                //Aqui linha de descrição válida!
                                e.Estabelecimento = split[1];
                                e.Documento = HashHelper.ComputeHash(e.Documento + e.Data.ToShortDateString() + e.Credito.ToString() + e.Debito.ToString());
                                extratos.Add(e);
                                e = null;
                            }
                            else
                            {
                                if (e != null)
                                {
                                    e.Documento = HashHelper.ComputeHash(e.Documento + e.Data.ToShortDateString() + e.Credito.ToString() + e.Debito.ToString());
                                    extratos.Add(e);
                                    e = null;
                                }
                            }
                        }
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
            return string.Format("insert into extrato(DOCUMENTO,DATA,OPERACAO,ESTABELECIMENTO,CREDITO,DEBITO, SOBRESCRITO, USERNAME) values('{0}',STR_TO_DATE('{1}','%d/%m/%Y'), '{2}', '{3}',{4},{5},{6}, '{7}')", e.Documento, e.Data.ToString("dd/MM/yyyy"), e.Operacao, e.Estabelecimento, e.Credito.ToString().Replace(",", "."), e.Debito.ToString().Replace(",", "."), e.Sobrescrito ? 1 : 0, e.Username);
        }
    }
}