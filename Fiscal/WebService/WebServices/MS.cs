namespace Fiscal.WebService.WebServices
{
    internal struct MS : IWebService, IWebServiceProducaoNFCe, IWebServiceHomologacaoNFCe
    {
        public string ConsultarProducao => "https://nfe.fazenda.ms.gov.br/producao/services2/CadConsultaCadastro2";
        public string AutorizarProducao => "https://nfe.fazenda.ms.gov.br/producao/services2/NfeAutorizacao";
        public string RespostaAutorizarProducao => "https://nfe.fazenda.ms.gov.br/producao/services2/NfeRetAutorizacao";
        public string RecepcaoEventoProducao => "https://nfe.fazenda.ms.gov.br/producao/services2/RecepcaoEvento";
        public string InutilizacaoProducao => "https://nfe.fazenda.ms.gov.br/producao/services2/NfeInutilizacao2";

        public string ConsultarHomologacao => "https://homologacao.nfe.ms.gov.br/homologacao/services2/NfeConsulta2";
        public string AutorizarHomologacao => "https://homologacao.nfe.ms.gov.br/homologacao/services2/NfeAutorizacao";
        public string RespostaAutorizarHomologacao => "https://homologacao.nfe.ms.gov.br/homologacao/services2/NfeRetAutorizacao";
        public string RecepcaoEventoHomologacao => "https://homologacao.nfe.ms.gov.br/homologacao/services2/RecepcaoEvento";
        public string InutilizacaoHomologacao => "https://homologacao.nfe.ms.gov.br/homologacao/services2/NfeInutilizacao2";

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
