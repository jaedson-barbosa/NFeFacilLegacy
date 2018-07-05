namespace Fiscal.WebService
{
    internal interface IWebServiceProducaoNFCe
    {
        string ConsultarProducaoNFCe { get; }
        string AutorizarProducaoNFCe { get; }
        string RespostaAutorizarProducaoNFCe { get; }
        string RecepcaoEventoProducaoNFCe { get; }
        string InutilizacaoProducaoNFCe { get; }
    }
}
