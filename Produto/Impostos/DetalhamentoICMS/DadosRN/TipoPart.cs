using BaseGeral.ModeloXML.PartesDetalhes;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos;

namespace Produto.Impostos.DetalhamentoICMS.DadosRN
{
    public class TipoPart : BaseRN
    {
        public string vICMSDeson { get; set; }
        public string motDesICMS { get; set; }
        public int modBC { get; set; }
        public string vBC { get; set; }
        public string pRedBC { get; set; }
        public string pICMS { get; set; }
        public string vICMS { get; set; }

        public string pBCOp { get; set; }
        public string UFST { get; set; }
        public int modBCST { get; set; }
        public string pMVAST { get; set; }
        public string pRedBCST { get; set; }
        public string vBCST { get; set; }
        public string pICMSST { get; set; }
        public string vICMSST { get; set; }

        public TipoPart() { }
        public TipoPart(TelasRN.TipoPart tela)
        {
            vICMSDeson = tela.vICMSDeson;
            motDesICMS = tela.motDesICMS;
            modBC = tela.modBC;
            vBC = tela.vBC;
            pRedBC = tela.pRedBC;
            pICMS = tela.pICMS;
            vICMS = tela.vICMS;

            pBCOp = tela.pBCOp;
            UFST = tela.UFST;
            modBCST = tela.modBCST;
            pMVAST = tela.pMVAST;
            pRedBCST = tela.pRedBCST;
            vBCST = tela.vBCST;
            pICMSST = tela.pICMSST;
            vICMSST = tela.vICMSST;
        }

        public override object Processar(DetalhesProdutos prod)
        {
            return new ICMSPart()
            {
                CST = "90",
                modBC = modBC.ToString(),
                modBCST = modBCST.ToString(),
                Orig = Origem,
                pICMS = pICMS,
                pICMSST = pICMSST,
                pMVAST = pMVAST,
                pRedBC = pRedBC,
                pRedBCST = pRedBCST,
                vBC = vBC,
                vBCST = vBCST,
                vICMS = vICMS,
                vICMSST = vICMSST,
                pBCOp = pBCOp,
                UFST = UFST
            };
        }
    }
}
