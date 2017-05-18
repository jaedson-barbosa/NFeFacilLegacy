using Windows.ApplicationModel.Core;
using Windows.System.Profile;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace NFeFacil
{
    [Windows.UI.Xaml.Markup.ContentProperty(Name = nameof(Icone))]
    public sealed partial class BarraTitulo : UserControl
    {
        public IconElement Icone { get; set; }
        public string Texto { get; set; }

        public BarraTitulo()
        {
            InitializeComponent();
            btnRetornar.Click += (x, y) => App.Retornar();
            AnalisarBarraTitulo();
        }

        private void AnalisarBarraTitulo()
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
                Window.Current.Activated += (sender, e) => TitleBar.Opacity = e.WindowActivationState != Windows.UI.Core.CoreWindowActivationState.Deactivated ? 1 : 0.5;

                var novoTB = ApplicationView.GetForCurrentView().TitleBar;
                var corBackground = new Color { A = 0 };
                novoTB.ButtonBackgroundColor = corBackground;
                novoTB.ButtonInactiveBackgroundColor = corBackground;
                novoTB.ButtonHoverBackgroundColor = new Color { A = 50 };
                novoTB.ButtonPressedBackgroundColor = new Color { A = 100 };
            }
        }
    }
}
