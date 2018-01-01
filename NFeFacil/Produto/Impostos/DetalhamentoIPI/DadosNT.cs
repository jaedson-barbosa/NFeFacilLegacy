using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;

namespace NFeFacil.Produto.Impostos.DetalhamentoIPI
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
