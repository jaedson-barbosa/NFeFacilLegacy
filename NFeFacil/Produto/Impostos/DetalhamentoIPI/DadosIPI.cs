using NFeFacil.ModeloXML.PartesDetalhes.PartesProduto;
using NFeFacil.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos;

namespace NFeFacil.Produto.Impostos.DetalhamentoIPI
{
    abstract class DadosIPI
    {
        public string CST { protected get; set; }
        public IPI PreImposto { get; set; }
        public abstract object Processar(ProdutoOuServico prod);
    }
}
