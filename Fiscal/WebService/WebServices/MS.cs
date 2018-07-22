namespace Fiscal.WebService.WebServices
{
    internal struct MS : IWebService, IWebServiceProducaoNFCe, IWebServiceHomologacaoNFCe
    {
        public string ConsultarProducao => "https://nfe.sefaz.ms.gov.br/ws/NFeConsultaProtocolo4";
        public string AutorizarProducao => "https://nfe.sefaz.ms.gov.br/ws/NFeAutorizacao4";
        public string RespostaAutorizarProducao => "https://nfe.sefaz.ms.gov.br/ws/NFeRetAutorizacao4";
        public string RecepcaoEventoProducao => "https://nfe.sefaz.ms.gov.br/ws/NFeRecepcaoEvento4";
        public string InutilizacaoProducao => "https://nfe.sefaz.ms.gov.br/ws/NFeInutilizacao4";

        public string ConsultarHomologacao => "https://hom.nfe.sefaz.ms.gov.br/ws/NFeConsultaProtocolo4";
        public string AutorizarHomologacao => "https://hom.nfe.sefaz.ms.gov.br/ws/NFeAutorizacao4";
        public string RespostaAutorizarHomologacao => "https://hom.nfe.sefaz.ms.gov.br/ws/NFeRetAutorizacao4";
        public string RecepcaoEventoHomologacao => "https://hom.nfe.sefaz.ms.gov.br/ws/NFeRecepcaoEvento4";
        public string InutilizacaoHomologacao => "https://hom.nfe.sefaz.ms.gov.br/ws/NFeInutilizacao4";

        public string ConsultarProducaoNFCe => "https://nfce.fazenda.ms.gov.br/producao/services2/NfeConsulta2";
        public string AutorizarProducaoNFCe => "https://nfce.fazenda.ms.gov.br/producao/services2/NfeAutorizacao";
        public string RespostaAutorizarProducaoNFCe => "https://nfce.fazenda.ms.gov.br/producao/services2/NfeRetAutorizacao";
        public string RecepcaoEventoProducaoNFCe => "https://nfce.fazenda.ms.gov.br/producao/services2/RecepcaoEvento";
        public string InutilizacaoProducaoNFCe => "https://nfce.fazenda.ms.gov.br/producao/services2/NfeInutilizacao2";

        public string ConsultarHomologacaoNFCe => "https://homologacao.nfce.fazenda.ms.gov.br/homologacao/services2/NfeConsulta2";
        public string AutorizarHomologacaoNFCe => "https://homologacao.nfce.fazenda.ms.gov.br/homologacao/services2/NfeAutorizacao";
        public string RespostaAutorizarHomologacaoNFCe => "https://homologacao.nfce.fazenda.ms.gov.br/homologacao/services2/NfeRetAutorizacao";
        public string RecepcaoEventoHomologacaoNFCe => "https://homologacao.nfce.fazenda.ms.gov.br/homologacao/services2/RecepcaoEvento";
        public string InutilizacaoHomologacaoNFCe => "https://homologacao.nfce.fazenda.ms.gov.br/homologacao/services2/NfeInutilizacao2";
    }
}
