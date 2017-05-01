namespace BibliotecaCentral.WebService.WebServices
{
    internal struct AM : IWebService
    {
        public string ConsultarProducao => "https://nfe.sefaz.am.gov.br/services2/services/NfeConsulta2";
        public string AutorizarProducao => "https://nfe.sefaz.am.gov.br/services2/services/NfeAutorizacao";
        public string RespostaAutorizarProducao => "https://nfe.sefaz.am.gov.br/services2/services/NfeRetAutorizacao";
        public string RecepcaoEventoProducao => "https://nfe.sefaz.am.gov.br/services2/services/RecepcaoEvento";

        public string ConsultarHomologacao => "https://homnfe.sefaz.am.gov.br/services2/services/NfeConsulta2";
        public string AutorizarHomologacao => "https://homnfe.sefaz.am.gov.br/services2/services/NfeAutorizacao";
        public string RespostaAutorizarHomologacao => "https://homnfe.sefaz.am.gov.br/services2/services/NfeRetAutorizacao";
        public string RecepcaoEventoHomologacao => "https://homnfe.sefaz.am.gov.br/services2/services/RecepcaoEvento";

        public string VersaoRecepcaoEvento => "1.00";
    }
}
