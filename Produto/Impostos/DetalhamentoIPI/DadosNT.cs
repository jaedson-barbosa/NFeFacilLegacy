using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos;

namespace Venda.Impostos.DetalhamentoIPI
{
    class DadosNT : DadosIPI
    {
        public override object Processar(ProdutoOuServico prod)
        {
            if (PreImposto.Corpo is IPINT corpo)
                corpo.CST = CST;
            else
                corpo = new IPINT(CST);
            return PreImposto;
        }
    }
}
