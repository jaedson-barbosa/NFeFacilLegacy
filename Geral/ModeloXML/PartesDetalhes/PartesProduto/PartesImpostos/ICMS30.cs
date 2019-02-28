using static BaseGeral.ExtensoesPrincipal;
namespace BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos
{
    public class ICMS30 : ComumICMS, IRegimeNormal
    {
        public ICMS30()
        {
        }

        public ICMS30(int origem, string cst, int modBCST, string pMVAST, string pRedBCST, double pICMSST, string motDesICMS, DetalhesProdutos prod)
            : base(origem, cst, false)
        {
            bool usarpMVAST = TryParse(pMVAST, out double pMVASTd);
            bool usarpRedBCST = TryParse(pRedBCST, out double pRedBCSTd);
            var vBCST = (CalcularBC(prod) + ObterIPI(prod)) * (100 + pMVASTd) / 100;
            var valorSemReducao = vBCST * pICMSST / 100;
            vBCST *= 1 - (pRedBCSTd / 100);
            var vICMSST = vBCST * pICMSST / 100;

            var vICMSDeson = valorSemReducao - vICMSST;
            bool infDeson = vICMSDeson > 0 && !string.IsNullOrEmpty(motDesICMS);

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
