using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;

namespace NFeFacil.Produto.Impostos.DetalhamentoICMS
{
    interface IDadosICMS
    {
        object Processar(DetalhesProdutos prod);
    }
}
