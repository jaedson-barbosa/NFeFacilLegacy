using BaseGeral.ModeloXML.PartesDetalhes;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos;
using BaseGeral.View;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Venda.Impostos.DetalhamentoPIS
{
    [DetalhePagina("PIS")]
    public sealed partial class DetalharQtde : UserControl, IProcessamentoImposto
    {
        double Valor;
        readonly Detalhamento Detalhamento;
        public PrincipaisImpostos Tipo => PrincipaisImpostos.PIS;

        public DetalharQtde(Detalhamento detalhamento)
        {
            Detalhamento = detalhamento;
            InitializeComponent();
        }

        public IImposto[] Processar(DetalhesProdutos prod)
        {
            var resultado = new DadosQtde()
            {
                Valor = Valor,
                CST = Detalhamento.CST.ToString("00")
            }.Processar(prod.Produto);
            if (resultado is IImposto[] list) return list;
            else return new IImposto[1] { (PIS)resultado };
        }
    }
}
