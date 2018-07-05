using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Venda.Impostos.DetalhamentoCOFINS
{
    interface IDadosCOFINS
    {
        string CST { set; }
        object Processar(ProdutoOuServico prod);
    }
}
