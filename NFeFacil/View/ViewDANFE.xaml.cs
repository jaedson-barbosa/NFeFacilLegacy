using NFeFacil.DANFE;
using BibliotecaCentral.ModeloXML;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.Graphics.Display;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Linq;
using System.Diagnostics;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class ViewDANFE : Page
    {
        private Processo processo;
        private GerenciadorImpressao gerenciadorImpressão;
        private GerenciadorExportacao gerenciadorExportação;

        public double Largura => CentimeterToPixel(21);
        public double Altura => CentimeterToPixel(29.7);

        private byte[] Pixels;

        int CentimeterToPixel(double Centimeter)
        {
            var dpi = DisplayInformation.GetForCurrentView().LogicalDpi;
            return (int)(Centimeter * dpi / 2.54d);
        }

        public ViewDANFE()
        {
            InitializeComponent();
            MainPage.Current.SeAtualizar(Symbol.View, "Visualizar DANFE");
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            processo = (Processo)e.Parameter;

            gerenciadorImpressão = new GerenciadorImpressao(processo, ref webView);
            gerenciadorExportação = new GerenciadorExportacao(processo, ref webView, ref Retangulo);
            gerenciadorImpressão.PaginasCarregadas += async (x, y) =>
            {
                await Task.Delay(1000);

                var _Brush = new WebViewBrush
                {
                    Stretch = Windows.UI.Xaml.Media.Stretch.Uniform
                };
                _Brush.SetSource(webView);
                _Brush.Redraw();
                Retangulo.Fill = _Brush;

                await Task.Delay(3000);
                var render = new RenderTargetBitmap();
                await render.RenderAsync(Retangulo);
                var pixels = await render.GetPixelsAsync();
                var bytes = pixels.ToArray();
                var porcentBranco = bytes.Count(k => k == 255) * 100 / bytes.Length;
                if (porcentBranco > 99 || bytes.Length < 10000)
                {
                    Debug.WriteLine("Não é válido");
                }
                else
                {
                    btnImprimir.IsEnabled = true;
                    btnSalvar.IsEnabled = true;
                    Pixels = bytes;
                }
            };
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e) => Dispose();
        private async void btnImprimir_Click(object sender, RoutedEventArgs e) => await gerenciadorImpressão.Imprimir(webView);
        private async void btnSalvar_Click(object sender, RoutedEventArgs e) => await gerenciadorExportação.Salvar(Pixels);
        public void Dispose() => gerenciadorImpressão.Dispose();
    }
}
