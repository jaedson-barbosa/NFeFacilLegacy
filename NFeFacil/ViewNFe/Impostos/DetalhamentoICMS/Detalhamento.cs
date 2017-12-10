// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe.Impostos.DetalhamentoICMS
{
    public struct Detalhamento : IDetalhamentoImposto
    {
        public CaixasImpostos.EscolherTipoICMS.Regimes Regime { get; set; }
        public PrincipaisImpostos Tipo => PrincipaisImpostos.ICMS;
        public string TipoICMSSN { get; set; }
        public string TipoICMSRN { get; set; }
        public int Origem { get; set; }
    }
}
