namespace NFeFacil.WebService.WebServices
{
    internal struct SVRS : IWebService
    {
        public string ConsultarProducao => "https://nfe.svrs.rs.gov.br/ws/NfeConsulta/NfeConsulta2.asmx";
        public string AutorizarProducao => "https://nfe.svrs.rs.gov.br/ws/NfeAutorizacao/NFeAutorizacao.asmx";
        public string RespostaAutorizarProducao => "https://nfe.svrs.rs.gov.br/ws/NfeRetAutorizacao/NFeRetAutorizacao.asmx";
        public string RecepcaoEventoProducao => "https://nfe.svrs.rs.gov.br/ws/recepcaoevento/recepcaoevento.asmx";

        public string ConsultarHomologacao => "https://nfe-homologacao.svrs.rs.gov.br/ws/NfeConsulta/NfeConsulta2.asmx";
        public string AutorizarHomologacao => "https://nfe-homologacao.svrs.rs.gov.br/ws/NfeAutorizacao/NFeAutorizacao.asmx";
        public string RespostaAutorizarHomologacao => "https://nfe-homologacao.svrs.rs.gov.br/ws/NfeRetAutorizacao/NFeRetAutorizacao.asmx";
        public string RecepcaoEventoHomologacao => "https://nfe-homologacao.svrs.rs.gov.br/ws/recepcaoevento/recepcaoevento.asmx";

        public string VersaoRecepcaoEvento => "1.00";
    }
}
