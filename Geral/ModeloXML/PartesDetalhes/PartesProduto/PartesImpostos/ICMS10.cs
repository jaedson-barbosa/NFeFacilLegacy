using static BaseGeral.ExtensoesPrincipal;
namespace BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos
{
    public class ICMS10 : ComumICMS, IRegimeNormal
    {
        public ICMS10(int origem, string cst, int modBC, double pICMS,
            int modBCST, string pMVAST, string pRedBCST, double pICMSST, DetalhesProdutos prod) : base(origem, cst, false)
        {
            var vBC = CalcularBC(prod);
            var vICMS = vBC * pICMS / 100;

            bool usarpMVAST = TryParse(pMVAST, out double pMVASTd);
            bool usarpRedBCST = TryParse(pRedBCST, out double pRedBCSTd);
            var vBCST = (vBC + ObterIPI(prod)) * (100 + pMVASTd) / 100;
            vBCST *= 1 - (pRedBCSTd / 100);

            var vICMSST = (vBCST * pICMSST / 100) - vICMS;

            this.modBC = modBC.ToString();
            this.vBC = ToStr(vBC);
            this.pICMS = ToStr(pICMS, "F4");
            this.vICMS = ToStr(vICMS);

            this.modBCST = modBCST.ToString();
            this.pMVAST = usarpMVAST ? ToStr(pMVASTd, "F4") : null;
            this.pRedBCST = usarpRedBCST ? ToStr(pRedBCSTd, "F4") : null;
            this.vBCST = ToStr(vBCST);
            this.pICMSST = ToStr(pICMSST, "F4");
            this.vICMSST = ToStr(vICMSST);
        }
    }
}
