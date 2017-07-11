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

        public double QuantTotal { get; private set; }
        public double ValorTotal { get; private set; }

        public RelatorioVendasAnuais()
        {
            using (var db = new AplicativoContext())
            {
                AnosDisponiveis = (from dado in db.NotasFiscais
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
            try
            {
                using (var db = new AplicativoContext())
                {
                    var notas = from item in db.NotasFiscais
                                let data = DateTime.Parse(item.DataEmissao)
                                where data.Year == AnoEscolhido
                                orderby data
                                let xml = XElement.Parse(item.XML)
                                let usarNFe = item.Status < 4
                                let nota = usarNFe ? xml.FromXElement<NFe>() : (xml.FromXElement<Processo>()).NFe
                                select nota;

                    var totalCliente = new List<TotalPorCliente>();
                    var gruposClientes = from nota in notas
                                         group nota by nota.Informações.destinatário.Documento;
                    foreach (var item in gruposClientes)
                    {
                        var det = item.Last().Informações;
                        var atual = new TotalPorCliente
                        {
                            Doc = det.destinatário.Documento,
                            Mun = det.destinatário.Endereco.NomeMunicipio,
                            Nome = det.destinatário.Nome,
                            Quantidade = item.Sum(x => x.Informações.produtos.Sum(prod => prod.Produto.QuantidadeComercializada)),
                            Total = item.Sum(x => x.Informações.total.ICMSTot.VNF)
                        };
                        totalCliente.Add(atual);
                    }
                    ResultadoCliente = (from item in totalCliente
                                        orderby item.Total descending
                                        select item).GerarObs();
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(ResultadoCliente)));

                    QuantTotal = notas.Sum(x => x.Informações.produtos.Sum(prod => prod.Produto.QuantidadeComercializada));
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(QuantTotal)));

                    ValorTotal = notas.Sum(x => x.Informações.total.ICMSTot.VNF);
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(ValorTotal)));

                    var gruposMeses = from nota in notas
                                      let data = DateTime.Parse(nota.Informações.identificação.DataHoraEmissão)
                                      group new { nota = nota, data = data.Month } by data.Month;
                    var totalMes = new List<TotalPorMes>(gruposMeses.Count());
                    foreach (var item in gruposMeses)
                    {
                        var primeiro = item.First();
                        var atual = new TotalPorMes
                        {
                            Mês = primeiro.data.ToString(),
                            Quantidade = item.Sum(det => det.nota.Informações.produtos.Sum(prod => prod.Produto.QuantidadeComercializada)),
                            Total = item.Sum(det => det.nota.Informações.total.ICMSTot.VNF)
                        };
                        totalMes.Add(atual);
                    }
                    ResultadoMes = totalMes.GerarObs();
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(ResultadoMes)));
                }
            }
            catch (Exception e)
            {
                e.ManipularErro();
            }
        }

        public struct TotalPorCliente
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
