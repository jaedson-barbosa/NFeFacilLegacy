using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos;

namespace Produto.Impostos.DetalhamentoIPI
{
    class DadosNT : DadosIPI
    {
        public override object Processar(ProdutoOuServico prod)
        {
            var corpo = (IPINT)PreImposto.Corpo;
            corpo.CST = CST;
            return PreImposto;
        }
    }
}
