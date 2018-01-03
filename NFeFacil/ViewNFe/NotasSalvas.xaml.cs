using NFeFacil.ItensBD;
using NFeFacil.ModeloXML;
using System;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using NFeFacil.Controles;
using System.Xml.Serialization;
using NFeFacil.WebService.Pacotes;
using NFeFacil.WebService;
using NFeFacil.Validacao;
using Windows.UI.Xaml.Media;
using NFeFacil.View;
using NFeFacil.WebService.Pacotes.PartesEnvEvento;
using NFeFacil.WebService.Pacotes.PartesRetEnvEvento;
using NFeFacil.Certificacao;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe
{
    [DetalhePagina(Symbol.Library, "Notas salvas")]
    public sealed partial class NotasSalvas : Page, IHambuguer
    {
        public NotasSalvas()
        {
            InitializeComponent();
            using (var repo = new Repositorio.Leitura())
            {
                var (emitidas, outras, canceladas) = repo.ObterNotas(DefinicoesTemporarias.EmitenteAtivo.CNPJ);
                NotasEmitidas = emitidas.GerarObs();
                OutrasNotas = outras.GerarObs();
                NotasCanceladas = canceladas.GerarObs();
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
            using (var repo = new Repositorio.OperacoesExtras())
            {
                repo.ExcluirNFe(nota);
                OutrasNotas.Remove(nota);
            }
        }

        private async void Cancelar(object sender, RoutedEventArgs e)
        {
            var nota = (NFeDI)((MenuFlyoutItem)sender).DataContext;
            var processo = XElement.Parse(nota.XML).FromXElement<Processo>();

            var estado = processo.NFe.Informacoes.identificacao.CódigoUF;
            var tipoAmbiente = processo.ProtNFe.InfProt.tpAmb;

            var gerenciador = new GerenciadorGeral<EnvEvento, RetEnvEvento>(estado, Operacoes.RecepcaoEvento, tipoAmbiente == 2);

            var cnpj = processo.NFe.Informacoes.emitente.CNPJ;
            var chave = processo.NFe.Informacoes.ChaveAcesso;
            var nProtocolo = processo.ProtNFe.InfProt.nProt;
            var entrada = new CancelarNFe();

            if (await entrada.ShowAsync() == ContentDialogResult.Primary)
            {
                var infoEvento = new InformacoesEvento(estado, cnpj, chave, nProtocolo, entrada.Motivo, tipoAmbiente);
                var envio = new EnvEvento(infoEvento);

                AssinaFacil assinador = new AssinaFacil();
                await assinador.Preparar();

                Progresso progresso = null;
                progresso = new Progresso(async x =>
                {
                    var resultado = await envio.PrepararEventos(assinador, x);
                    if (!resultado.Item1)
                    {
                        return resultado;
                    }
                    await progresso.Update(1);

                    var resposta = await gerenciador.EnviarAsync(envio);
                    if (resposta.ResultadorEventos[0].InfEvento.CStat == 135)
                    {
                        using (var repo = new Repositorio.Escrita())
                        {
                            repo.SalvarItemSimples(new RegistroCancelamento()
                            {
                                ChaveNFe = chave,
                                DataHoraEvento = resposta.ResultadorEventos[0].InfEvento.DhRegEvento,
                                TipoAmbiente = tipoAmbiente,
                                XML = new ProcEventoCancelamento()
                                {
                                    Eventos = envio.Eventos,
                                    RetEvento = resposta.ResultadorEventos,
                                    Versao = resposta.Versao
                                }.ToXElement<ProcEventoCancelamento>().ToString()
                            }, DefinicoesTemporarias.DateTimeNow);

                            nota.Status = (int)StatusNFe.Cancelada;
                            repo.SalvarItemSimples(nota, DefinicoesTemporarias.DateTimeNow);
                            await progresso.Update(6);

                            NotasEmitidas.Remove(nota);
                            NotasCanceladas.Insert(0, nota);
                        }
                        return (true, "NFe cancelada com sucesso.");
                    }
                    else
                    {
                        return (false, resposta.ResultadorEventos[0].InfEvento.XMotivo);
                    }
                }, assinador.CertificadosDisponiveis, "Subject",
                "Preparar eventos com assinatura do emitente",
                "Preparar conexão",
                "Obter conteúdo da requisição",
                "Enviar requisição",
                "Processar resposta",
                "Salvar registro de cancelamento no banco de dados");
                gerenciador.ProgressChanged += async (x, y) => await progresso.Update(y + 1);
                await progresso.ShowAsync();
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
