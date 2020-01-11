using BaseGeral.ModeloXML.PartesDetalhes;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos;
using static BaseGeral.ExtensoesPrincipal;

namespace Venda.Impostos.DetalhamentoICMS.DadosRN
{
    public class Tipo10 : BaseRN
    {
        public int modBC { get; set; }
        public double pICMS { get; set; }

        public int modBCST { get; set; }
        public string pMVAST { get; set; }
        public string pRedBCST { get; set; }
        public double pICMSST { get; set; }

        public Tipo10() { }
        public Tipo10(TelasRN.Tipo10 tela)
        {
            modBC = tela.modBC;
            pICMS = tela.pICMS;

            modBCST = tela.modBCST;
            pMVAST = tela.pMVAST;
            pRedBCST = tela.pRedBCST;
            pICMSST = tela.pICMSST;
        }

        public override object Processar(DetalhesProdutos prod)
        {
            return new ICMS10(Origem, CST, modBC, pICMS, modBCST, pMVAST, pRedBCST, pICMSST, prod);
        }
    }
}
