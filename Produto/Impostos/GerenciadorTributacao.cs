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

        public async Task<DetalhesProdutos> AplicarTributacaoAutomatica(bool definirTotalImpostos = true)
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
            var produto = roteiro.Finalizar();
            if (definirTotalImpostos)
            {
                var caixa = new DefinirTotalImpostos();
                if (await caixa.ShowAsync() == ContentDialogResult.Primary
                    && caixa.ValorTotalTributos != 0)
                    produto.Impostos.vTotTrib = ExtensoesPrincipal.ToStr(caixa.ValorTotalTributos);
                else
                    produto.Impostos.vTotTrib = null;
            }
            else
                produto.Impostos.vTotTrib = null;
            return produto;
        }

        public void AplicarTributacaoManual()
        {
            BasicMainPage.Current.Navegar<EscolhaImpostos>(Produto);
        }
    }
}
