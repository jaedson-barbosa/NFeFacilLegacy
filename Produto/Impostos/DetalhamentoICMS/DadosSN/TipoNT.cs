using BaseGeral.ModeloXML.PartesDetalhes;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos;

namespace NFeFacil.Produto.Impostos.DetalhamentoICMS.DadosSN
{
    public class TipoNT : BaseSN
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
