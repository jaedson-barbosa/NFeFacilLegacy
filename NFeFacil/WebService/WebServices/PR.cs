namespace NFeFacil.WebService.WebServices
{
    internal struct PR : IWebService
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
    }
}
