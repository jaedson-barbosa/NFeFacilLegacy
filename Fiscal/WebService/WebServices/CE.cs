namespace Fiscal.WebService.WebServices
{
    internal struct CE : IWebService, IWebServiceProducaoNFCe, IWebServiceHomologacaoNFCe
    {
        public string ConsultarProducao => "https://nfe.sefaz.ce.gov.br/nfe4/services/CadConsultaCadastro4?wsdl";
        public string AutorizarProducao => "https://nfe.sefaz.ce.gov.br/nfe4/services/NFeAutorizacao4?wsdl";
        public string RespostaAutorizarProducao => "https://nfe.sefaz.ce.gov.br/nfe4/services/NFeRetAutorizacao4?wsdl";
        public string RecepcaoEventoProducao => "https://nfe.sefaz.ce.gov.br/nfe4/services/NFeRecepcaoEvento4?wsdl";
        public string InutilizacaoProducao => "https://nfe.sefaz.ce.gov.br/nfe4/services/NFeInutilizacao4?wsdl";

        public string ConsultarHomologacao => "https://nfeh.sefaz.ce.gov.br/nfe4/services/NFeConsultaProtocolo4?WSDL";
        public string AutorizarHomologacao => "https://nfeh.sefaz.ce.gov.br/nfe4/services/NFeAutorizacao4?WSDL";
        public string RespostaAutorizarHomologacao => "https://nfeh.sefaz.ce.gov.br/nfe4/services/NFeRetAutorizacao4?WSDL";
        public string RecepcaoEventoHomologacao => "https://nfeh.sefaz.ce.gov.br/nfe4/services/NFeRecepcaoEvento4?WSDL";
        public string InutilizacaoHomologacao => "https://nfeh.sefaz.ce.gov.br/nfe4/services/NFeInutilizacao4?WSDL";

        public string ConsultarProducaoNFCe => "https://nfce.sefaz.ce.gov.br/nfce/services/NfeConsulta2?WSDL";
        public string AutorizarProducaoNFCe => "https://nfce.sefaz.ce.gov.br/nfce/services/NfeAutorizacao?WSDL";
        public string RespostaAutorizarProducaoNFCe => "https://nfce.sefaz.ce.gov.br/nfce/services/NfeRetAutorizacao?WSDL";
        public string RecepcaoEventoProducaoNFCe => "https://nfce.sefaz.ce.gov.br/nfce/services/RecepcaoEvento?WSDL";
        public string InutilizacaoProducaoNFCe => "https://nfce.sefaz.ce.gov.br/nfce/services/NfeInutilizacao2?WSDL";

        public string ConsultarHomologacaoNFCe => "https://nfceh.sefaz.ce.gov.br/nfce/services/NfeConsulta2?WSDL";
        public string AutorizarHomologacaoNFCe => "https://nfceh.sefaz.ce.gov.br/nfce/services/NfeAutorizacao?WSDL";
        public string RespostaAutorizarHomologacaoNFCe => "https://nfceh.sefaz.ce.gov.br/nfce/services/NfeRetAutorizacao?WSDL";
        public string RecepcaoEventoHomologacaoNFCe => "https://nfceh.sefaz.ce.gov.br/nfce/services/RecepcaoEvento?WSDL";
        public string InutilizacaoHomologacaoNFCe => "https://nfceh.sefaz.ce.gov.br/nfce/services/NfeInutilizacao2?WSDL";
    }
}
