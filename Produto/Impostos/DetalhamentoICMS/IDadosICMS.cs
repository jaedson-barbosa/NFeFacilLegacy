using BaseGeral.ModeloXML.PartesDetalhes;

namespace Produto.Impostos.DetalhamentoICMS
{
    interface IDadosICMS
    {
        object Processar(DetalhesProdutos prod);
    }
}
