using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;

namespace NFeFacil.ViewNFe.Impostos.DetalhamentoICMS.DadosSN
{
    class TipoNT : BaseSN
    {
        public override object Processar(DetalhesProdutos prod)
        {
            return new ICMSSN102()
            {
                CSOSN = CSOSN,
                Orig = Origem
            };
        }
    }
}
