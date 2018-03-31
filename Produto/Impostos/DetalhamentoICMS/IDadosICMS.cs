using BaseGeral.ModeloXML.PartesDetalhes;

namespace Venda.Impostos.DetalhamentoICMS
{
    interface IDadosICMS
    {
        object Processar(DetalhesProdutos prod);
    }
}
