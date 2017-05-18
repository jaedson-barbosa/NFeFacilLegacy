using BibliotecaCentral.Log;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.System.Profile;
using Windows.UI;
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
            btnRetornar.Click += (x, y) => Retornar();
            SystemNavigationManager.GetForCurrentView().BackRequested += (x,e) =>
            {
                e.Handled = true;
                Retornar();
            };
            AbrirFunçao(typeof(View.Inicio));
        }

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

        private void AnalisarBarraTituloAsync()
        {
            var familia = AnalyticsInfo.VersionInfo.DeviceFamily;
            if (familia.Contains("Mobile"))
            {
                btnRetornar.Visibility = Visibility.Collapsed;
                var barra = StatusBar.GetForCurrentView();
                var cor = new View.Estilos.Auxiliares.BibliotecaCores().Cor1;
                barra.BackgroundColor = cor;
                barra.BackgroundOpacity = 1;
            }
            else if (familia.Contains("Desktop"))
            {
                CoreApplicationViewTitleBar tb = CoreApplication.GetCurrentView().TitleBar;
                tb.ExtendViewIntoTitleBar = true;
                tb.IsVisibleChanged += (sender, e) => TitleBar.Visibility = sender.IsVisible ? Visibility.Visible : Visibility.Collapsed;
                tb.LayoutMetricsChanged += (sender, e) => TitleBar.Height = sender.Height;
                Window.Current.SetTitleBar(MainTitleBar);
                Window.Current.Activated += (sender, e) => TitleBar.Opacity = e.WindowActivationState != CoreWindowActivationState.Deactivated ? 1 : 0.5;

                var novoTB = ApplicationView.GetForCurrentView().TitleBar;
                var corBackground = new Color { A = 0 };
                novoTB.ButtonBackgroundColor = corBackground;
                novoTB.ButtonInactiveBackgroundColor = corBackground;
                novoTB.ButtonHoverBackgroundColor = new Color { A = 50 };
                novoTB.ButtonPressedBackgroundColor = new Color { A = 100 };
            }
            else
            {
                Log.Escrever(TitulosComuns.ErroSimples, "Tipo não reconhecido de dispositivo, não é possível mudar a barra de título.");
            }
        }

        public void AbrirFunçao(string tela, object parametro = null)
        {
            AbrirFunçao(Type.GetType($"NFeFacil.View.{tela}"), parametro);
        }

        public void AbrirFunçao(Type tela, object parametro = null)
        {
            frmPrincipal.Navigate(tela, parametro);
        }

        public async void SeAtualizar(Telas atual, Symbol símbolo, string texto)
        {
            txtTitulo.Text = texto;
            AvisoOrentacaoHabilitado = TelasHorizontais.Contains(atual);
            symTitulo.Content = new SymbolIcon(símbolo);

            if (atual == Telas.Inicio)
            {
                await Task.Delay(1000);
                frmPrincipal.BackStack.Clear();
                frmPrincipal.ForwardStack.Clear();
            }
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
                if (!await retorna.Verificar())
                {
                    return;
                }
            }
            else if ((frm.Content as FrameworkElement).DataContext is IValida retornaDC)
            {
                if (!await retornaDC.Verificar())
                {
                    return;
                }
            }

            if (frmPrincipal.CanGoBack)
            {
                frmPrincipal.GoBack(new Windows.UI.Xaml.Media.Animation.DrillInNavigationTransitionInfo());
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
