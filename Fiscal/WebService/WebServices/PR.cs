namespace Fiscal.WebService.WebServices
{
    internal struct PR : IWebService, IWebServiceProducaoNFCe, IWebServiceHomologacaoNFCe
    {
        public string ConsultarProducao => "https://nfe.sefa.pr.gov.br/nfe/NFeConsultaProtocolo4?wsdl";
        public string AutorizarProducao => "https://nfe.sefa.pr.gov.br/nfe/NFeAutorizacao4?wsdl";
        public string RespostaAutorizarProducao => "https://nfe.sefa.pr.gov.br/nfe/NFeRetAutorizacao4?wsdl";
        public string RecepcaoEventoProducao => "https://nfe.sefa.pr.gov.br/nfe/NFeRecepcaoEvento4?wsdl";
        public string InutilizacaoProducao => "https://nfe.sefa.pr.gov.br/nfe/NFeInutilizacao4?wsdl";

        public string ConsultarHomologacao => "https://homologacao.nfe.sefa.pr.gov.br/nfe/NFeConsultaProtocolo4?wsdl";
        public string AutorizarHomologacao => "https://homologacao.nfe.sefa.pr.gov.br/nfe/NFeAutorizacao4?wsdl";
        public string RespostaAutorizarHomologacao => "https://homologacao.nfe.sefa.pr.gov.br/nfe/NFeRetAutorizacao4?wsdl";
        public string RecepcaoEventoHomologacao => "https://homologacao.nfe.sefa.pr.gov.br/nfe/NFeRecepcaoEvento4?wsdl";
        public string InutilizacaoHomologacao => "https://homologacao.nfe.sefa.pr.gov.br/nfe/NFeInutilizacao4?wsdl";

        public string ConsultarProducaoNFCe => "https://nfce.sefa.pr.gov.br/nfce/NFeConsultaProtocolo4";
        public string AutorizarProducaoNFCe => "https://nfce.sefa.pr.gov.br/nfce/NFeAutorizacao4";
        public string RespostaAutorizarProducaoNFCe => "https://nfce.sefa.pr.gov.br/nfce/NFeRetAutorizacao4";
        public string RecepcaoEventoProducaoNFCe => "https://nfce.sefa.pr.gov.br/nfce/NFeRecepcaoEvento4";
        public string InutilizacaoProducaoNFCe => "https://nfce.sefa.pr.gov.br/nfce/NFeInutilizacao4";

        public string ConsultarHomologacaoNFCe => "https://homologacao.nfce.sefa.pr.gov.br/nfce/NFeConsultaProtocolo4";
        public string AutorizarHomologacaoNFCe => "https://homologacao.nfce.sefa.pr.gov.br/nfce/NFeAutorizacao4";
        public string RespostaAutorizarHomologacaoNFCe => "https://homologacao.nfce.sefa.pr.gov.br/nfce/NFeRetAutorizacao4";
        public string RecepcaoEventoHomologacaoNFCe => "https://homologacao.nfce.sefa.pr.gov.br/nfce/NFeRecepcaoEvento4";
        public string InutilizacaoHomologacaoNFCe => "https://homologacao.nfce.sefa.pr.gov.br/nfce/NFeInutilizacao4";
    }
}
