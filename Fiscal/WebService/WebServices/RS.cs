namespace Fiscal.WebService.WebServices
{
    internal struct RS : IWebService, IWebServiceProducaoNFCe, IWebServiceHomologacaoNFCe
    {
        public string ConsultarProducao => "https://nfe.sefazrs.rs.gov.br/ws/NfeConsulta/NfeConsulta4.asmx";
        public string AutorizarProducao => "https://nfe.sefazrs.rs.gov.br/ws/NfeAutorizacao/NFeAutorizacao4.asmx";
        public string RespostaAutorizarProducao => "https://nfe.sefazrs.rs.gov.br/ws/NfeRetAutorizacao/NFeRetAutorizacao4.asmx";
        public string RecepcaoEventoProducao => "https://nfe.sefazrs.rs.gov.br/ws/recepcaoevento/recepcaoevento4.asmx";
        public string InutilizacaoProducao => "https://nfe.sefazrs.rs.gov.br/ws/nfeinutilizacao/nfeinutilizacao4.asmx";

        public string ConsultarHomologacao => "https://nfe-homologacao.sefazrs.rs.gov.br/ws/NfeConsulta/NfeConsulta4.asmx";
        public string AutorizarHomologacao => "https://nfe-homologacao.sefazrs.rs.gov.br/ws/NfeAutorizacao/NFeAutorizacao4.asmx";
        public string RespostaAutorizarHomologacao => "https://nfe-homologacao.sefazrs.rs.gov.br/ws/NfeRetAutorizacao/NFeRetAutorizacao4.asmx";
        public string RecepcaoEventoHomologacao => "https://nfe-homologacao.sefazrs.rs.gov.br/ws/recepcaoevento/recepcaoevento4.asmx";
        public string InutilizacaoHomologacao => "https://nfe-homologacao.sefazrs.rs.gov.br/ws/nfeinutilizacao/nfeinutilizacao4.asmx";

        public string ConsultarProducaoNFCe => "https://nfce.sefazrs.rs.gov.br/ws/NfeConsulta/NfeConsulta4.asmx";
        public string AutorizarProducaoNFCe => "https://nfce.sefazrs.rs.gov.br/ws/NfeAutorizacao/NFeAutorizacao4.asmx";
        public string RespostaAutorizarProducaoNFCe => "https://nfce.sefazrs.rs.gov.br/ws/NfeRetAutorizacao/NFeRetAutorizacao4.asmx";
        public string RecepcaoEventoProducaoNFCe => "https://nfce.sefazrs.rs.gov.br/ws/recepcaoevento/recepcaoevento4.asmx";
        public string InutilizacaoProducaoNFCe => "https://nfce.sefazrs.rs.gov.br/ws/nfeinutilizacao/nfeinutilizacao4.asmx";

        public string ConsultarHomologacaoNFCe => "https://nfce-homologacao.sefazrs.rs.gov.br/ws/NfeConsulta/NfeConsulta4.asmx";
        public string AutorizarHomologacaoNFCe => "https://nfce-homologacao.sefazrs.rs.gov.br/ws/NfeAutorizacao/NFeAutorizacao4.asmx";
        public string RespostaAutorizarHomologacaoNFCe => "https://nfce-homologacao.sefazrs.rs.gov.br/ws/NfeRetAutorizacao/NFeRetAutorizacao4.asmx";
        public string RecepcaoEventoHomologacaoNFCe => "https://nfce-homologacao.sefazrs.rs.gov.br/ws/recepcaoevento/recepcaoevento4.asmx";
        public string InutilizacaoHomologacaoNFCe => "https://nfce-homologacao.sefazrs.rs.gov.br/ws/nfeinutilizacao/nfeinutilizacao4.asmx";
    }
}
