using NFeFacil.DANFE.Pacotes;
using NFeFacil.DANFE.Processamento;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace NFeFacil.View.PaginasDANFE
{
    public sealed partial class PaginaPrincipal : UserControl
    {
        double LarguraPagina => CentimeterToPixel(21);
        double AlturaPagina => CentimeterToPixel(29.7);

        Thickness MargemPadrao => new Thickness(CentimeterToPixel(1));

        DadosCabecalho ContextoCanhoto { get; }
        DadosAdicionais ContextoDadosAdicionais { get; }
        DadosCliente ContextoCliente { get; }
        DadosImposto ContextoImposto { get; }
        DadosMotorista ContextoTransporte { get; }
        DadosNFe ContextoNFe { get; }
        DadosISSQN ContextoISSQN { get; }
        Geral ContextoGeral { get; }

        UIElementCollection PaiPaginas { get; }

        public PaginaPrincipal(BibliotecaCentral.ModeloXML.Processo processo, UIElementCollection paiPaginas)
        {
            this.InitializeComponent();
            var geral = ViewDados.Converter(processo);
            ContextoCanhoto = geral._DadosCabecalho;
            ContextoDadosAdicionais = geral._DadosAdicionais;
            ContextoCliente = geral._DadosCliente;
            ContextoImposto = geral._DadosImposto;
            ContextoTransporte = geral._DadosMotorista;
            ContextoNFe = geral._DadosNFe;
            ContextoISSQN = geral._DadosISSQN;
            ContextoGeral = geral;

            PaiPaginas = paiPaginas;
        }

        static double CentimeterToPixel(double Centimeter)
        {
            const double fator = 96 / 2.54;
            return Centimeter * fator;
        }

        private void CampoProdutos_Loaded(object sender, RoutedEventArgs e)
        {
            double total = 0, maximo = espacoParaProdutos.ActualHeight;
            var produtosNestaPagina = ContextoGeral._DadosProdutos.TakeWhile(x =>
            {
                var item = new PartesDANFE.ItemProduto() { DataContext = x };
                item.Measure(new Windows.Foundation.Size(PartesDANFE.DimensoesPadrao.LarguraTotalStatic, espacoParaProdutos.ActualHeight));
                total += item.DesiredSize.Height;
                return total <= maximo;
            });
            ((FrameworkElement)sender).DataContext = produtosNestaPagina.ToArray();
            if (ContextoGeral._DadosProdutos.Length - produtosNestaPagina.Count() > 0)
            {
                var produtosRestantes = ContextoGeral._DadosProdutos.Except(produtosNestaPagina);
                PaiPaginas.Add(new PaginaExtra(produtosRestantes, infoAdicional.CampoObservacoes, PaiPaginas));
            }
        }
    }
}
