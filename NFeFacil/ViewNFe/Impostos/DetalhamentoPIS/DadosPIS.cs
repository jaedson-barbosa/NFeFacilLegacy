using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using System.Globalization;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.Impostos.DetalhamentoPIS
{
    abstract class DadosPIS
    {
        public string CST { protected get; set; }
        public abstract object Processar(ProdutoOuServico prod);
        protected static CultureInfo CulturaPadrao = CultureInfo.InvariantCulture;
    }
}
