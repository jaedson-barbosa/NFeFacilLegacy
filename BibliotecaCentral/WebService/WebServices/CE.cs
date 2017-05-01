namespace BibliotecaCentral.WebService.WebServices
{
    internal struct CE : IWebService
    {
        public string ConsultarProducao => "https://nfe.sefaz.ce.gov.br/nfe2/services/NfeConsulta2?wsdl";
        public string AutorizarProducao => "https://nfe.sefaz.ce.gov.br/nfe2/services/NfeAutorizacao?wsdl";
        public string RespostaAutorizarProducao => "https://nfe.sefaz.ce.gov.br/nfe2/services/NfeRetAutorizacao?wsdl";
        public string RecepcaoEventoProducao => "https://nfe.sefaz.ce.gov.br/nfe2/services/RecepcaoEvento?wsdl";

        public string ConsultarHomologacao => "https://nfeh.sefaz.ce.gov.br/nfe2/services/NfeConsulta2?wsdl ";
        public string AutorizarHomologacao => "https://nfeh.sefaz.ce.gov.br/nfe2/services/NfeAutorizacao?wsdl";
        public string RespostaAutorizarHomologacao => "https://nfeh.sefaz.ce.gov.br/nfe2/services/NfeRetAutorizacao?wsdl";
        public string RecepcaoEventoHomologacao => "https://nfeh.sefaz.ce.gov.br/nfe2/services/RecepcaoEvento?wsdl";

        public string VersaoRecepcaoEvento => "1.00";
    }
}
