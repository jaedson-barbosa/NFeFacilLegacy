namespace Fiscal.WebService.WebServices
{
    // Os web services são iguais
    internal struct GO : IWebService
    {
        public string ConsultarProducao => "https://nfe.sefaz.go.gov.br/nfe/services/NFeConsultaProtocolo4?wsdl";
        public string AutorizarProducao => "https://nfe.sefaz.go.gov.br/nfe/services/NFeAutorizacao4?wsdl";
        public string RespostaAutorizarProducao => "https://nfe.sefaz.go.gov.br/nfe/services/NFeRetAutorizacao4?wsdl";
        public string RecepcaoEventoProducao => "https://nfe.sefaz.go.gov.br/nfe/services/NFeRecepcaoEvento4?wsdl";
        public string InutilizacaoProducao => "https://nfe.sefaz.go.gov.br/nfe/services/NFeInutilizacao4?wsdl";

        public string ConsultarHomologacao => "https://homolog.sefaz.go.gov.br/nfe/services/NFeConsultaProtocolo4?wsdl";
        public string AutorizarHomologacao => "https://homolog.sefaz.go.gov.br/nfe/services/NFeAutorizacao4?wsdl";
        public string RespostaAutorizarHomologacao => "https://homolog.sefaz.go.gov.br/nfe/services/NFeRetAutorizacao4?wsdl";
        public string RecepcaoEventoHomologacao => "https://homolog.sefaz.go.gov.br/nfe/services/NFeRecepcaoEvento4?wsdl";
        public string InutilizacaoHomologacao => "https://homolog.sefaz.go.gov.br/nfe/services/NFeInutilizacao4?wsdl";
    }
}
