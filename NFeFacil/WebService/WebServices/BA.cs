namespace NFeFacil.WebService.WebServices
{
    internal struct BA : IWebService
    {
        public string ConsultarProducao => "https://nfe.sefaz.ba.gov.br/webservices/NfeConsulta/NfeConsulta.asmx";
        public string AutorizarProducao => "https://nfe.sefaz.ba.gov.br/webservices/NfeAutorizacao/NfeAutorizacao.asmx";
        public string RespostaAutorizarProducao => "https://nfe.sefaz.ba.gov.br/webservices/NfeRetAutorizacao/NfeRetAutorizacao.asmx";
        public string RecepcaoEventoProducao => "https://nfe.sefaz.ba.gov.br/webservices/sre/recepcaoevento.asmx";

        public string ConsultarHomologacao => "https://hnfe.sefaz.ba.gov.br/webservices/NfeConsulta/NfeConsulta.asmx";
        public string AutorizarHomologacao => "https://hnfe.sefaz.ba.gov.br/webservices/NfeAutorizacao/NfeAutorizacao.asmx";
        public string RespostaAutorizarHomologacao => "https://hnfe.sefaz.ba.gov.br/webservices/NfeRetAutorizacao/NfeRetAutorizacao.asmx";
        public string RecepcaoEventoHomologacao => "https://hnfe.sefaz.ba.gov.br/webservices/sre/recepcaoevento.asmx";

        public string VersaoRecepcaoEvento => "2.00";
    }
}
