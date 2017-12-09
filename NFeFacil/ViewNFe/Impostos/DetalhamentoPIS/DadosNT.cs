using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.Impostos.DetalhamentoPIS
{
    sealed class DadosNT : DadosPIS
    {
        public override object Processar(ProdutoOuServico prod)
        {
            return new PIS
            {
                Corpo = new PISNT()
                {
                    CST = CST
                }
            };
        }
    }
}
