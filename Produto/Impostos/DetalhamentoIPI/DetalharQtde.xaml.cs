using BaseGeral.ModeloXML.PartesDetalhes;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos;
using BaseGeral.View;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Venda.Impostos.DetalhamentoIPI
{
    [DetalhePagina("IPI")]
    public sealed partial class DetalharQtde : UserControl, IProcessamentoImposto
    {
        readonly IPI Conjunto = new IPI();
        double ValorUnitario;
        readonly Detalhamento Detalhamento;
        public PrincipaisImpostos Tipo => PrincipaisImpostos.IPI;

        public DetalharQtde(Detalhamento detalhamento)
        {
            Detalhamento = detalhamento;
            InitializeComponent();
        }

        public IImposto[] Processar(DetalhesProdutos prod)
        {
            var resultado = new DadosTrib()
            {
                CST = Detalhamento.CST.ToString("00"),
                Valor = ValorUnitario,
                PreImposto = Conjunto,
                TipoCalculo = TiposCalculo.PorValor
            }.Processar(prod.Produto);
            return new IImposto[1] { (IPI)resultado };
        }
    }
}
