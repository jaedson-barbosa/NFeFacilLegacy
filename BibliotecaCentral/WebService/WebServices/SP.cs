namespace BibliotecaCentral.WebService.WebServices
{
    internal struct SP : IWebService
    {
        public string ConsultarProducao => "https://nfe.fazenda.sp.gov.br/ws/nfeconsulta2.asmx";
        public string AutorizarProducao => "https://nfe.fazenda.sp.gov.br/ws/nfeautorizacao.asmx";
        public string RespostaAutorizarProducao => "https://nfe.fazenda.sp.gov.br/ws/nferetautorizacao.asmx";
        public string RecepcaoEventoProducao => "https://nfe.fazenda.sp.gov.br/ws/recepcaoevento.asmx";

        public string ConsultarHomologacao => "https://homologacao.nfe.fazenda.sp.gov.br/ws/nfeconsulta2.asmx";
        public string AutorizarHomologacao => "https://homologacao.nfe.fazenda.sp.gov.br/ws/nfeautorizacao.asmx";
        public string RespostaAutorizarHomologacao => "https://homologacao.nfe.fazenda.sp.gov.br/ws/nferetautorizacao.asmx";
        public string RecepcaoEventoHomologacao => "https://homologacao.nfe.fazenda.sp.gov.br/ws/recepcaoevento.asmx";

        public string VersaoRecepcaoEvento => "1.00";
    }
}
