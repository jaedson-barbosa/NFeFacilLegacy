using BibliotecaCentral.ItensBD;
using BibliotecaCentral.Log;
using BibliotecaCentral.ModeloXML.PartesProcesso;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTransporte;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.System.Profile;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x416

namespace NFeFacil
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ILog Log = new Saida();
        private bool avisoOrentacaoHabilitado;

        private bool AvisoOrentacaoHabilitado
        {
            get => avisoOrentacaoHabilitado;
            set
            {
                avisoOrentacaoHabilitado = value;
                AnalisarOrientacao(ApplicationView.GetForCurrentView());
            }
        }

        internal static MainPage Current { get; private set; }

        public MainPage()
        {
            InitializeComponent();
            Current = this;
            AnalisarBarraTituloAsync();
            ApplicationView.GetForCurrentView().VisibleBoundsChanged += (x, y) => AnalisarOrientacao(x);
            SystemNavigationManager.GetForCurrentView().BackRequested += (x,e) =>
            {
                e.Handled = true;
                Retornar();
            };
            AbrirInicio();
        }

        async void AbrirInicio() => await AbrirFunçaoAsync(nameof(View.Inicio));

        private void AnalisarOrientacao(ApplicationView sender)
        {
            if (AvisoOrentacaoHabilitado)
            {
                if (sender.Orientation == ApplicationViewOrientation.Landscape)
                {
                    grdAvisoRotacao.Visibility = Visibility.Collapsed;
                }
                else
                {
                    grdAvisoRotacao.Visibility = Visibility.Visible;
                }
            }
            else
            {
                grdAvisoRotacao.Visibility = Visibility.Collapsed;
            }
        }

        private static async void AnalisarBarraTituloAsync()
        {
            var familia = AnalyticsInfo.VersionInfo.DeviceFamily;
            if (familia.Contains("Mobile"))
                await StatusBar.GetForCurrentView().HideAsync();
            else if (familia.Contains("Desktop"))
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            else
                new Saida().Escrever(TitulosComuns.ErroSimples, "Tipo não reconhecido de dispositivo, não é possível mudar a barra de título.");
        }


        public async Task AbrirFunçaoAsync(string tela, object parametro = null)
        {
            await AbrirFunçaoAsync(Type.GetType($"NFeFacil.View.{tela}"), parametro);
        }

        public async Task AbrirFunçaoAsync(Type tela, object parametro = null)
        {
            if (TelasComParametroObrigatorio.Contains(tela) && parametro == null)
            {
                ObterValorPadrao(tela, out parametro);
            }

            if (frmPrincipal.Content != null)
            {
                if (frmPrincipal.Content is IEsconde esconde)
                {
                    await esconde.EsconderAsync();
                }
                else
                {
                    Log.Escrever(TitulosComuns.ErroSimples, $"A tela {frmPrincipal.Content} precisa implementar IEsconde.");
                }
            }
            frmPrincipal.Navigate(tela, parametro);
        }

        private List<Type> TelasComParametroObrigatorio = new List<Type>
        {
            typeof(View.AdicionarDestinatario),
            typeof(View.AdicionarEmitente),
            typeof(View.AdicionarMotorista),
            typeof(View.AdicionarProduto),
            typeof(View.ManipulacaoNotaFiscal)
        };

        private void ObterValorPadrao(Type tipo, out object retorno)
        {
            if (tipo == typeof(View.AdicionarDestinatario))
            {
                retorno = new GrupoViewBanco<Destinatario>();
            }
            else if (tipo == typeof(View.AdicionarEmitente))
            {
                retorno = new GrupoViewBanco<Emitente>();
            }
            else if (tipo == typeof(View.AdicionarMotorista))
            {
                retorno = new GrupoViewBanco<Motorista>();
            }
            else if (tipo == typeof(View.AdicionarProduto))
            {
                retorno = new GrupoViewBanco<BaseProdutoOuServico>();
            }
            else if (tipo == typeof(View.ManipulacaoNotaFiscal))
            {
                retorno = new ConjuntoManipuladorNFe
                {
                    NotaSalva = new NFe()
                    {
                        Informações = new Detalhes()
                        {
                            identificação = new Identificacao(),
                            emitente = new Emitente(),
                            destinatário = new Destinatario(),
                            produtos = new List<DetalhesProdutos>(),
                            transp = new Transporte(),
                            cobr = new Cobranca(),
                            infAdic = new InformacoesAdicionais(),
                            exporta = new Exportacao(),
                            compra = new Compra(),
                            cana = new RegistroAquisicaoCana()
                        }
                    },
                    OperacaoRequirida = TipoOperacao.Adicao,
                    StatusAtual = StatusNFe.Edição
                };
            }
            else
            {
                retorno = null;
                Log.Escrever(TitulosComuns.ErroSimples, "Valor padrão desse tipo não cadastrado");
            }
        }

        public void SeAtualizar(Telas atual, Symbol símbolo, string texto)
        {
            txtTitulo.Text = texto;
            AvisoOrentacaoHabilitado = TelasHorizontais.Contains(atual);
            symTitulo.Content = new SymbolIcon(símbolo);
        }

        public void SeAtualizar(Telas atual, string glyph, string texto)
        {
            txtTitulo.Text = texto;
            AvisoOrentacaoHabilitado = TelasHorizontais.Contains(atual);
            symTitulo.Content = new FontIcon
            {
                Glyph = glyph,
            };
        }

        public async void Retornar()
        {
            var frm = frmPrincipal;
            if (frm.Content is IValida retorna)
            {
                if (await retorna.Verificar())
                {
                    if (frm.Content is IEsconde esconde)
                    {
                        await esconde.EsconderAsync();
                    }
                }
                else
                {
                    return;
                }
            }
            else if (frm.Content is IEsconde esconde)
            {
                await esconde.EsconderAsync();
            }

            if (frmPrincipal.CanGoBack)
            {
                frmPrincipal.GoBack();
            }
            else
            {
                Log.Escrever(TitulosComuns.ErroSimples, "Não é possível voltar para a tela anterior.");
            }
        }

        private List<Telas> TelasHorizontais = new List<Telas>(3)
        {
            Telas.GerenciarDadosBase,
            Telas.HistoricoSincronizacao,
            Telas.NotasSalvas
        };
    }
}
