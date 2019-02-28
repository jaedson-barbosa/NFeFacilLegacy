namespace BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos
{
    /// <summary>
    /// Grupo de Partilha do ICMS entre a UF de origem e UF de destino ou a UF definida na legislação. 
    /// </summary>
    public class ICMSPart : ComumICMS, IRegimeNormal
    {
        public ICMSPart()
        {
        }

        public ICMSPart(int origem, string cst, int modBC, string vBC, string pICMS, string vICMS,
            int modBCST, string pMVAST, string pRedBCST, string vBCST, string pICMSST, string vICMSST, string pRedBC, string pBCOp, string UFST)
            : base(origem, cst, false)
        {
            this.modBC = modBC.ToString();
            this.vBC = vBC;
            this.pICMS = pICMS;
            this.vICMS = vICMS;
            this.pRedBC = pRedBC;
            this.modBCST = modBCST.ToString();
            this.pMVAST = pMVAST;
            this.pRedBCST = pRedBCST;
            this.vBCST = vBCST;
            this.pICMSST = pICMSST;
            this.vICMSST = vICMSST;

            this.pBCOp = pBCOp;
            this.UFST = UFST;
        }
    }
}
