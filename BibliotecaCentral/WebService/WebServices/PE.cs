namespace BibliotecaCentral.WebService.WebServices
{
    internal struct PE : IWebService
    {
        public string ConsultarProducao => "https://nfe.sefaz.pe.gov.br/nfe-service/services/NfeConsulta2";
        public string AutorizarProducao => "https://nfe.sefaz.pe.gov.br/nfe-service/services/NfeAutorizacao?wsdl";
        public string RespostaAutorizarProducao => "https://nfe.sefaz.pe.gov.br/nfe-service/services/NfeRetAutorizacao?wsdl";
        public string RecepcaoEventoProducao => "https://nfe.sefaz.pe.gov.br/nfe-service/services/RecepcaoEvento";

        public string ConsultarHomologacao => "https://nfehomolog.sefaz.pe.gov.br/nfe-service/services/NfeConsulta2";
        public string AutorizarHomologacao => "https://nfehomolog.sefaz.pe.gov.br/nfe-service/services/NfeAutorizacao?wsdl";
        public string RespostaAutorizarHomologacao => "https://nfehomolog.sefaz.pe.gov.br/nfe-service/services/NfeRetAutorizacao?wsdl";
        public string RecepcaoEventoHomologacao => "https://nfehomolog.sefaz.pe.gov.br/nfe-service/services/RecepcaoEvento";

        public string VersaoRecepcaoEvento => "1.00";
    }
}
