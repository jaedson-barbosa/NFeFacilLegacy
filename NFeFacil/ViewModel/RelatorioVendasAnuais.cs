using BibliotecaCentral;
using BibliotecaCentral.ModeloXML;
using BibliotecaCentral.ModeloXML.PartesProcesso;
using BibliotecaCentral.Repositorio;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Xml.Linq;

namespace NFeFacil.ViewModel
{
    public sealed class RelatorioVendasAnuais : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<int> AnosDisponiveis { get; private set; }
        public ObservableCollection<TotalPorMes> ResultadoMes { get; private set; }
        public ObservableCollection<TotalPorCliente> ResultadoCliente { get; private set; }

        public RelatorioVendasAnuais()
        {
            using (var db = new NotasFiscais())
            {
                AnosDisponiveis = (from dado in db.Registro
                                   let ano = Convert.ToDateTime(dado.DataEmissao).Year
                                   orderby ano ascending
                                   select ano).Distinct().GerarObs();
            }
        }

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

        private void AnoMudou()
        {
            using (var db = new NotasFiscais())
            {
                var notas = from item in db.Registro
                        let xml = XElement.Parse(item.XML)
                        let usarNFe = item.Status < 4
                        let nota = usarNFe ? xml.FromXElement<NFe>() : (xml.FromXElement<Processo>()).NFe
                        select nota;

                var totalCliente = new List<TotalPorCliente>();
                foreach (var item in notas)
                {
                    var det = item.Informações;
                    if (totalCliente.Count(x => x.Doc == det.destinatário.Documento) == 0)
                        totalCliente.Add(new TotalPorCliente { Doc = det.destinatário.Documento });
                    var tot = totalCliente.Single(x => x.Doc == det.destinatário.Documento);
                    tot.Mun = det.destinatário.endereco.NomeMunicipio;
                    tot.Nome = det.destinatário.nome;
                    foreach (var prod in det.produtos)
                    {
                        tot.Quantidade += prod.Produto.QuantidadeComercializada;
                    }
                    tot.Total += det.total.ICMSTot.vNF;
                }
                totalCliente.Add(new TotalPorCliente
                {
                    Nome = "Total",
                    Quantidade = totalCliente.Sum(x => x.Quantidade),
                    Total = totalCliente.Sum(x => x.Total)
                });
                ResultadoCliente = (from item in totalCliente
                                    orderby item.Total descending
                                    select item).GerarObs();
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(ResultadoCliente)));

                var totalMes = new List<TotalPorMes>(12);
                for (int i = 1; i < 13; i++)
                {
                    totalMes.Add(new TotalPorMes { Mês = i.ToString() });
                }
                foreach (var item in notas)
                {
                    var det = item.Informações;
                    var data = Convert.ToDateTime(det.identificação.DataHoraEmissão);
                    foreach (var prod in det.produtos)
                    {
                        totalMes[data.Month - 1].Quantidade += prod.Produto.QuantidadeComercializada;
                    }
                    totalMes[data.Month - 1].Total += det.total.ICMSTot.vNF;
                }
                totalMes.Add(new TotalPorMes
                {
                    Mês = "Total",
                    Quantidade = totalMes.Sum(x => x.Quantidade),
                    Total = totalMes.Sum(x => x.Total)
                });
                ResultadoMes = totalMes.GerarObs();
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(ResultadoMes)));
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
