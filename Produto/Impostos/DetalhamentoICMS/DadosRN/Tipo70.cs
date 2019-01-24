using BaseGeral.ModeloXML.PartesDetalhes;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos;
using static BaseGeral.ExtensoesPrincipal;

namespace Venda.Impostos.DetalhamentoICMS.DadosRN
{
    public class Tipo70 : BaseRN
    {
        public int modBC { get; set; }
        public double pICMS { get; set; }
        public double pRedBC { get; set; }

        public int modBCST { get; set; }
        public string pMVAST { get; set; }
        public string pRedBCST { get; set; }
        public double pICMSST { get; set; }

        public string motDesICMS { get; set; }

        public Tipo70() { }
        public Tipo70(TelasRN.Tipo70 tela)
        {
            modBC = tela.modBC;
            pRedBC = tela.pRedBC;
            pICMS = tela.pICMS;

            modBCST = tela.modBCST;
            pMVAST = tela.pMVAST;
            pRedBCST = tela.pRedBCST;
            pICMSST = tela.pICMSST;

            motDesICMS = tela.motDesICMS;
        }

        public override object Processar(DetalhesProdutos prod)
        {
            return new ICMS70(Origem, CST, modBC, pICMS, pRedBC, modBCST, pMVAST, pRedBCST, pICMSST, motDesICMS, prod);
        }
    }
}
