namespace Fiscal.WebService
{
    internal interface IWebServiceHomologacaoNFCe
    {
        string ConsultarHomologacaoNFCe { get; }
        string AutorizarHomologacaoNFCe { get; }
        string RespostaAutorizarHomologacaoNFCe { get; }
        string RecepcaoEventoHomologacaoNFCe { get; }
        string InutilizacaoHomologacaoNFCe { get; }
    }
}
