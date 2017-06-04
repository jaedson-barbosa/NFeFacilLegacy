using NFeFacil.DANFE;
using BibliotecaCentral.ModeloXML;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class ViewDANFE : Page
    {
        private GerenciadorImpressao gerenciadorImpressão;

        public double Largura => CentimeterToPixel(21);
        public double Altura => CentimeterToPixel(29.7);

        private byte[] Pixels;

        double CentimeterToPixel(double Centimeter)
        {
            //96 é a constante usada pelo CSS e 2.54 é a quantidade de cm de uma polegada
            const double fator = 96 / 2.54;
            return Centimeter * fator;
        }

        public ViewDANFE()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            MainPage.Current.SeAtualizar(Symbol.View, "Visualizar DANFE");

            gerenciadorImpressão = new GerenciadorImpressao();

            var processo = (Processo)e.Parameter;
            stk.Children.Add(new PaginasDANFE.PaginaPrincipal(processo));
            btnImprimir.IsEnabled = true;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e) => Dispose();
        private async void btnImprimir_Click(object sender, RoutedEventArgs e) => await gerenciadorImpressão.Imprimir(stk.Children[0]);
        public void Dispose() => gerenciadorImpressão.Dispose();
    }
}
