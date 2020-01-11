namespace BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos
{
    public class ICMS90 : ComumICMS, IRegimeNormal
    {
        public ICMS90()
        {
        }

        public ICMS90(int origem, string cst, int modBC, string vBC, string pICMS, string vICMS,
            int modBCST, string pMVAST, string pRedBCST, string vBCST, string pICMSST, string vICMSST, string pRedBC, string vICMSDeson, string motDesICMS)
            : base(origem, cst, false)
        {
            this.modBC = modBC == -1 ? null : modBC.ToString();;
            this.vBC = vBC;
            this.pICMS = pICMS;
            this.vICMS = vICMS;
            this.pRedBC = pRedBC;
            this.modBCST = modBCST == -1 ? null : modBCST.ToString();
            this.pMVAST = pMVAST;
            this.pRedBCST = pRedBCST;
            this.vBCST = vBCST;
            this.pICMSST = pICMSST;
            this.vICMSST = vICMSST;

            this.vICMSDeson = vICMSDeson;
            this.motDesICMS = motDesICMS;
        }
    }
}
