namespace NFeFacil.WebService.WebServices
{
    internal struct AM : IWebService, IWebServiceHomologacaoNFCe
    {
        public string ConsultarProducao => "https://nfe.sefaz.am.gov.br/services2/services/NfeConsulta2";
        public string AutorizarProducao => "https://nfe.sefaz.am.gov.br/services2/services/NfeAutorizacao";
        public string RespostaAutorizarProducao => "https://nfe.sefaz.am.gov.br/services2/services/NfeRetAutorizacao";
        public string RecepcaoEventoProducao => "https://nfe.sefaz.am.gov.br/services2/services/RecepcaoEvento";
        public string InutilizacaoProducao => "https://nfe.sefaz.am.gov.br/services2/services/NfeInutilizacao2";

        public string ConsultarHomologacao => "https://homnfe.sefaz.am.gov.br/services2/services/NfeConsulta2";
        public string AutorizarHomologacao => "https://homnfe.sefaz.am.gov.br/services2/services/NfeAutorizacao";
        public string RespostaAutorizarHomologacao => "https://homnfe.sefaz.am.gov.br/services2/services/NfeRetAutorizacao";
        public string RecepcaoEventoHomologacao => "https://homnfe.sefaz.am.gov.br/services2/services/RecepcaoEvento";
        public string InutilizacaoHomologacao => "https://homnfe.sefaz.am.gov.br/services2/services/NfeInutilizacao2";

        public string ConsultarHomologacaoNFCe => "https://homnfce.sefaz.am.gov.br/nfce-services-nac/services/NfeConsulta2";
        public string AutorizarHomologacaoNFCe => "https://homnfce.sefaz.am.gov.br/nfce-services-nac/services/NfeAutorizacao";
        public string RespostaAutorizarHomologacaoNFCe => "https://homnfce.sefaz.am.gov.br/nfce-services-nac/services/NfeRetAutorizacao";
        public string RecepcaoEventoHomologacaoNFCe => "https://homnfce.sefaz.am.gov.br/nfce-services-nac/services/NfeRecepcao2";
        public string InutilizacaoHomologacaoNFCe => "https://homnfce.sefaz.am.gov.br/nfce-services-nac/services/NfeInutilizacao2";
    }
}
