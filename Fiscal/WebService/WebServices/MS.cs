namespace Fiscal.WebService.WebServices
{
    internal struct MS : IWebService, IWebServiceProducaoNFCe, IWebServiceHomologacaoNFCe
    {
        public string ConsultarProducao => "https://nfe.sefaz.ms.gov.br/ws/NFeConsultaProtocolo4";
        public string AutorizarProducao => "https://nfe.sefaz.ms.gov.br/ws/NFeAutorizacao4";
        public string RespostaAutorizarProducao => "https://nfe.sefaz.ms.gov.br/ws/NFeRetAutorizacao4";
        public string RecepcaoEventoProducao => "https://nfe.sefaz.ms.gov.br/ws/NFeRecepcaoEvento4";
        public string InutilizacaoProducao => "https://nfe.sefaz.ms.gov.br/ws/NFeInutilizacao4";

        public string ConsultarHomologacao => "https://hom.nfe.sefaz.ms.gov.br/ws/NFeConsultaProtocolo4";
        public string AutorizarHomologacao => "https://hom.nfe.sefaz.ms.gov.br/ws/NFeAutorizacao4";
        public string RespostaAutorizarHomologacao => "https://hom.nfe.sefaz.ms.gov.br/ws/NFeRetAutorizacao4";
        public string RecepcaoEventoHomologacao => "https://hom.nfe.sefaz.ms.gov.br/ws/NFeRecepcaoEvento4";
        public string InutilizacaoHomologacao => "https://hom.nfe.sefaz.ms.gov.br/ws/NFeInutilizacao4";

        public string ConsultarProducaoNFCe => "https://nfce.sefaz.ms.gov.br/ws/NFeConsultaProtocolo4";
        public string AutorizarProducaoNFCe => "https://nfce.sefaz.ms.gov.br/ws/NFeAutorizacao4";
        public string RespostaAutorizarProducaoNFCe => "https://nfce.sefaz.ms.gov.br/ws/NFeRetAutorizacao4";
        public string RecepcaoEventoProducaoNFCe => "https://nfce.sefaz.ms.gov.br/ws/NFeRecepcaoEvento4";
        public string InutilizacaoProducaoNFCe => "https://nfce.sefaz.ms.gov.br/ws/NFeInutilizacao4";

        public string ConsultarHomologacaoNFCe => "https://hom.nfce.sefaz.ms.gov.br/ws/NFeConsultaProtocolo4";
        public string AutorizarHomologacaoNFCe => "https://hom.nfce.sefaz.ms.gov.br/ws/NFeAutorizacao4";
        public string RespostaAutorizarHomologacaoNFCe => "https://hom.nfce.sefaz.ms.gov.br/ws/NFeRetAutorizacao4";
        public string RecepcaoEventoHomologacaoNFCe => "https://hom.nfce.sefaz.ms.gov.br/ws/NFeRecepcaoEvento4";
        public string InutilizacaoHomologacaoNFCe => "https://hom.nfce.sefaz.ms.gov.br/ws/NFeInutilizacao4";
    }
}
