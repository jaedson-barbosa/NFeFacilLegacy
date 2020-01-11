namespace Fiscal.WebService.WebServices
{
    internal struct SVRS : IWebService, IWebServiceProducaoNFCe, IWebServiceHomologacaoNFCe
    {
        public string ConsultarProducao => "https://nfe.svrs.rs.gov.br/ws/NfeConsulta/NfeConsulta4.asmx";
        public string AutorizarProducao => "https://nfe.svrs.rs.gov.br/ws/NfeAutorizacao/NFeAutorizacao4.asmx";
        public string RespostaAutorizarProducao => "https://nfe.svrs.rs.gov.br/ws/NfeRetAutorizacao/NFeRetAutorizacao4.asmx";
        public string RecepcaoEventoProducao => "https://nfe.svrs.rs.gov.br/ws/recepcaoevento/recepcaoevento4.asmx";
        public string InutilizacaoProducao => "https://nfe.svrs.rs.gov.br/ws/nfeinutilizacao/nfeinutilizacao4.asmx";

        public string ConsultarHomologacao => "https://nfe-homologacao.svrs.rs.gov.br/ws/NfeConsulta/NfeConsulta4.asmx";
        public string AutorizarHomologacao => "https://nfe-homologacao.svrs.rs.gov.br/ws/NfeAutorizacao/NFeAutorizacao4.asmx";
        public string RespostaAutorizarHomologacao => "https://nfe-homologacao.svrs.rs.gov.br/ws/NfeRetAutorizacao/NFeRetAutorizacao4.asmx";
        public string RecepcaoEventoHomologacao => "https://nfe-homologacao.svrs.rs.gov.br/ws/recepcaoevento/recepcaoevento4.asmx";
        public string InutilizacaoHomologacao => "https://nfe-homologacao.svrs.rs.gov.br/ws/nfeinutilizacao/nfeinutilizacao4.asmx";

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
