// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Produto.Impostos.DetalhamentoICMS
{
    public struct Detalhamento : IDetalhamentoImposto
    {
        public PrincipaisImpostos Tipo => PrincipaisImpostos.ICMS;
        public string TipoICMSSN { get; set; }
        public string TipoICMSRN { get; set; }
        public int Origem { get; set; }
    }
}
