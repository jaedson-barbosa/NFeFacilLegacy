using BaseGeral;
using BaseGeral.ModeloXML.PartesDetalhes;
using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Venda.Impostos
{
    public sealed class GerenciadorTributacao
    {
        DadosAdicaoProduto Produto { get; }

        public GerenciadorTributacao(DadosAdicaoProduto produto)
        {
            Produto = produto;
        }

        public async Task<DetalhesProdutos> AplicarTributacaoAutomatica()
        {
            var icms = Produto.Auxiliar.GetICMSArmazenados();
            var imps = Produto.Auxiliar.GetImpSimplesArmazenados();

            var padrao = Produto.ImpostosPadrao;
            var detalhamentos = new IDetalhamentoImposto[padrao.Length];
            for (int i = 0; i < padrao.Length; i++)
            {
                var (Tipo, NomeTemplate, CST) = padrao[i];
                ImpostoArmazenado impPronto;
                if (Tipo == PrincipaisImpostos.ICMS) impPronto = icms.First(Analisar);
                else impPronto = imps.First(Analisar);
                bool Analisar(ImpostoArmazenado x) => x.Tipo == Tipo && x.NomeTemplate == NomeTemplate && x.CST == CST;
                detalhamentos[i] = impPronto;
            }
            var roteiro = new RoteiroAdicaoImpostos(detalhamentos, Produto.Completo);
            while (roteiro.Avancar()) roteiro.ProcessarSalvo();

            var produto = roteiro.Finalizar();
            var caixa = new DefinirTotalImpostos();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary && !string.IsNullOrEmpty(caixa.ValorTotalTributos))
            {
                produto.Impostos.vTotTrib = caixa.ValorTotalTributos;
            }
            else
            {
                produto.Impostos.vTotTrib = null;
            }
            return produto;
        }

        public void AplicarTributacaoManual()
        {
            BasicMainPage.Current.Navegar<EscolhaImpostos>(Produto);
        }
    }
}
