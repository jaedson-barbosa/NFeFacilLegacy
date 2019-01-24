using BaseGeral.ModeloXML.PartesDetalhes;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos;
using static BaseGeral.ExtensoesPrincipal;

namespace Venda.Impostos.DetalhamentoICMS.DadosRN
{
    public class Tipo20 : BaseRN
    {
        public int modBC { get; set; }
        public double pICMS { get; set; }

        public double pRedBC { get; set; }
        public string motDesICMS { get; set; }

        public Tipo20() { }
        public Tipo20(TelasRN.Tipo20 tela)
        {
            modBC = tela.modBC;
            pRedBC = tela.pRedBC;
            pICMS = tela.pICMS;
            motDesICMS = tela.motDesICMS;
        }

        public override object Processar(DetalhesProdutos prod)
        {
            return new ICMS20(Origem, CST, modBC, pICMS, pRedBC, motDesICMS, prod);
        }
    }
}
