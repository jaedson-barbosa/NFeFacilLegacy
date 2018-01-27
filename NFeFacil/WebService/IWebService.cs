namespace NFeFacil.WebService
{
    internal interface IWebService
    {
        string ConsultarProducao { get; }
        string AutorizarProducao { get; }
        string RespostaAutorizarProducao { get; }
        string RecepcaoEventoProducao { get; }
        string InutilizacaoProducao { get; }

        string ConsultarHomologacao { get; }
        string AutorizarHomologacao { get; }
        string RespostaAutorizarHomologacao { get; }
        string RecepcaoEventoHomologacao { get; }
        string InutilizacaoHomologacao { get; }
    }

    internal interface IWebServiceProducaoNFCe
    {
        string ConsultarProducaoNFCe { get; }
        string AutorizarProducaoNFCe { get; }
        string RespostaAutorizarProducaoNFCe { get; }
        string RecepcaoEventoProducaoNFCe { get; }
        string InutilizacaoProducaoNFCe { get; }
    }

    internal interface IWebServiceHomologacaoNFCe
    {
        string ConsultarHomologacaoNFCe { get; }
        string AutorizarHomologacaoNFCe { get; }
        string RespostaAutorizarHomologacaoNFCe { get; }
        string RecepcaoEventoHomologacaoNFCe { get; }
        string InutilizacaoHomologacaoNFCe { get; }
    }
}
