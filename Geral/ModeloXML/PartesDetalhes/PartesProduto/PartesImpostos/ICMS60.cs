namespace BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos
{
    public class ICMS60 : ComumICMS, IRegimeNormal
    {
        public ICMS60(int origem, string cst, string vBCSTRet, string pST, string vICMSSTRet) : base(origem, cst, false)
        {
            this.vBCSTRet = vBCSTRet;
            this.pST = pST;
            this.vICMSSTRet = vICMSSTRet;
        }
    }
}
