using BaseGeral.ModeloXML.PartesDetalhes;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos;
using static BaseGeral.ExtensoesPrincipal;

namespace Venda.Impostos.DetalhamentoICMS.DadosSN
{
    public class Tipo202 : BaseSN
    {
        public int modBCST { get; set; }
        public string pMVAST { get; set; }
        public string pRedBCST { get; set; }
        public double pICMSST { get; set; }

        public Tipo202() { }
        public Tipo202(TelasSN.Tipo202 tela)
        {
            modBCST = tela.modBCST;
            pMVAST = tela.pMVAST;
            pRedBCST = tela.pRedBCST;
            pICMSST = tela.pICMSST;
        }

        public override object Processar(DetalhesProdutos prod)
        {
            return new ICMSSN202(Origem, CSOSN, modBCST, pMVAST, pRedBCST, pICMSST, prod);
        }
    }
}
