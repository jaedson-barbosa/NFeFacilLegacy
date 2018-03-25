namespace Fiscal.WebService
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
}
