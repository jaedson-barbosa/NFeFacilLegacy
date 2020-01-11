using static BaseGeral.ExtensoesPrincipal;
namespace BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos
{
    public class ICMSSN201 : ComumICMS, ISimplesNacional
    {
        public ICMSSN201()
        {
        }

        public ICMSSN201(int origem, string csosn, int modBCST, string pMVAST, string pRedBCST, double pICMSST, string pCredSN, string vCredICMSSN, DetalhesProdutos prod) : base(origem, csosn, true)
        {
            bool usarpMVAST = TryParse(pMVAST, out double pMVASTd);
            bool usarpRedBCST = TryParse(pRedBCST, out double pRedBCSTd);
            var vBCST = CalcularBC(prod) * (100 + pMVASTd) / 100;
            vBCST *= 1 - (pRedBCSTd / 100);
            var vICMSST = vBCST * pICMSST / 100;


            this.modBCST = modBCST.ToString();
            this.pMVAST = usarpMVAST ? ToStr(pMVASTd, "F4") : null;
            this.pRedBCST = usarpRedBCST ? ToStr(pRedBCSTd, "F4") : null;
            this.vBCST = ToStr(vBCST);
            this.pICMSST = ToStr(pICMSST, "F4");
            this.vICMSST = ToStr(vICMSST);
            this.pCredSN = pCredSN;
            this.vCredICMSSN = vCredICMSSN;
        }
    }
}
