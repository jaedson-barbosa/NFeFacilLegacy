using System;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.System.Profile;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace NFeFacil
{
    /// <summary>
    ///Fornece o comportamento específico do aplicativo para complementar a classe Application padrão.
    /// </summary>
    sealed partial class App : Application
    {
        bool IsMobile { get; }
        bool IsDesktop { get; }

        /// <summary>
        /// Inicializa o objeto singleton do aplicativo.  Esta é a primeira linha de código criado
        /// executado e, como tal, é o equivalente lógico de main() ou WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();
            var familia = AnalyticsInfo.VersionInfo.DeviceFamily;
            IsMobile = familia.Contains("Mobile");
            IsDesktop = familia.Contains("Desktop");
        }

        /// <summary>
        /// Chamado quando o aplicativo é iniciado normalmente pelo usuário final.  Outros pontos de entrada
        /// serão usados, por exemplo, quando o aplicativo for iniciado para abrir um arquivo específico.
        /// </summary>
        /// <param name="e">Detalhes sobre a solicitação e o processo de inicialização.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            PersonalizarDesign(BaseGeral.DefinicoesPermanentes.UsarFluent);

            var rootFrame = Window.Current.Content as MainPage;
            if (rootFrame == null)
            {
                Window.Current.Content = rootFrame = new MainPage();
            }

            if (e.PrelaunchActivated == false)
            {
                Window.Current.Activate();
            }

            PersonalisarBarraTitulo();
        }

        void PersonalisarBarraTitulo()
        {
            MainPage current = MainPage.Current;
            if (IsMobile) EsconderBarraCelular();
            else if (IsDesktop)
            {
                CoreApplicationViewTitleBar tb = CoreApplication.GetCurrentView().TitleBar;
                tb.ExtendViewIntoTitleBar = true;
                tb.LayoutMetricsChanged += (sender, e) => current.TitleBar.Height = sender.Height;

                Window.Current.SetTitleBar(current.MainTitleBar);
                Window.Current.Activated += (sender, e) => current.TitleBar.Opacity = e.WindowActivationState != CoreWindowActivationState.Deactivated ? 1 : 0.5;

                var novoTB = ApplicationView.GetForCurrentView().TitleBar;
                novoTB.ButtonBackgroundColor = Colors.Transparent;
                novoTB.ButtonInactiveBackgroundColor = Colors.Transparent;
                novoTB.ButtonHoverBackgroundColor = new Color { A = 50 };
                novoTB.ButtonPressedBackgroundColor = new Color { A = 100 };
            }

            async void EsconderBarraCelular() => await StatusBar.GetForCurrentView().HideAsync();
        }

        void PersonalizarDesign(bool usarFluent)
        {
            if (usarFluent && !IsMobile)
            {
                ((Style)Resources[typeof(CommandBar)]).BasedOn = (Style)Resources["CommandBarRevealStyle"];
                ((Style)Resources[typeof(Button)]).BasedOn = (Style)Resources["ButtonRevealStyle"];
            }
        }
    }
}
