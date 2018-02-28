namespace NFeFacil.WebService.WebServices
{
    internal struct SP : IWebService, IWebServiceProducaoNFCe, IWebServiceHomologacaoNFCe
    {
        public string ConsultarProducao => "https://nfe.fazenda.sp.gov.br/ws/nfeconsulta2.asmx";
        public string AutorizarProducao => "https://nfe.fazenda.sp.gov.br/ws/nfeautorizacao.asmx";
        public string RespostaAutorizarProducao => "https://nfe.fazenda.sp.gov.br/ws/nferetautorizacao.asmx";
        public string RecepcaoEventoProducao => "https://nfe.fazenda.sp.gov.br/ws/recepcaoevento.asmx";
        public string InutilizacaoProducao => "https://nfe.fazenda.sp.gov.br/ws/nfeinutilizacao2.asmx";

        public string ConsultarHomologacao => "https://homologacao.nfe.fazenda.sp.gov.br/ws/nfeconsulta2.asmx";
        public string AutorizarHomologacao => "https://homologacao.nfe.fazenda.sp.gov.br/ws/nfeautorizacao.asmx";
        public string RespostaAutorizarHomologacao => "https://homologacao.nfe.fazenda.sp.gov.br/ws/nferetautorizacao.asmx";
        public string RecepcaoEventoHomologacao => "https://homologacao.nfe.fazenda.sp.gov.br/ws/recepcaoevento.asmx";
        public string InutilizacaoHomologacao => "https://homologacao.nfe.fazenda.sp.gov.br/ws/nfeinutilizacao2.asmx";

        public string ConsultarProducaoNFCe => "https://nfce.fazenda.sp.gov.br/ws/NFeConsulta2.asmx";
        public string AutorizarProducaoNFCe => "https://nfce.fazenda.sp.gov.br/ws/NFeAutorizacao.asmx";
        public string RespostaAutorizarProducaoNFCe => "https://nfce.fazenda.sp.gov.br/ws/NFeRetAutorizacao.asmx";
        public string RecepcaoEventoProducaoNFCe => "https://nfce.fazenda.sp.gov.br/ws/RecepcaoEvento.asmx";
        public string InutilizacaoProducaoNFCe => "https://nfce.fazenda.sp.gov.br/ws/NFeInutilizacao2.asmx";

        public string ConsultarHomologacaoNFCe => "https://homologacao.nfce.fazenda.sp.gov.br/ws/nfeconsulta2.asmx";
        public string AutorizarHomologacaoNFCe => "https://homologacao.nfce.fazenda.sp.gov.br/ws/nfeautorizacao.asmx";
        public string RespostaAutorizarHomologacaoNFCe => "https://homologacao.nfce.fazenda.sp.gov.br/ws/nferetautorizacao.asmx";
        public string RecepcaoEventoHomologacaoNFCe => "https://homologacao.nfce.fazenda.sp.gov.br/ws/recepcaoevento.asmx";
        public string InutilizacaoHomologacaoNFCe => "https://homologacao.nfce.fazenda.sp.gov.br/ws/nfeinutilizacao2.asmx";
    }
}
