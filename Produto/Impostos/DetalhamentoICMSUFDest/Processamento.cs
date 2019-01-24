using BaseGeral.ModeloXML.PartesDetalhes;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto;
using Windows.UI.Xaml.Controls;

namespace Venda.Impostos.DetalhamentoICMSUFDest
{
    sealed class Processamento : ProcessamentoImposto
    {
        IDadosICMSUFDest dados;

        public override IImposto[] Processar(DetalhesProdutos prod)
        {
            var imposto = dados.Imposto;
            return new IImposto[1] { imposto };
        }

        public override void ProcessarDados(Page Tela)
        {
            if (Detalhamento is Detalhamento detalhamento && Tela?.GetType() == typeof(Detalhar))
            {
                dados = (IDadosICMSUFDest)Tela;
            }
        }
    }
}
