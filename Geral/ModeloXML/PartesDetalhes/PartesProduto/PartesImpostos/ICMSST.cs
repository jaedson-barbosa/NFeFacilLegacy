namespace BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos
{
    public sealed class ICMSST : ComumICMS, IRegimeNormal
    {
        public ICMSST(int origem, string cst, string vBCSTRet, string vICMSSTRet, string vBCSTDest, string vICMSSTDest) : base(origem, cst, false)
        {
            this.vBCSTRet = vBCSTRet;
            this.vICMSSTRet = vICMSSTRet;
            this.vBCSTDest = vBCSTDest;
            this.vICMSSTDest = vICMSSTDest;
        }
    }
}
