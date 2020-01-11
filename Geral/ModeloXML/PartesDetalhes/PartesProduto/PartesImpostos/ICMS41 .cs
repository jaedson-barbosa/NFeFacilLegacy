namespace BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesImpostos
{
    /// <summary>
    /// Grupo Tributação ICMS = 40, 41, 50.
    /// </summary>
    public class ICMS41 : ComumICMS, IRegimeNormal
    {
        public ICMS41()
        {
        }

        public ICMS41(int origem, string cst, string vICMSDeson, string motDesICMS) : base(origem, cst, false)
        {
            this.vICMSDeson = vICMSDeson;
            this.motDesICMS = motDesICMS;
        }
    }
}
