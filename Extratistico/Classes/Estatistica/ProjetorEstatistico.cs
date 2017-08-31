using Extratistico.Areas.Cadastros.Models.Entidades;
using Extratistico.Areas.Extratos.Models.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Extratistico.Classes.Estatistica
{
    public class ProjetorEstatistico
    {
        private List<ExtratoVM> _extratoVM;
        private List<FrequenciaVM> _frequenciaVM;
        private Descriptive statistics;
        public List<ProjectedDate> projecao = new List<ProjectedDate>();
        private double _saldoCredito;

        public ProjetorEstatistico(List<ExtratoVM> extratoVM, List<FrequenciaVM> frequenciaVM, double saldoCredito = 0)
        {
            _extratoVM = extratoVM;
            _frequenciaVM = frequenciaVM;
            _saldoCredito = saldoCredito;

        }
        //Criar o frequencia VM que puxa da tabela, ai começa a festa!
        public void CalcularProjecao(int dias)
        {
            for (int i = 1; i <= dias; i++)
            {
                projecao.Add(new ProjectedDate() { Date = _extratoVM.Select(x=>x.Data).Max().AddDays(i), Valores = new List<ProjectedCategory>() });
            }

            List<double> valores = new List<double>();
            List<double> intervaloDias = new List<double>();

            //Pegando os gastos de frequencia CONTINUA!
            var comparer = _extratoVM.Select(y => y.Data).Max().AddDays(-60);
            var dados = _extratoVM.Where(x => x.Data.CompareTo(comparer) > 0 && x.Frequencia == "CONTINUO");
            foreach (var categoria in dados.Select(x => x.Categoria).Distinct())
            {
                foreach (var tipo in dados.Where(x => x.Categoria == categoria).Select(x => x.TipoOperacao).Distinct())
                {
                    foreach (var dia in dados.Where(x=> x.Categoria == categoria && x.TipoOperacao == tipo).Select(x => x.Data).Distinct())
                    {
                        valores.Add(dados.Where(x => x.Categoria == categoria && x.TipoOperacao == tipo && x.Data == dia).Sum(x => x.Valor));
                    }

                    double valorReal;
                    if (valores.Count() > 1)
                    {
                        statistics = new Descriptive(valores.ToArray());
                        statistics.Analyze();
                        var q3 = statistics.Result.ThirdQuartile;
                        valorReal = q3 * (valores.Count() / (double)60);
                    }
                    else
                    {
                        valorReal = (valores.FirstOrDefault() / (double)60);
                    }

                    valores.Clear();
                    foreach (var data in projecao)
                    {
                        data.Valores.Add(new ProjectedCategory() { Categoria = categoria, Tipo = tipo, Valor = valorReal });
                    }
                }
            }

            //Pegando os gastos de frequencia PERIODICA!
            DateTime? lastDate = null;
            comparer = _extratoVM.Select(y => y.Data).Max().AddMonths(-6);
            dados = _extratoVM.Where(x => x.Data.CompareTo(comparer) > 0 && x.Frequencia == "PERIODICO");
            foreach (var categoria in dados.Select(x => x.Categoria).Distinct())
            {
                foreach (var tipo in dados.Where(x => x.Categoria == categoria).Select(x => x.TipoOperacao).Distinct())
                {
                    foreach (var extrato in dados.Where(x => x.Categoria == categoria && x.TipoOperacao == tipo).OrderBy(x => x.Data))
                    {
                        if (lastDate == null)
                        {
                            lastDate = extrato.Data;
                        }
                        else
                        {
                            intervaloDias.Add((extrato.Data - lastDate.Value).Days);
                            lastDate = extrato.Data;
                        }
                        valores.Add(extrato.Valor);
                    }

                    double valorReal;
                    if (valores.Count() > 2)
                    {
                        statistics = new Descriptive(valores.ToArray());
                        statistics.Analyze();
                        valorReal = statistics.Result.ThirdQuartile;
                    }
                    else
                    {
                        continue;
                    }

                    //Calcula o intervalo entre dois gastos
                    double intervalo;
                    if (intervaloDias.Count() > 1)
                    {
                        statistics = new Descriptive(intervaloDias.ToArray());
                        statistics.Analyze();
                        intervalo = statistics.Result.ThirdQuartile;
                        intervaloDias.Clear();
                    }
                    else
                    {
                        intervalo = intervaloDias.FirstOrDefault();
                    }

                    valores.Clear();


                    int multiplicador = 1;
                    foreach (var data in projecao)
                    {
                        if ((data.Date - lastDate.Value.AddDays(intervalo * multiplicador)).Days == 0)
                        {
                            multiplicador += 1;
                            data.Valores.Add(new ProjectedCategory() { Categoria = categoria, Tipo = tipo, Valor = valorReal });
                        }
                    }
                }
            }

            //Pegando os gastos de frequencia MENSAL!
            comparer = _extratoVM.Select(y => y.Data).Max().AddMonths(-6);
            dados = _extratoVM.Where(x => x.Data.CompareTo(comparer) > 0 && x.Frequencia == "MENSAL");
            foreach (var categoria in dados.Select(x => x.Categoria).Distinct())
            {
                foreach (var tipo in dados.Where(x => x.Categoria == categoria).Select(x => x.TipoOperacao).Distinct())
                {
                    foreach (var mes in dados.Where(x => x.Categoria == categoria && x.TipoOperacao == tipo).Select(x => x.Mes).Distinct())
                    {
                        double totMes = 0;
                        int qtdMes = 0;
                        foreach (var extrato in dados.Where(x => x.Categoria == categoria && x.TipoOperacao == tipo && x.Mes == mes).OrderBy(x => x.Data))
                        {
                            if (qtdMes < _frequenciaVM.Where(x => x.TipoDesc == tipo).Count())
                            {
                                qtdMes++;
                                totMes += extrato.Valor;
                            }
                        }
                        if (qtdMes == _frequenciaVM.Where(x => x.TipoDesc == tipo).Count())
                        {
                            valores.Add(totMes);
                        }                        
                    }

                    double valorReal;
                    if (valores.Count() > 1)
                    {
                        statistics = new Descriptive(valores.ToArray());
                        statistics.Analyze();
                        var q3 = statistics.Result.ThirdQuartile;
                        valorReal = statistics.Result.ThirdQuartile;
                    }
                    else
                    {
                        valorReal = valores.FirstOrDefault();
                    }

                    valores.Clear();

                    foreach (var f in _frequenciaVM.Where(x => x.TipoDesc == tipo))
                    {
                        foreach (var data in projecao.Where(x => x.Date.Day.ToString() == f.Dia || (f.Dia == "U" && x.Date.Day == DateTime.DaysInMonth(x.Date.Year, x.Date.Month))))
                        {
                            data.Valores.Add(new ProjectedCategory() { Categoria = categoria, Tipo = tipo, Valor = valorReal * (f.Percentual / 100) });
                        }
                    }
                }
            }

            //Pegando os gastos de frequencia ANUAL!
            foreach (var categoria in _extratoVM.Where(x => x.Frequencia == "ANUAL").Select(x => x.Categoria).Distinct())
            {
                foreach (var tipo in _extratoVM.Where(x => x.Frequencia == "ANUAL" && x.Categoria == categoria).Select(x => x.TipoOperacao).Distinct())
                {
                    foreach (var ano in _extratoVM.Where(x => x.Frequencia == "ANUAL" && x.Categoria == categoria && x.TipoOperacao == tipo).Select(x => x.Ano).Distinct())
                    {
                        valores.Add(_extratoVM.Where(x => x.Frequencia == "ANUAL" && x.Categoria == categoria && x.TipoOperacao == tipo && x.Ano == ano).Sum(x => x.Valor));
                    }

                    double valorReal;
                    if (valores.Count() > 1)
                    {
                        statistics = new Descriptive(valores.ToArray());
                        statistics.Analyze();
                        var q3 = statistics.Result.ThirdQuartile;
                        valorReal = statistics.Result.ThirdQuartile;
                    }
                    else
                    {
                        valorReal = valores.FirstOrDefault();
                    }

                    valores.Clear();

                    foreach (var f in _frequenciaVM.Where(x => x.TipoDesc == tipo))
                    {
                        foreach (var data in projecao.Where(x => (x.Date.Day.ToString() == f.Dia && x.Date.Month.ToString() == f.Mes) || (f.Dia == "U" && x.Date.Day == DateTime.DaysInMonth(x.Date.Year, x.Date.Month) && x.Date.Month.ToString() == f.Mes)))
                        {
                            data.Valores.Add(new ProjectedCategory() { Categoria = categoria, Tipo = tipo, Valor = valorReal * (f.Percentual / 100) });
                        }
                    }
                }
            }

            //Pegando os gastos no cartao de CREDITO!
            foreach (var f in _frequenciaVM.Where(x => x.Frequencia == "CREDITO"))
            {
                foreach (var data in projecao.Where(x => x.Date.Day.ToString() == f.Dia || (f.Dia == "U" && x.Date.Day == DateTime.DaysInMonth(x.Date.Year, x.Date.Month))))
                {
                    data.Valores.Add(new ProjectedCategory() { Categoria = "Cartão de Crédito", Tipo = "Cartão de Crédito", Valor = _saldoCredito });
                }
            }
        }
    }
}