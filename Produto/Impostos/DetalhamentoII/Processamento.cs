using BaseGeral.ModeloXML.PartesDetalhes;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos;
using Windows.UI.Xaml.Controls;

namespace Venda.Impostos.DetalhamentoII
{
    sealed class Processamento : ProcessamentoImposto
    {
        IDadosII dados;

        public override ImpostoBase[] Processar(DetalhesProdutos prod)
        {
            var imposto = dados.Imposto;
            return new ImpostoBase[1] { imposto };
        }

        public override void ProcessarDados(Page Tela)
        {
            if (Detalhamento is Detalhamento detalhamento && Tela?.GetType() == typeof(Detalhar))
            {
                dados = (IDadosII)Tela;
            }
        }
    }

    sealed class Dados : IDadosII
    {
        public II Imposto { get; set; }
    }
}
