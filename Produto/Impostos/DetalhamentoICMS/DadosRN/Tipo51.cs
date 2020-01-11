using BaseGeral.ModeloXML.PartesDetalhes;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos;
using static BaseGeral.ExtensoesPrincipal;

namespace Venda.Impostos.DetalhamentoICMS.DadosRN
{
    public class Tipo51 : BaseRN
    {
        public int modBC { get; set; }
        public double pICMS { get; set; }

        public double pRedBC { get; set; }
        public double pDif { get; set; }
        public bool Calcular { get; set; }

        public Tipo51() { }
        public Tipo51(TelasRN.Tipo51 tela)
        {
            modBC = tela.modBC;
            pRedBC = tela.pRedBC;
            pICMS = tela.pICMS;
            pDif = tela.pDif;
            Calcular = tela.Calcular;
        }

        public override object Processar(DetalhesProdutos prod)
        {
            if (Calcular)
            {
                return new ICMS51(Origem, CST, modBC, pICMS, pRedBC, pDif, prod);
            }
            else
            {
                return new ICMS51(Origem, CST);
            }
        }
    }
}
