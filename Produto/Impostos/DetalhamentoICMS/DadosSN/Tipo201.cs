using BaseGeral.ModeloXML.PartesDetalhes;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos;
using static BaseGeral.ExtensoesPrincipal;

namespace Venda.Impostos.DetalhamentoICMS.DadosSN
{
    public class Tipo201 : BaseSN
    {
        public int modBCST { get; set; }
        public string pMVAST { get; set; }
        public string pRedBCST { get; set; }
        public double pICMSST { get; set; }

        public string pCredSN { get; set; }
        public string vCredICMSSN { get; set; }

        public Tipo201() { }
        public Tipo201(TelasSN.Tipo201 tela)
        {
            modBCST = tela.modBCST;
            pMVAST = tela.pMVAST;
            pRedBCST = tela.pRedBCST;
            pICMSST = tela.pICMSST;

            pCredSN = tela.pCredSN;
            vCredICMSSN = tela.vCredICMSSN;
        }

        public override object Processar(DetalhesProdutos prod)
        {
            return new ICMSSN201(Origem, CSOSN, modBCST, pMVAST, pRedBCST, pICMSST, pCredSN, vCredICMSSN, prod);
        }
    }
}
