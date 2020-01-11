using BaseGeral.ModeloXML.PartesDetalhes;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos;

namespace Venda.Impostos.DetalhamentoICMS.DadosSN
{
    public class TipoNT : BaseSN
    {
        public override object Processar(DetalhesProdutos prod)
        {
            return new ICMSSN102(Origem, CSOSN);
        }
    }
}
