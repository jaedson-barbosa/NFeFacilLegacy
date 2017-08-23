using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using static NFeFacil.ExtensoesPrincipal;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewRegistroVenda
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class DARV : Page
    {
        public DARV()
        {
            this.InitializeComponent();
            DefinirTamanho(19, 27.7);
            paiGeral.Margin = new Thickness(CentimeterToPixel(1));
        }

        void DefinirTamanho(double largura, double altura)
        {
            paiGeral.Width = CentimeterToPixel(largura - 2);
            paiGeral.Height = CentimeterToPixel(altura - 2);
        }
    }

    public struct ConjuntoDadosDARV
    {
        public DadosEmitente Emitente { get; set; }
        public PacotesImpressaoGenericos.DadosCliente Cliente { get; set; }
        public string DataVenda { get; set; }
        public string IdVenda { get; set; }
        public string Vendedor { get; set; }
        public string CPFVendedor { get; set; }
        public string Motorista { get; set; }
        public DadosProduto[] Produtos { get; set; }
        public string Desconto { get; set; }
        public string Total { get; set; }
        public string Observações { get; set; }
        public string ChaveNFeRelacionada { get; set; }
    }

    public struct DadosEmitente
    {
        public string NomeFicticio { get; set; }
        public PacotesImpressaoGenericos.Endereço Endereco { get; set; }
        public string Email { get; set; }
    }

    public struct DadosProduto
    {
        public string Descricao { get; set; }
        public string Quantidade { get; set; }
        public string Valor { get; set; }
        public string Total { get; set; }
    }
}
