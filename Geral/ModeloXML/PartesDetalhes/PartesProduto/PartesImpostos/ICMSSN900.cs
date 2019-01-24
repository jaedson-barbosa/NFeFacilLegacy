namespace BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos
{
    public class ICMSSN900 : ComumICMS, ISimplesNacional
    {
        public ICMSSN900(int origem, string csosn, int modBC, string vBC, string pRedBC, string pICMS, string vICMS,
            int modBCST, string pMVAST, string pRedBCST, string vBCST, string pICMSST, string vICMSST, string pCredSN, string vCredICMSSN)
            : base(origem, csosn, true)
        {
            this.modBC = modBC.ToString();
            this.vBC = vBC;
            this.pRedBC = pRedBC;
            this.pICMS = pICMS;
            this.vICMS = vICMS;
            this.modBCST = modBCST.ToString();
            this.pMVAST = pMVAST;
            this.pRedBCST = pRedBCST;
            this.vBCST = vBCST;
            this.pICMSST = pICMSST;
            this.vICMSST = vICMSST;
            this.pCredSN = pCredSN;
            this.vCredICMSSN = vCredICMSSN;
        }
    }
}
