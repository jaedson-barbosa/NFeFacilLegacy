namespace Fiscal.WebService.WebServices
{
    internal struct MT : IWebService, IWebServiceProducaoNFCe, IWebServiceHomologacaoNFCe
    {
        public string ConsultarProducao => "https://nfe.sefaz.mt.gov.br/nfews/v2/services/NfeConsulta4?wsdl";
        public string AutorizarProducao => "https://nfe.sefaz.mt.gov.br/nfews/v2/services/NfeAutorizacao4?wsdl";
        public string RespostaAutorizarProducao => "https://nfe.sefaz.mt.gov.br/nfews/v2/services/NfeRetAutorizacao4?wsdl";
        public string RecepcaoEventoProducao => "https://nfe.sefaz.mt.gov.br/nfews/v2/services/RecepcaoEvento4?wsdl";
        public string InutilizacaoProducao => "https://nfe.sefaz.mt.gov.br/nfews/v2/services/NfeInutilizacao4?wsdl";

        public string ConsultarHomologacao => "https://homologacao.sefaz.mt.gov.br/nfews/v2/services/NfeConsulta4?wsdl";
        public string AutorizarHomologacao => "https://homologacao.sefaz.mt.gov.br/nfews/v2/services/NfeAutorizacao4?wsdl";
        public string RespostaAutorizarHomologacao => "https://homologacao.sefaz.mt.gov.br/nfews/v2/services/NfeRetAutorizacao4?wsdl";
        public string RecepcaoEventoHomologacao => "https://homologacao.sefaz.mt.gov.br/nfews/v2/services/RecepcaoEvento4?wsdl";
        public string InutilizacaoHomologacao => "https://homologacao.sefaz.mt.gov.br/nfews/v2/services/NfeInutilizacao4?wsdl";

        public string ConsultarProducaoNFCe => "https://nfce.sefaz.mt.gov.br/nfcews/services/NfeConsulta4";
        public string AutorizarProducaoNFCe => "https://nfce.sefaz.mt.gov.br/nfcews/services/NfeAutorizacao4";
        public string RespostaAutorizarProducaoNFCe => "https://nfce.sefaz.mt.gov.br/nfcews/services/NfeRetAutorizacao4";
        public string RecepcaoEventoProducaoNFCe => "https://nfce.sefaz.mt.gov.br/nfcews/services/RecepcaoEvento4";
        public string InutilizacaoProducaoNFCe => "https://nfce.sefaz.mt.gov.br/nfcews/services/NfeInutilizacao4";

        public string ConsultarHomologacaoNFCe => "https://homologacao.sefaz.mt.gov.br/nfcews/services/NfeConsulta4";
        public string AutorizarHomologacaoNFCe => "https://homologacao.sefaz.mt.gov.br/nfcews/services/NfeAutorizacao4";
        public string RespostaAutorizarHomologacaoNFCe => "https://homologacao.sefaz.mt.gov.br/nfcews/services/NfeRetAutorizacao4";
        public string RecepcaoEventoHomologacaoNFCe => "https://homologacao.sefaz.mt.gov.br/nfcews/services/RecepcaoEvento4";
        public string InutilizacaoHomologacaoNFCe => "https://homologacao.sefaz.mt.gov.br/nfcews/services/NfeInutilizacao4";
    }
}
