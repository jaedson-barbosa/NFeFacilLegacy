using BaseGeral.ModeloXML.PartesDetalhes;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos;
using BaseGeral.View;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Venda.Impostos.DetalhamentoIPI
{
    [DetalhePagina("IPI")]
    public sealed partial class DetalharSimples : UserControl, IProcessamentoImposto
    {
        public readonly IPI Conjunto = new IPI();
        readonly Detalhamento Detalhamento;
        public PrincipaisImpostos Tipo => PrincipaisImpostos.IPI;

        public DetalharSimples(Detalhamento detalhamento)
        {
            Detalhamento = detalhamento;
            InitializeComponent();
        }

        public IImposto[] Processar(DetalhesProdutos prod)
        {
            var resultado = new DadosNT
            {
                CST = Detalhamento.CST.ToString("00"),
                PreImposto = Conjunto
            }.Processar(prod.Produto);
            return new IImposto[1] { (IPI)resultado };
        }
    }
}
