using NFeFacil.ViewModel.Itens;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace NFeFacil.ViewModel
{
    public sealed class RelatorioVendasAnuais : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<int> AnosDisponiveis
        {
            get
            {
                using (var tabela = new TabelaNFe())
                {
                    var anos = from dado in tabela.RegistroCompleto()
                               let ano = dado.DataEmissao.ToDateTime().Year
                               orderby ano ascending
                               select ano;
                    return anos.Distinct().GerarObs();
                }
            }
        }

        private IEnumerable<NFe> notas;

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
            using (TabelaNFe tabela = new TabelaNFe())
            {
                notas = from c in await Task.WhenAll(from dado in tabela.RegistroCompleto()
                                                     where dado.DataEmissao.ToDateTime().Year == anoEscolhido
                                                     select tabela.Retornar(dado.ID))
                        select c.nota ?? c.proc.NFe;
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
                        var det = item.informações;
                        var data = det.identificação.dataHoraEmissão.ToDateTime();
                        foreach (var prod in det.produtos)
                        {
                            total[data.Month - 1].Quantidade += prod.produto.quantidadeComercializada;
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
                else return null;
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
                        var det = item.informações;
                        if (total.Count(x => x.Doc == det.destinatário.obterDocumento) == 0)
                            total.Add(new TotalPorCliente { Doc = det.destinatário.obterDocumento });
                        var tot = total.Single(x => x.Doc == det.destinatário.obterDocumento);
                        tot.Mun = det.destinatário.endereço.nomeMunicipio;
                        tot.Nome = det.destinatário.nome;
                        foreach (var prod in det.produtos)
                        {
                            tot.Quantidade += prod.produto.quantidadeComercializada;
                        }
                        tot.Total += det.total.ICMSTot.vNF;
                    }
                    total.Add(new TotalPorCliente
                    {
                        Nome = "Total",
                        Quantidade = total.Sum(x => x.Quantidade),
                        Total = total.Sum(x => x.Total)
                    });
                    return total.GerarObs();
                }
                else return null;
            }
        }
    }
}
