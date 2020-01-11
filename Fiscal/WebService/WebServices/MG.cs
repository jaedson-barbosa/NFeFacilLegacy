namespace Fiscal.WebService.WebServices
{
    // Falta verificar os web services porque o site ainda está em construção
    internal struct MG : IWebService
    {
        public string ConsultarProducao => "https://nfe.fazenda.mg.gov.br/nfe2/services/NFeConsultaProtocolo4";
        public string AutorizarProducao => "https://nfe.fazenda.mg.gov.br/nfe2/services/NFeAutorizacao4";
        public string RespostaAutorizarProducao => "https://nfe.fazenda.mg.gov.br/nfe2/services/NFeRetAutorizacao4";
        public string RecepcaoEventoProducao => "https://nfe.fazenda.mg.gov.br/nfe2/services/NFeRecepcaoEvento4";
        public string InutilizacaoProducao => "https://nfe.fazenda.mg.gov.br/nfe2/services/NFeInutilizacao4";

        public string ConsultarHomologacao => "https://hnfe.fazenda.mg.gov.br/nfe2/services/NFeConsultaProtocolo4";
        public string AutorizarHomologacao => "https://hnfe.fazenda.mg.gov.br/nfe2/services/NFeAutorizacao4";
        public string RespostaAutorizarHomologacao => "https://hnfe.fazenda.mg.gov.br/nfe2/services/NFeRetAutorizacao4";
        public string RecepcaoEventoHomologacao => "https://hnfe.fazenda.mg.gov.br/nfe2/services/NFeRecepcaoEvento4";
        public string InutilizacaoHomologacao => "https://hnfe.fazenda.mg.gov.br/nfe2/services/NFeInutilizacao4";
    }
}
