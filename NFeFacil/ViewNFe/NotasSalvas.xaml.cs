using NFeFacil.ItensBD;
using NFeFacil.ModeloXML;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Navigation;
using NFeFacil.Controles;
using System.Xml.Serialization;
using NFeFacil.Log;
using NFeFacil.WebService.Pacotes;
using System.Threading.Tasks;
using NFeFacil.WebService;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class NotasSalvas : Page, IHambuguer
    {
        public NotasSalvas()
        {
            InitializeComponent();
            using (var db = new AplicativoContext())
            {
                var notasFiscais = db.NotasFiscais.ToArray();
                NotasEmitidas = (from nota in notasFiscais
                                 where nota.Status == (int)StatusNFe.Emitida
                                 orderby nota.DataEmissao descending
                                 select new NFeView(nota)).GerarObs();
                OutrasNotas = (from nota in notasFiscais
                               where nota.Status != (int)StatusNFe.Emitida
                               orderby nota.DataEmissao descending
                               select new NFeView(nota)).GerarObs();
                
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            MainPage.Current.SeAtualizar(Symbol.Library, "Notas salvas");
        }

        ObservableCollection<NFeView> NotasEmitidas { get; }
        ObservableCollection<NFeView> OutrasNotas { get; }

        ICollectionView NotasEmitidasView => new CollectionViewSource() { Source = NotasEmitidas }.View;
        ICollectionView OutrasNotasView => new CollectionViewSource() { Source = OutrasNotas }.View;

        public ObservableCollection<ItemHambuguer> ConteudoMenu => new ObservableCollection<ItemHambuguer>
        {
            new ItemHambuguer(Symbol.Send, "Emitidas"),
            new ItemHambuguer(Symbol.SaveLocal, "Outras")
        };

        private void Exibir(object sender, RoutedEventArgs e)
        {
            var nota = (NFeView)((MenuFlyoutItem)sender).DataContext;
            MainPage.Current.Navegar<VisualizacaoNFe>(nota.Nota);
        }

        private async void Cancelar(object sender, RoutedEventArgs e)
        {
            var nota = (NFeView)((MenuFlyoutItem)sender).DataContext;
            var Nota = nota.Nota;
            var processo = XElement.Parse(Nota.XML).FromXElement<Processo>();
            if (await Cancelar(processo))
            {
                Nota.Status = (int)StatusNFe.Cancelada;
                using (var db = new AplicativoContext())
                {
                    Nota.UltimaData = DateTime.Now;
                    db.Update(Nota);
                    db.SaveChanges();
                }

                nota.CalcularMensagemApoio();
                NotasEmitidas.Remove(nota);
                OutrasNotas.Add(nota);
            }
        }

        public async Task<bool> Cancelar(Processo Processo)
        {
            ILog Log = Popup.Current;

            try
            {
                var estado = Processo.NFe.Informacoes.identificacao.CódigoUF;
                var tipoAmbiente = Processo.ProtNFe.InfProt.tpAmb;

                var gerenciador = new GerenciadorGeral<EnvEvento, RetEnvEvento>(estado, Operacoes.RecepcaoEvento, tipoAmbiente == 2);

                var cnpj = Processo.NFe.Informacoes.emitente.CNPJ;
                var chave = Processo.NFe.Informacoes.ChaveAcesso;
                var nProtocolo = Processo.ProtNFe.InfProt.nProt;
                var versao = gerenciador.Enderecos.VersaoRecepcaoEvento;
                var entrada = new CancelarNFe();

                if (await entrada.ShowAsync() == ContentDialogResult.Primary)
                {
                    var infoEvento = new InformacoesEvento(estado, cnpj, chave, versao, nProtocolo, entrada.Motivo, tipoAmbiente);
                    var envio = new EnvEvento(gerenciador.Enderecos.VersaoRecepcaoEvento, infoEvento);
                    await envio.PrepararEventos();
                    var resposta = await gerenciador.EnviarAsync(envio);
                    if (resposta.RetEvento[0].InfEvento.CStat == 135)
                    {
                        using (var contexto = new AplicativoContext())
                        {
                            contexto.Cancelamentos.Add(new RegistroCancelamento()
                            {
                                ChaveNFe = chave,
                                DataHoraEvento = resposta.RetEvento[0].InfEvento.DhRegEvento,
                                TipoAmbiente = tipoAmbiente,
                                XML = new ProcEventoCancelamento()
                                {
                                    Eventos = envio.Eventos,
                                    RetEvento = resposta.RetEvento,
                                    Versao = resposta.Versao
                                }.ToXElement<ProcEventoCancelamento>().ToString()
                            });
                            contexto.SaveChanges();
                        }
                        Log.Escrever(TitulosComuns.Sucesso, "NFe cancelada com sucesso.");
                        return true;
                    }
                    else
                    {
                        Log.Escrever(TitulosComuns.Erro, resposta.RetEvento[0].InfEvento.XMotivo);
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                Log.Escrever(TitulosComuns.Erro, e.Message);
                return false;
            }
        }

        public void AtualizarMain(int index) => main.SelectedIndex = index;

        private void TelaMudou(object sender, SelectionChangedEventArgs e)
        {
            var index = ((FlipView)sender).SelectedIndex;
            MainPage.Current.AlterarSelectedIndexHamburguer(index);
        }

        sealed class NFeView
        {
            public NFeDI Nota { get; }
            public string MensagemApoio { get; set; }

            public NFeView(NFeDI nota)
            {
                Nota = nota;
                CalcularMensagemApoio();
            }

            public void CalcularMensagemApoio()
            {
                if (Nota.Status == (int)StatusNFe.Emitida)
                {
                    MensagemApoio = $"Exportada: {BoolToString(Nota.Exportada)}; Impressa: {BoolToString(Nota.Impressa)}";
                }
                else
                {
                    MensagemApoio = $"Status: {((StatusNFe)Nota.Status).ToString()}";
                }

                string BoolToString(bool booleano) => booleano ? "Sim" : "Não";
            }

            public override bool Equals(object obj)
            {
                if (obj is NFeView view)
                {
                    return Nota.Id == view.Nota.Id;
                }
                return false;
            }

            public override int GetHashCode()
            {
                return Nota.Id.GetHashCode();
            }
        }

        [XmlRoot("procEventoNFe", Namespace = "http://www.portalfiscal.inf.br/nfe")]
        public struct ProcEventoCancelamento
        {
            [XmlAttribute("versao")]
            public string Versao { get; set; }

            [XmlElement("evento")]
            public Evento[] Eventos { get; set; }

            [XmlElement("retEvento")]
            public ResultadoEvento[] RetEvento { get; set; }
        }
    }
}
