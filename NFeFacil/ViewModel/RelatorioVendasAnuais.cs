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
            ResultadoMes = new ObservableCollection<TotalPorMes>();
            ResultadoCliente = new ObservableCollection<TotalPorCliente>();
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

                    ResultadoCliente.Clear();
                    var gruposClientes = from nota in notas
                                         group nota by nota.Informações.destinatário.Documento into item
                                         let total = item.Sum(x => x.Informações.total.ICMSTot.VNF)
                                         orderby total descending
                                         select new { Notas = item, Total = total };
                    foreach (var item in gruposClientes)
                    {
                        var det = item.Notas.Last().Informações;
                        var atual = new TotalPorCliente
                        {
                            Doc = det.destinatário.Documento,
                            Mun = det.destinatário.Endereco.NomeMunicipio,
                            Nome = det.destinatário.Nome,
                            Quantidade = item.Notas.Sum(x => x.Informações.produtos.Sum(prod => prod.Produto.QuantidadeComercializada)),
                            Total = item.Total
                        };
                        ResultadoCliente.Add(atual);
                    }

                    ResultadoMes.Clear();
                    var gruposMeses = from nota in notas
                                      let data = DateTime.Parse(nota.Informações.identificação.DataHoraEmissão)
                                      group new { Nota = nota, Data = data.Month } by data.Month;
                    foreach (var item in gruposMeses)
                    {
                        var primeiro = item.First();
                        var atual = new TotalPorMes
                        {
                            Mês = primeiro.Data.ToString(),
                            Quantidade = item.Sum(det => det.Nota.Informações.produtos.Sum(prod => prod.Produto.QuantidadeComercializada)),
                            Total = item.Sum(det => det.Nota.Informações.total.ICMSTot.VNF)
                        };
                        ResultadoMes.Add(atual);
                    }
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
