using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.Impostos.DetalhamentoCOFINS
{
    abstract class DadosCOFINS
    {
        public string CST { protected get; set; }
        public abstract object Processar(ProdutoOuServico prod);
    }
}
