using NFeFacil.Log;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.Impostos.DetalhamentoCOFINS
{
    abstract class DadosDuplos : IDadosCOFINS
    {
        public string CST { protected get; set; }
        public double Aliquota { protected get; set; }
        public double Valor { protected get; set; }
        public TiposCalculo TipoCalculo { protected get; set; }

        public abstract object Processar(ProdutoOuServico prod);
    }
}
