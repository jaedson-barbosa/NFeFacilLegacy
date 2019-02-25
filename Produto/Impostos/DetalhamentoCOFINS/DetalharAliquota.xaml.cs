using BaseGeral.ModeloXML.PartesDetalhes;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos;
using BaseGeral.View;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Venda.Impostos.DetalhamentoCOFINS
{
    [DetalhePagina("COFINS")]
    public sealed partial class DetalharAliquota : UserControl, IProcessamentoImposto
    {
        double Aliquota;
        readonly Detalhamento Detalhamento;
        public PrincipaisImpostos Tipo => PrincipaisImpostos.COFINS;

        public DetalharAliquota(Detalhamento detalhamento)
        {
            Detalhamento = detalhamento;
            InitializeComponent();
        }

        public IImposto[] Processar(DetalhesProdutos prod)
        {
            var resultado = new DadosAliq()
            {
                Aliquota = Aliquota,
                CST = Detalhamento.CST.ToString("00")
            }.Processar(prod.Produto);
            if (resultado is IImposto[] list) return list;
            else return new IImposto[1] { (COFINS)resultado };
        }
    }
}
