using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;

namespace NFeFacil.ViewNFe.Impostos.DetalhamentoIPI
{
    abstract class DadosIPI
    {
        public string CST { protected get; set; }
        public IPI PreImposto { get; set; }
        public abstract object Processar(ProdutoOuServico prod);
    }
}
