namespace Fiscal.WebService.WebServices
{
    internal struct PR : IWebService, IWebServiceProducaoNFCe, IWebServiceHomologacaoNFCe
    {
        public string ConsultarProducao => "https://nfe.fazenda.pr.gov.br/nfe/NFeConsulta3?wsdl";
        public string AutorizarProducao => "https://nfe.fazenda.pr.gov.br/nfe/NFeAutorizacao3?wsdl";
        public string RespostaAutorizarProducao => "https://nfe.fazenda.pr.gov.br/nfe/NFeRetAutorizacao3?wsdl";
        public string RecepcaoEventoProducao => "https://nfe.fazenda.pr.gov.br/nfe/NFeRecepcaoEvento?wsdl";
        public string InutilizacaoProducao => "https://nfe.fazenda.pr.gov.br/nfe/NFeInutilizacao3?wsdl";

        public string ConsultarHomologacao => "https://homologacao.nfe.fazenda.pr.gov.br/nfe/NFeConsulta3?wsdl";
        public string AutorizarHomologacao => "https://homologacao.nfe.fazenda.pr.gov.br/nfe/NFeAutorizacao3?wsdl";
        public string RespostaAutorizarHomologacao => "https://homologacao.nfe.fazenda.pr.gov.br/nfe/NFeRetAutorizacao3?wsdl";
        public string RecepcaoEventoHomologacao => "https://homologacao.nfe.fazenda.pr.gov.br/nfe/NFeRecepcaoEvento?wsdl";
        public string InutilizacaoHomologacao => "https://homologacao.nfe.fazenda.pr.gov.br/nfe/NFeInutilizacao3?wsdl";

        public string ConsultarProducaoNFCe => "https://nfce.fazenda.pr.gov.br/nfce/NFeConsulta3?wsdl";
        public string AutorizarProducaoNFCe => "https://nfce.fazenda.pr.gov.br/nfce/NFeAutorizacao3?wsdl";
        public string RespostaAutorizarProducaoNFCe => "https://nfce.fazenda.pr.gov.br/nfce/NFeRetAutorizacao3?wsdl";
        public string RecepcaoEventoProducaoNFCe => "https://nfce.fazenda.pr.gov.br/nfce/NFeRecepcaoEvento?wsdl";
        public string InutilizacaoProducaoNFCe => "https://nfce.fazenda.pr.gov.br/nfce/NFeInutilizacao3?wsdl";

        public string ConsultarHomologacaoNFCe => "https://homologacao.nfce.fazenda.pr.gov.br/nfce/NFeConsulta3?wsdl";
        public string AutorizarHomologacaoNFCe => "https://homologacao.nfce.fazenda.pr.gov.br/nfce/NFeAutorizacao3?wsdl";
        public string RespostaAutorizarHomologacaoNFCe => "https://homologacao.nfce.fazenda.pr.gov.br/nfce/NFeRetAutorizacao3?wsdl";
        public string RecepcaoEventoHomologacaoNFCe => "https://homologacao.nfce.fazenda.pr.gov.br/nfce/NFeRecepcaoEvento?wsdl";
        public string InutilizacaoHomologacaoNFCe => "https://nfce.fazenda.pr.gov.br/nfce/NFeInutilizacao3?wsdl";
    }
}
