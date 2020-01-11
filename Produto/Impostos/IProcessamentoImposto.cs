using BaseGeral.ModeloXML.PartesDetalhes;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Venda.Impostos
{
    public interface IProcessamentoImposto
    {
        PrincipaisImpostos Tipo { get; }
        IImposto[] Processar(DetalhesProdutos prod);
    }
}
