namespace Fiscal.WebService.WebServices
{
    internal struct SP : IWebService, IWebServiceProducaoNFCe, IWebServiceHomologacaoNFCe
    {
        public string ConsultarProducao => "https://nfe.fazenda.sp.gov.br/ws/nfeconsultaprotocolo4.asmx";
        public string AutorizarProducao => "https://nfe.fazenda.sp.gov.br/ws/nfeautorizacao4.asmx";
        public string RespostaAutorizarProducao => "https://nfe.fazenda.sp.gov.br/ws/nferetautorizacao4.asmx";
        public string RecepcaoEventoProducao => "https://nfe.fazenda.sp.gov.br/ws/nferecepcaoevento4.asmx";
        public string InutilizacaoProducao => "https://nfe.fazenda.sp.gov.br/ws/nfeinutilizacao4.asmx";

        public string ConsultarHomologacao => "https://homologacao.nfe.fazenda.sp.gov.br/ws/nfeconsultaprotocolo4.asmx";
        public string AutorizarHomologacao => "https://homologacao.nfe.fazenda.sp.gov.br/ws/nfeautorizacao4.asmx";
        public string RespostaAutorizarHomologacao => "https://homologacao.nfe.fazenda.sp.gov.br/ws/nferetautorizacao4.asmx";
        public string RecepcaoEventoHomologacao => "https://homologacao.nfe.fazenda.sp.gov.br/ws/nferecepcaoevento4.asmx";
        public string InutilizacaoHomologacao => "https://homologacao.nfe.fazenda.sp.gov.br/ws/nfeinutilizacao4.asmx";

        public string ConsultarProducaoNFCe => "https://nfce.fazenda.sp.gov.br/ws/NFeConsultaProtocolo4.asmx";
        public string AutorizarProducaoNFCe => "https://nfce.fazenda.sp.gov.br/ws/NFeAutorizacao4.asmx";
        public string RespostaAutorizarProducaoNFCe => "https://nfce.fazenda.sp.gov.br/ws/NFeRetAutorizacao4.asmx";
        public string RecepcaoEventoProducaoNFCe => "https://nfce.fazenda.sp.gov.br/ws/NFeRecepcaoEvento4.asmx";
        public string InutilizacaoProducaoNFCe => "https://nfce.fazenda.sp.gov.br/ws/NFeInutilizacao4.asmx";

        public string ConsultarHomologacaoNFCe => "https://homologacao.nfce.fazenda.sp.gov.br/ws/NFeConsultaProtocolo4.asmx";
        public string AutorizarHomologacaoNFCe => "https://homologacao.nfce.fazenda.sp.gov.br/ws/NFeAutorizacao4.asmx";
        public string RespostaAutorizarHomologacaoNFCe => "https://homologacao.nfce.fazenda.sp.gov.br/ws/NFeRetAutorizacao4.asmx";
        public string RecepcaoEventoHomologacaoNFCe => "https://homologacao.nfce.fazenda.sp.gov.br/ws/NFeRecepcaoEvento4.asmx";
        public string InutilizacaoHomologacaoNFCe => "https://homologacao.nfce.fazenda.sp.gov.br/ws/NFeInutilizacao4.asmx";
    }
}
