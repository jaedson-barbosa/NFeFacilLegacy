namespace Fiscal.WebService.WebServices
{
    internal struct AM : IWebService, IWebServiceHomologacaoNFCe
    {
        public string ConsultarProducao => "https://nfe.sefaz.am.gov.br/services2/services/NfeConsulta4";
        public string AutorizarProducao => "https://nfe.sefaz.am.gov.br/services2/services/NfeAutorizacao4";
        public string RespostaAutorizarProducao => "https://nfe.sefaz.am.gov.br/services2/services/NfeRetAutorizacao4";
        public string RecepcaoEventoProducao => "https://nfe.sefaz.am.gov.br/services2/services/RecepcaoEvento4";
        public string InutilizacaoProducao => "https://nfe.sefaz.am.gov.br/services2/services/NfeInutilizacao4";

        public string ConsultarHomologacao => "https://homnfe.sefaz.am.gov.br/services2/services/NfeConsulta4";
        public string AutorizarHomologacao => "https://homnfe.sefaz.am.gov.br/services2/services/NfeAutorizacao4";
        public string RespostaAutorizarHomologacao => "https://homnfe.sefaz.am.gov.br/services2/services/NfeRetAutorizacao4";
        public string RecepcaoEventoHomologacao => "https://homnfe.sefaz.am.gov.br/services2/services/RecepcaoEvento4";
        public string InutilizacaoHomologacao => "https://homnfe.sefaz.am.gov.br/services2/services/NfeInutilizacao4";

        public string ConsultarHomologacaoNFCe => "https://homnfce.sefaz.am.gov.br/nfce-services-nac/services/NfeConsulta2";
        public string AutorizarHomologacaoNFCe => "https://homnfce.sefaz.am.gov.br/nfce-services-nac/services/NfeAutorizacao";
        public string RespostaAutorizarHomologacaoNFCe => "https://homnfce.sefaz.am.gov.br/nfce-services-nac/services/NfeRetAutorizacao";
        public string RecepcaoEventoHomologacaoNFCe => "https://homnfce.sefaz.am.gov.br/nfce-services-nac/services/NfeRecepcao2";
        public string InutilizacaoHomologacaoNFCe => "https://homnfce.sefaz.am.gov.br/nfce-services-nac/services/NfeInutilizacao2";
    }
}
