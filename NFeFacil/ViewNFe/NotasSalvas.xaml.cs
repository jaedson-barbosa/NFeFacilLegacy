using NFeFacil.ItensBD;
using NFeFacil.ModeloXML;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using NFeFacil.Controles;
using System.Xml.Serialization;
using NFeFacil.Log;
using NFeFacil.WebService.Pacotes;
using System.Threading.Tasks;
using NFeFacil.WebService;
using NFeFacil.Validacao;
using Windows.UI.Xaml.Media;

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
                                 where nota.CNPJEmitente == Propriedades.EmitenteAtivo.CNPJ
                                 orderby nota.DataEmissao descending
                                 select nota).GerarObs();
                OutrasNotas = (from nota in notasFiscais
                               where nota.Status != (int)StatusNFe.Emitida && nota.Status != (int)StatusNFe.Cancelada
                               where nota.CNPJEmitente == Propriedades.EmitenteAtivo.CNPJ
                               orderby nota.DataEmissao descending
                               select nota).GerarObs();
                NotasCanceladas = (from nota in notasFiscais
                               where nota.Status == (int)StatusNFe.Cancelada
                               where nota.CNPJEmitente == Propriedades.EmitenteAtivo.CNPJ
                               orderby nota.DataEmissao descending
                               select nota).GerarObs();
            }
        }

        ObservableCollection<NFeDI> NotasEmitidas { get; }
        ObservableCollection<NFeDI> OutrasNotas { get; }
        ObservableCollection<NFeDI> NotasCanceladas { get; }

        public ObservableCollection<ItemHambuguer> ConteudoMenu => new ObservableCollection<ItemHambuguer>
        {
            new ItemHambuguer(Symbol.Send, "Emitidas"),
            new ItemHambuguer(Symbol.SaveLocal, "Outras"),
            new ItemHambuguer(Symbol.Cancel, "Canceladas")
        };

        private void Exibir(object sender, RoutedEventArgs e)
        {
            var nota = (NFeDI)((MenuFlyoutItem)sender).DataContext;
            MainPage.Current.Navegar<VisualizacaoNFe>(nota);
        }

        private void Excluir(object sender, RoutedEventArgs e)
        {
            var nota = (NFeDI)((MenuFlyoutItem)sender).DataContext;
            using (var db = new AplicativoContext())
            {
                db.NotasFiscais.Remove(nota);
                OutrasNotas.Remove(nota);
                db.SaveChanges();
            }
            Popup.Current.Escrever(TitulosComuns.Sucesso, "Nota excluída com sucesso.");
        }

        private async void Cancelar(object sender, RoutedEventArgs e)
        {
            var nota = (NFeDI)((MenuFlyoutItem)sender).DataContext;
            var processo = XElement.Parse(nota.XML).FromXElement<Processo>();
            if (await Cancelar(processo))
            {
                nota.Status = (int)StatusNFe.Cancelada;
                using (var db = new AplicativoContext())
                {
                    nota.UltimaData = Propriedades.DateTimeNow;
                    db.Update(nota);
                    db.SaveChanges();

                    NotasEmitidas.Remove(nota);
                    NotasCanceladas.Insert(0, nota);
                }
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

        public int SelectedIndex { set => main.SelectedIndex = value; }

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

        async void CriarCopia(object sender, RoutedEventArgs e)
        {
            var nota = (NFeDI)((MenuFlyoutItem)sender).DataContext;
            var processo = XElement.Parse(nota.XML).FromXElement<Processo>();
            var nfe = processo.NFe;
            var analisador = new AnalisadorNFe(ref nfe);
            analisador.Desnormalizar();
            await new CriadorNFe(nfe).ShowAsync();
        }
    }

    sealed class BoolToColor : IValueConverter
    {
        static readonly Brush Ativo = new SolidColorBrush(new AuxiliaresEstilos.BibliotecaCores().Cor1);
        static readonly Brush Inativo = new SolidColorBrush(Windows.UI.Colors.Transparent);

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var booleano = (bool)value;
            return booleano ? Ativo : Inativo;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
