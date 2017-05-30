using NFeFacil.DANFE.Pacotes;
using NFeFacil.DANFE.Processamento;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace NFeFacil.View.PaginasDANFE
{
    public sealed partial class PaginaUnica : UserControl
    {
        double LarguraPagina => CentimeterToPixel(21);
        double AlturaPagina => CentimeterToPixel(29.7);

        Thickness MargemPadrao => new Thickness(CentimeterToPixel(1));

        DadosCabecalho ContextoCanhoto { get; }
        DadosAdicionais ContextoDadosAdicionais { get; }
        DadosCliente ContextoCliente { get; }
        DadosImposto ContextoImposto { get; }
        DadosMotorista ContextoTransporte { get; }

        public PaginaUnica(BibliotecaCentral.ModeloXML.Processo processo)
        {
            this.InitializeComponent();
            var geral = ViewDados.Converter(processo);
            ContextoCanhoto = geral._DadosCabecalho;
            ContextoDadosAdicionais = geral._DadosAdicionais;
            ContextoCliente = geral._DadosCliente;
            ContextoImposto = geral._DadosImposto;
            ContextoTransporte = geral._DadosMotorista;
        }

        static double CentimeterToPixel(double Centimeter)
        {
            const double fator = 96 / 2.54;
            return Centimeter * fator;
        }
    }
}
