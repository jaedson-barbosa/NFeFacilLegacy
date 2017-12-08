using NFeFacil.Log;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.Impostos
{
    public interface IProcessamentoImposto
    {
        object Tela { set; }
        IDetalhamentoImposto Detalhamento { set; }
        bool ValidarEntradaDados(ILog log);
        bool ValidarDados(ILog log);
        Imposto[] Processar(ProdutoOuServico prod);
    }
}
