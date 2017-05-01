namespace BibliotecaCentral.WebService.WebServices
{
    internal struct SVAN : IWebService
    {
        public string ConsultarProducao => "https://www.sefazvirtual.fazenda.gov.br/NfeConsulta2/NfeConsulta2.asmx";
        public string AutorizarProducao => "https://www.sefazvirtual.fazenda.gov.br/NfeAutorizacao/NfeAutorizacao.asmx";
        public string RespostaAutorizarProducao => "https://www.sefazvirtual.fazenda.gov.br/NfeRetAutorizacao/NfeRetAutorizacao.asmx";
        public string RecepcaoEventoProducao => "https://www.sefazvirtual.fazenda.gov.br/RecepcaoEvento/RecepcaoEvento.asmx";

        public string ConsultarHomologacao => "https://hom.sefazvirtual.fazenda.gov.br/NfeConsulta2/NfeConsulta2.asmx";
        public string AutorizarHomologacao => "https://hom.sefazvirtual.fazenda.gov.br/NfeAutorizacao/NfeAutorizacao.asmx";
        public string RespostaAutorizarHomologacao => "https://hom.sefazvirtual.fazenda.gov.br/NfeRetAutorizacao/NfeRetAutorizacao.asmx";
        public string RecepcaoEventoHomologacao => "https://hom.sefazvirtual.fazenda.gov.br/RecepcaoEvento/RecepcaoEvento.asmx";

        public string VersaoRecepcaoEvento => "1.00";
    }
}
