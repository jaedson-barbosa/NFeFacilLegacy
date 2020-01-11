using BaseGeral.ModeloXML.PartesDetalhes;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos;
using static BaseGeral.ExtensoesPrincipal;

namespace Venda.Impostos.DetalhamentoICMS.DadosRN
{
    public class Tipo0 : BaseRN
    {
        public int modBC { get; set; }
        public double pICMS { get; set; }

        public Tipo0() { }
        public Tipo0(TelasRN.Tipo0 tela)
        {
            modBC = tela.modBC;
            pICMS = tela.pICMS;
        }

        public override object Processar(DetalhesProdutos prod)
        {
            return new ICMS00(Origem, CST, modBC, pICMS, prod);
        }
    }
}
