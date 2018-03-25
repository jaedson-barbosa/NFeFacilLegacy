namespace Fiscal.WebService.WebServices
{
    // Os web services são iguais
    internal struct GO : IWebService
    {
        public string ConsultarProducao => "https://nfe.sefaz.go.gov.br/nfe/services/v2/NfeConsulta2?wsdl";
        public string AutorizarProducao => "https://nfe.sefaz.go.gov.br/nfe/services/v2/NfeAutorizacao?wsdl";
        public string RespostaAutorizarProducao => "https://nfe.sefaz.go.gov.br/nfe/services/v2/NfeRetAutorizacao?wsdl";
        public string RecepcaoEventoProducao => "https://nfe.sefaz.go.gov.br/nfe/services/v2/RecepcaoEvento?wsdl";
        public string InutilizacaoProducao => "https://nfe.sefaz.go.gov.br/nfe/services/v2/NfeInutilizacao2?wsdl";

        public string ConsultarHomologacao => "https://homolog.sefaz.go.gov.br/nfe/services/v2/NfeConsulta2?wsdl";
        public string AutorizarHomologacao => "https://homolog.sefaz.go.gov.br/nfe/services/v2/NfeAutorizacao?wsdl";
        public string RespostaAutorizarHomologacao => "https://homolog.sefaz.go.gov.br/nfe/services/v2/NfeRetAutorizacao?wsdl";
        public string RecepcaoEventoHomologacao => "https://homolog.sefaz.go.gov.br/nfe/services/v2/RecepcaoEvento?wsdl";
        public string InutilizacaoHomologacao => "https://homolog.sefaz.go.gov.br/nfe/services/v2/NfeInutilizacao2?wsdl";
    }
}
