using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos;

namespace Venda.Impostos.DetalhamentoIPI
{
    abstract class DadosIPI
    {
        public string CST { protected get; set; }
        public IPI PreImposto { get; set; }
        public abstract object Processar(ProdutoOuServico prod);
    }
}
