using static BaseGeral.ExtensoesPrincipal;
namespace BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos
{
    public class ICMS70 : ComumICMS, IRegimeNormal
    {
        public ICMS70()
        {
        }

        public ICMS70(int origem, string cst, int modBC, double pICMS, double pRedBC,
            int modBCST, string pMVAST, string pRedBCST, double pICMSST, string motDesICMS, DetalhesProdutos prod)
            : base(origem, cst, false)
        {
            var vBC = CalcularBC(prod);
            var bcSemReducao = vBC * pICMS / 100;
            vBC *= 1 - (pRedBC / 100);
            var vICMS = vBC * pICMS / 100;

            bool usarpMVAST = TryParse(pMVAST, out double pMVASTd);
            bool usarpRedBCST = TryParse(pRedBCST, out double pRedBCSTd);
            var vBCST = (vBC + ObterIPI(prod)) * (100 + pMVASTd) / 100;
            var bcstSemReducao = (vBCST * pICMSST / 100) - vICMS;

            vBCST *= 1 - (pRedBCSTd / 100);

            var vICMSST = (vBCST * pICMSST / 100) - vICMS;

            var vICMSDeson = (bcSemReducao - vICMS) + (bcstSemReducao - vICMSST);
            bool infDeson = vICMSDeson > 0 && !string.IsNullOrEmpty(motDesICMS);

            this.modBC = modBC.ToString();
            this.vBC = ToStr(vBC);
            this.pICMS = ToStr(pICMS, "F4");
            this.vICMS = ToStr(vICMS);
            this.pRedBC = ToStr(pRedBC, "F4");
            this.modBCST = modBCST.ToString();
            this.pMVAST = usarpMVAST ? ToStr(pMVASTd, "F4") : null;
            this.pRedBCST = usarpRedBCST ? ToStr(pRedBCSTd, "F4") : null;
            this.vBCST = ToStr(vBCST);
            this.pICMSST = ToStr(pICMSST, "F4");
            this.vICMSST = ToStr(vICMSST);

            this.vICMSDeson = infDeson ? ToStr(vICMSDeson) : null;
            this.motDesICMS = infDeson ? motDesICMS : null;
        }
    }
}
