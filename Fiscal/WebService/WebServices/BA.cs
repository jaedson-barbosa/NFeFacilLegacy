namespace Fiscal.WebService.WebServices
{
    internal struct BA : IWebService, IWebServiceProducaoNFCe, IWebServiceHomologacaoNFCe
    {
        public string ConsultarProducao => "https://nfe.sefaz.ba.gov.br/webservices/NFeConsultaProtocolo4/NFeConsultaProtocolo4.asmx";
        public string AutorizarProducao => "https://nfe.sefaz.ba.gov.br/webservices/NFeAutorizacao4/NFeAutorizacao4.asmx";
        public string RespostaAutorizarProducao => "https://nfe.sefaz.ba.gov.br/webservices/NFeRetAutorizacao4/NFeRetAutorizacao4.asmx";
        public string RecepcaoEventoProducao => "https://nfe.sefaz.ba.gov.br/webservices/NFeRecepcaoEvento4/NFeRecepcaoEvento4.asmx";
        public string InutilizacaoProducao => "https://nfe.sefaz.ba.gov.br/webservices/NFeInutilizacao4/NFeInutilizacao4.asmx";

        public string ConsultarHomologacao => "https://hnfe.sefaz.ba.gov.br/webservices/NFeConsultaProtocolo4/NFeConsultaProtocolo4.asmx";
        public string AutorizarHomologacao => "https://hnfe.sefaz.ba.gov.br/webservices/NFeAutorizacao4/NFeAutorizacao4.asmx";
        public string RespostaAutorizarHomologacao => "https://hnfe.sefaz.ba.gov.br/webservices/NFeRetAutorizacao4/NFeRetAutorizacao4.asmx";
        public string RecepcaoEventoHomologacao => "https://hnfe.sefaz.ba.gov.br/webservices/NFeRecepcaoEvento4/NFeRecepcaoEvento4.asmx";
        public string InutilizacaoHomologacao => "https://hnfe.sefaz.ba.gov.br/webservices/NFeInutilizacao4/NFeInutilizacao4.asmx";

        public string ConsultarProducaoNFCe => "https://nfce.svrs.rs.gov.br/ws/NfeConsulta/NfeConsulta4.asmx";
        public string AutorizarProducaoNFCe => "https://nfce.svrs.rs.gov.br/ws/NfeAutorizacao/NFeAutorizacao4.asmx";
        public string RespostaAutorizarProducaoNFCe => "https://nfce.svrs.rs.gov.br/ws/NfeRetAutorizacao/NFeRetAutorizacao4.asmx";
        public string RecepcaoEventoProducaoNFCe => "https://nfce.svrs.rs.gov.br/ws/recepcaoevento/recepcaoevento4.asmx";
        public string InutilizacaoProducaoNFCe => "https://nfce.svrs.rs.gov.br/ws/nfeinutilizacao/nfeinutilizacao4.asmx";

        public string ConsultarHomologacaoNFCe => "https://nfce-homologacao.svrs.rs.gov.br/ws/NfeConsulta/NfeConsulta4.asmx";
        public string AutorizarHomologacaoNFCe => "https://nfce-homologacao.svrs.rs.gov.br/ws/NfeAutorizacao/NFeAutorizacao4.asmx";
        public string RespostaAutorizarHomologacaoNFCe => "https://nfce-homologacao.svrs.rs.gov.br/ws/NfeRetAutorizacao/NFeRetAutorizacao4.asmx";
        public string RecepcaoEventoHomologacaoNFCe => "https://nfce-homologacao.svrs.rs.gov.br/ws/recepcaoevento/recepcaoevento4.asmx";
        public string InutilizacaoHomologacaoNFCe => "https://nfce-homologacao.svrs.rs.gov.br/ws/nfeinutilizacao/nfeinutilizacao4.asmx";
    }
}
