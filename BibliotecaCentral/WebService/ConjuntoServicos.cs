namespace BibliotecaCentral.WebService
{
    internal static class ConjuntoServicos
    {
        internal static DadosServico Autorizar = new DadosServico(Autorizarendereco, AutorizarServico, AutorizarMetodo);
        internal static DadosServico Consultar = new DadosServico(Consultarendereco, ConsultarServico, ConsultarMetodo);
        internal static DadosServico RespostaAutorizar = new DadosServico(RespostaAutorizarendereco, RespostaAutorizarServico, RespostaAutorizarMetodo);

        internal const string Autorizarendereco = "https://nfe-homologacao.svrs.rs.gov.br/ws/NfeAutorizacao/NFeAutorizacao.asmx"; //"https://nfe.svrs.rs.gov.br/ws/NfeAutorizacao/NFeAutorizacao.asmx";
        internal const string AutorizarServico = "http://www.portalfiscal.inf.br/nfe/wsdl/NfeAutorizacao";
        internal const string AutorizarMetodo = "http://www.portalfiscal.inf.br/nfe/wsdl/NfeAutorizacao/nfeAutorizacaoLote";

        internal const string Consultarendereco = "https://nfe.svrs.rs.gov.br/ws/NfeConsulta/NfeConsulta2.asmx";
        internal const string ConsultarServico = "http://www.portalfiscal.inf.br/nfe/wsdl/NfeConsulta2";
        internal const string ConsultarMetodo = "http://www.portalfiscal.inf.br/nfe/wsdl/NfeConsulta2/nfeConsultaNF2";

        internal const string RespostaAutorizarendereco = "https://nfe-homologacao.svrs.rs.gov.br/ws/NfeRetAutorizacao/NFeRetAutorizacao.asmx"; //"https://nfe.svrs.rs.gov.br/ws/NfeRetAutorizacao/NFeRetAutorizacao.asmx";
        internal const string RespostaAutorizarServico = "http://www.portalfiscal.inf.br/nfe/wsdl/NfeRetAutorizacao";
        internal const string RespostaAutorizarMetodo = "http://www.portalfiscal.inf.br/nfe/wsdl/NfeRetAutorizacao/nfeRetAutorizacaoLote";
    }
}
