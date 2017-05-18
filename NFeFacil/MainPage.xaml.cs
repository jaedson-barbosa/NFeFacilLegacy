using BibliotecaCentral.Log;
using System;
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
    public sealed partial class MainPage : ContentPresenter
    {
        private ILog Log = new Saida();
        internal static MainPage Current { get; private set; }

        public MainPage()
        {
            InitializeComponent();
            Current = this;
            AnalisarBarraTituloAsync();
            btnRetornar.Click += (x, y) => Retornar();
            SystemNavigationManager.GetForCurrentView().BackRequested += (x,e) =>
            {
                e.Handled = true;
                Retornar();
            };
            AbrirFunçao(typeof(View.Inicio));
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

        public void AbrirFunçao(Type tela, object parametro = null)
        {
            frmPrincipal.Navigate(tela, parametro);
        }

        public void SeAtualizar(Telas atual, Symbol símbolo, string texto)
        {
            txtTitulo.Text = texto;
            symTitulo.Content = new SymbolIcon(símbolo);
        }

        public void SeAtualizar(Telas atual, string glyph, string texto)
        {
            txtTitulo.Text = texto;
            symTitulo.Content = new FontIcon
            {
                Glyph = glyph,
            };
        }

        public async void Retornar()
        {
            if (frmPrincipal.Content is IValida retorna)
            {
                if (!await retorna.Verificar())
                {
                    return;
                }
            }
            else if ((frmPrincipal.Content as FrameworkElement).DataContext is IValida retornaDC)
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
    }
}
