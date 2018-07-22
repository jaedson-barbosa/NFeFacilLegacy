namespace Fiscal.WebService.WebServices
{
    internal struct AM : IWebService, IWebServiceProducaoNFCe, IWebServiceHomologacaoNFCe
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

        public string ConsultarProducaoNFCe => "https://nfce.sefaz.am.gov.br/nfce-services/services/NfeConsulta4";
        public string AutorizarProducaoNFCe => "https://nfce.sefaz.am.gov.br/nfce-services/services/NfeAutorizacao4";
        public string RespostaAutorizarProducaoNFCe => "https://nfce.sefaz.am.gov.br/nfce-services/services/NfeRetAutorizacao4";
        public string RecepcaoEventoProducaoNFCe => "https://nfce.sefaz.am.gov.br/nfce-services/services/RecepcaoEvento4";
        public string InutilizacaoProducaoNFCe => "https://nfce.sefaz.am.gov.br/nfce-services/services/NfeInutilizacao4";

        public string ConsultarHomologacaoNFCe => "https://homnfce.sefaz.am.gov.br/nfce-services/services/NfeConsulta4";
        public string AutorizarHomologacaoNFCe => "https://homnfce.sefaz.am.gov.br/nfce-services/services/NfeAutorizacao4";
        public string RespostaAutorizarHomologacaoNFCe => "https://homnfce.sefaz.am.gov.br/nfce-services/services/NfeRetAutorizacao4";
        public string RecepcaoEventoHomologacaoNFCe => "https://homnfce.sefaz.am.gov.br/nfce-services/services/RecepcaoEvento4";
        public string InutilizacaoHomologacaoNFCe => "https://homnfce.sefaz.am.gov.br/nfce-services/services/NfeInutilizacao4";
    }
}
