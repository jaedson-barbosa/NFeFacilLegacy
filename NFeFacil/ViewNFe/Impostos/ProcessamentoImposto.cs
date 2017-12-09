using NFeFacil.Log;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using System.Globalization;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.Impostos
{
    public abstract class ProcessamentoImposto
    {
        protected static CultureInfo CulturaPadrao = CultureInfo.InvariantCulture;

        public object Tela { protected get; set; }
        public IDetalhamentoImposto Detalhamento { protected get; set; }

        public abstract bool ValidarEntradaDados(ILog log);
        public abstract bool ValidarDados(ILog log);
        public abstract Imposto[] Processar(ProdutoOuServico prod);
    }
}
