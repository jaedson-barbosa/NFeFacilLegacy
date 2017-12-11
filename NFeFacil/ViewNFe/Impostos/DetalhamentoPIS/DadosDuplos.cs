using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.Impostos.DetalhamentoPIS
{
    abstract class DadosDuplos : DadosPIS
    {
        public double Aliquota { protected get; set; }
        public double Valor { protected get; set; }
        public TiposCalculo TipoCalculo { protected get; set; }

        public override abstract object Processar(ProdutoOuServico prod);
    }
}
