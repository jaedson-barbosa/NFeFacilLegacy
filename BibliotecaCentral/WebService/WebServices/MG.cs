namespace BibliotecaCentral.WebService.WebServices
{
    internal struct MG : IWebService
    {
        public string ConsultarProducao => "https://nfe.fazenda.mg.gov.br/nfe2/services/NfeConsulta2";
        public string AutorizarProducao => "https://nfe.fazenda.mg.gov.br/nfe2/services/NfeAutorizacao";
        public string RespostaAutorizarProducao => "https://nfe.fazenda.mg.gov.br/nfe2/services/NfeRetAutorizacao";
        public string RecepcaoEventoProducao => "https://nfe.fazenda.mg.gov.br/nfe2/services/RecepcaoEvento";

        public string ConsultarHomologacao => "https://hnfe.fazenda.mg.gov.br/nfe2/services/NfeConsulta2";
        public string AutorizarHomologacao => "https://hnfe.fazenda.mg.gov.br/nfe2/services/NfeAutorizacao";
        public string RespostaAutorizarHomologacao => "https://hnfe.fazenda.mg.gov.br/nfe2/services/NfeRetAutorizacao";
        public string RecepcaoEventoHomologacao => "https://hnfe.fazenda.mg.gov.br/nfe2/services/RecepcaoEvento";

        public string VersaoRecepcaoEvento => "1.00";
    }
}
