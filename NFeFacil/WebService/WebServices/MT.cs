﻿namespace NFeFacil.WebService.WebServices
{
    internal struct MT : IWebService
    {
        public string ConsultarProducao => "https://nfe.sefaz.mt.gov.br/nfews/v2/services/NfeConsulta2?wsdl";
        public string AutorizarProducao => "https://nfe.sefaz.mt.gov.br/nfews/v2/services/NfeAutorizacao?wsdl";
        public string RespostaAutorizarProducao => "https://nfe.sefaz.mt.gov.br/nfews/v2/services/NfeRetAutorizacao?wsdl";
        public string RecepcaoEventoProducao => "https://nfe.sefaz.mt.gov.br/nfews/v2/services/RecepcaoEvento?wsdl";
        public string InutilizacaoProducao => "https://nfe.sefaz.mt.gov.br/nfews/v2/services/NfeInutilizacao2?wsdl";

        public string ConsultarHomologacao => "https://homologacao.sefaz.mt.gov.br/nfews/v2/services/NfeConsulta2?wsdl";
        public string AutorizarHomologacao => "https://homologacao.sefaz.mt.gov.br/nfews/v2/services/NfeAutorizacao?wsdl";
        public string RespostaAutorizarHomologacao => "https://homologacao.sefaz.mt.gov.br/nfews/v2/services/NfeRetAutorizacao?wsdl";
        public string RecepcaoEventoHomologacao => "https://homologacao.sefaz.mt.gov.br/nfews/v2/services/RecepcaoEvento?wsdl";
        public string InutilizacaoHomologacao => "https://homologacao.sefaz.mt.gov.br/nfews/v2/services/NfeInutilizacao2";
    }
}
