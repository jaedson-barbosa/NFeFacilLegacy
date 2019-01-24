using BaseGeral.ModeloXML.PartesDetalhes;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos;
using static BaseGeral.ExtensoesPrincipal;

namespace Venda.Impostos.DetalhamentoICMS.DadosRN
{
    public class Tipo30 : BaseRN
    {
        public int modBCST { get; set; }
        public double pICMSST { get; set; }
        public string pMVAST { get; set; }
        public string pRedBCST { get; set; }
        public string motDesICMS { get; set; }

        public Tipo30() { }
        public Tipo30(TelasRN.Tipo30 tela)
        {
            modBCST = tela.modBCST;
            pMVAST = tela.pMVAST;
            pRedBCST = tela.pRedBCST;
            pICMSST = tela.pICMSST;
            motDesICMS = tela.motDesICMS;
        }

        public override object Processar(DetalhesProdutos prod)
        {
            return new ICMS30(Origem, CST, modBCST, pMVAST, pRedBCST, pICMSST, motDesICMS, prod);
        }
    }
}
