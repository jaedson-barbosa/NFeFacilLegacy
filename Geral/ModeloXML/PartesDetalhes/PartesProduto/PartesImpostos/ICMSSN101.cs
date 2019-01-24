namespace BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos
{
    public class ICMSSN101 : ComumICMS, ISimplesNacional
    {
        public ICMSSN101(int origem, string csosn, string pCredSN, string vCredICMSSN) : base(origem, csosn, true)
        {
            this.pCredSN = pCredSN;
            this.vCredICMSSN = vCredICMSSN;
        }
    }
}
