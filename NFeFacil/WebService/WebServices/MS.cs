namespace NFeFacil.WebService.WebServices
{
    internal struct MS : IWebService
    {
        public string ConsultarProducao => "https://nfe.fazenda.ms.gov.br/producao/services2/CadConsultaCadastro2";
        public string AutorizarProducao => "https://nfe.fazenda.ms.gov.br/producao/services2/NfeAutorizacao";
        public string RespostaAutorizarProducao => "https://nfe.fazenda.ms.gov.br/producao/services2/NfeRetAutorizacao";
        public string RecepcaoEventoProducao => "https://nfe.fazenda.ms.gov.br/producao/services2/RecepcaoEvento";

        public string ConsultarHomologacao => "https://homologacao.nfe.ms.gov.br/homologacao/services2/NfeConsulta2";
        public string AutorizarHomologacao => "https://homologacao.nfe.ms.gov.br/homologacao/services2/NfeAutorizacao";
        public string RespostaAutorizarHomologacao => "https://homologacao.nfe.ms.gov.br/homologacao/services2/NfeRetAutorizacao";
        public string RecepcaoEventoHomologacao => "https://homologacao.nfe.ms.gov.br/homologacao/services2/RecepcaoEvento";

        public string VersaoRecepcaoEvento => "1.00";
    }
}
