namespace Fiscal.WebService.WebServices
{
    internal struct PE : IWebService, IWebServiceProducaoNFCe, IWebServiceHomologacaoNFCe
    {
        public string ConsultarProducao => "https://nfe.sefaz.pe.gov.br/nfe-service/services/NfeConsulta2";
        public string AutorizarProducao => "https://nfe.sefaz.pe.gov.br/nfe-service/services/NfeAutorizacao?wsdl";
        public string RespostaAutorizarProducao => "https://nfe.sefaz.pe.gov.br/nfe-service/services/NfeRetAutorizacao?wsdl";
        public string RecepcaoEventoProducao => "https://nfe.sefaz.pe.gov.br/nfe-service/services/RecepcaoEvento";
        public string InutilizacaoProducao => "https://nfe.sefaz.pe.gov.br/nfe-service/services/NfeInutilizacao2";

        public string ConsultarHomologacao => "https://nfehomolog.sefaz.pe.gov.br/nfe-service/services/NfeConsulta2";
        public string AutorizarHomologacao => "https://nfehomolog.sefaz.pe.gov.br/nfe-service/services/NfeAutorizacao?wsdl";
        public string RespostaAutorizarHomologacao => "https://nfehomolog.sefaz.pe.gov.br/nfe-service/services/NfeRetAutorizacao?wsdl";
        public string RecepcaoEventoHomologacao => "https://nfehomolog.sefaz.pe.gov.br/nfe-service/services/RecepcaoEvento";
        public string InutilizacaoHomologacao => "https://nfehomolog.sefaz.pe.gov.br/nfe-service/services/NfeInutilizacao2";

        public string ConsultarProducaoNFCe => "https://nfce.svrs.rs.gov.br/ws/NfeConsulta/NfeConsulta2.asmx";
        public string AutorizarProducaoNFCe => "https://nfce.svrs.rs.gov.br/ws/NfeAutorizacao/NFeAutorizacao.asmx";
        public string RespostaAutorizarProducaoNFCe => "https://nfce.svrs.rs.gov.br/ws/NfeRetAutorizacao/NFeRetAutorizacao.asmx";
        public string RecepcaoEventoProducaoNFCe => "https://nfce.svrs.rs.gov.br/ws/recepcaoevento/recepcaoevento.asmx";
        public string InutilizacaoProducaoNFCe => "https://nfce.svrs.rs.gov.br/ws/nfeinutilizacao/nfeinutilizacao2.asmx";

        public string ConsultarHomologacaoNFCe => "https://nfce-homologacao.svrs.rs.gov.br/ws/NfeConsulta/NfeConsulta2.asmx";
        public string AutorizarHomologacaoNFCe => "https://nfce-homologacao.svrs.rs.gov.br/ws/NfeAutorizacao/NFeAutorizacao.asmx";
        public string RespostaAutorizarHomologacaoNFCe => "https://nfce-homologacao.svrs.rs.gov.br/ws/NfeRetAutorizacao/NFeRetAutorizacao.asmx";
        public string RecepcaoEventoHomologacaoNFCe => "https://nfce-homologacao.svrs.rs.gov.br/ws/recepcaoevento/recepcaoevento.asmx";
        public string InutilizacaoHomologacaoNFCe => "https://nfce-homologacao.svrs.rs.gov.br/ws/nfeinutilizacao/nfeinutilizacao2.asmx";
    }
}
