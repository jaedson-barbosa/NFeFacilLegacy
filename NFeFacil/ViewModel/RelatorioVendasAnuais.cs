using BibliotecaCentral;
using BibliotecaCentral.ModeloXML;
using BibliotecaCentral.ModeloXML.PartesProcesso;
using BibliotecaCentral.Repositorio;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace NFeFacil.ViewModel
{
    public sealed class RelatorioVendasAnuais : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<int> AnosDisponiveis
        {
            get
            {
                using (var db = new NotasFiscais())
                {
                    var anos = from dado in db.Registro
                               let ano = Convert.ToDateTime(dado.DataEmissao).Year
                               orderby ano ascending
                               select ano;
                    return anos.Distinct().GerarObs();
                }
            }
        }

        private List<NFe> notas;

        private int anoEscolhido;
        public int AnoEscolhido
        {
            get { return anoEscolhido; }
            set
            {
                anoEscolhido = value;
                AnoMudou();
            }
        }

        private async void AnoMudou()
        {
            using (var db = new NotasFiscais())
            {
                notas = new List<NFe>();
                foreach (var item in await db.RegistroAsync())
                {
                    if (item.nota.Status < 4)
                    {
                        notas.Add(item.xml.FromXElement<NFe>());
                    }
                    else
                    {
                        var proc = item.xml.FromXElement<Processo>();
                        notas.Add(proc.NFe);
                    }
                }
            }
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(ResultadoCliente)));
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(ResultadoMes)));
        }

        public ObservableCollection<TotalPorMes> ResultadoMes
        {
            get
            {
                if (notas != null)
                {
                    var total = new List<TotalPorMes>(12);
                    for (int i = 1; i < 13; i++)
                    {
                        total.Add(new TotalPorMes { Mês = i.ToString() });
                    }
                    foreach (var item in notas)
                    {
                        var det = item.Informações;
                        var data = Convert.ToDateTime(det.identificação.DataHoraEmissão);
                        foreach (var prod in det.produtos)
                        {
                            total[data.Month - 1].Quantidade += prod.Produto.QuantidadeComercializada;
                        }
                        total[data.Month - 1].Total += det.total.ICMSTot.vNF;
                    }
                    total.Add(new TotalPorMes
                    {
                        Mês = "Total",
                        Quantidade = total.Sum(x => x.Quantidade),
                        Total = total.Sum(x => x.Total)
                    });
                    return total.GerarObs();
                }
                else
                {
                    return null;
                }
            }
        }

        public ObservableCollection<TotalPorCliente> ResultadoCliente
        {
            get
            {
                if (notas != null)
                {
                    var total = new List<TotalPorCliente>();
                    foreach (var item in notas)
                    {
                        var det = item.Informações;
                        if (total.Count(x => x.Doc == det.destinatário.obterDocumento) == 0)
                            total.Add(new TotalPorCliente { Doc = det.destinatário.obterDocumento });
                        var tot = total.Single(x => x.Doc == det.destinatário.obterDocumento);
                        tot.Mun = det.destinatário.endereco.NomeMunicipio;
                        tot.Nome = det.destinatário.nome;
                        foreach (var prod in det.produtos)
                        {
                            tot.Quantidade += prod.Produto.QuantidadeComercializada;
                        }
                        tot.Total += det.total.ICMSTot.vNF;
                    }
                    total.Add(new TotalPorCliente
                    {
                        Nome = "Total",
                        Quantidade = total.Sum(x => x.Quantidade),
                        Total = total.Sum(x => x.Total)
                    });
                    return (from item in total
                            orderby item.Total descending
                            select item).GerarObs();
                }
                else
                {
                    return null;
                }
            }
        }

        public sealed class TotalPorCliente
        {
            public double Quantidade { get; set; }
            public double Total { get; set; }
            public string Nome { get; set; }
            public string Doc { get; set; }
            public string Mun { get; set; }
        }

        public sealed class TotalPorMes
        {
            public double Quantidade { get; set; }
            public double Total { get; set; }
            public string Mês { get; set; }
        }
    }
}
