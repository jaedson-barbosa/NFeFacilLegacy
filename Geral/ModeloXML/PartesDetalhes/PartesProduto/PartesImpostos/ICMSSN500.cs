namespace BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos
{
    public class ICMSSN500 : ComumICMS, ISimplesNacional
    {
        public ICMSSN500(int origem, string csosn, string vBCSTRet, string vICMSSTRet) : base(origem, csosn, true)
        {
            this.vBCSTRet = vBCSTRet;
            this.vICMSSTRet = vICMSSTRet;
        }
    }
}
