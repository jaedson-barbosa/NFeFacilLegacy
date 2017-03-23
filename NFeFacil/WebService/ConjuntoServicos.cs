namespace NFeFacil.WebService
{
    internal static class ConjuntoServicos
    {
        internal static DadosServico Autorizar = new DadosServico(AutorizarEndereco, AutorizarServico, AutorizarMetodo);
        internal static DadosServico Consultar = new DadosServico(ConsultarEndereco, ConsultarServico, ConsultarMetodo);
        internal static DadosServico RespostaAutorizar = new DadosServico(RespostaAutorizarEndereco, RespostaAutorizarServico, RespostaAutorizarMetodo);

        internal const string AutorizarEndereco = "https://nfe.svrs.rs.gov.br/ws/NfeAutorizacao/NFeAutorizacao.asmx";
        internal const string AutorizarServico = "http://www.portalfiscal.inf.br/nfe/wsdl/NfeAutorizacao";
        internal const string AutorizarMetodo = "http://www.portalfiscal.inf.br/nfe/wsdl/NfeAutorizacao/nfeAutorizacaoLote";

        internal const string ConsultarEndereco = "https://nfe.svrs.rs.gov.br/ws/NfeConsulta/NfeConsulta2.asmx";
        internal const string ConsultarServico = "http://www.portalfiscal.inf.br/nfe/wsdl/NfeConsulta2";
        internal const string ConsultarMetodo = "http://www.portalfiscal.inf.br/nfe/wsdl/NfeConsulta2/nfeConsultaNF2";

        internal const string RespostaAutorizarEndereco = "https://nfe.svrs.rs.gov.br/ws/NfeRetAutorizacao/NFeRetAutorizacao.asmx";
        internal const string RespostaAutorizarServico = "http://www.portalfiscal.inf.br/nfe/wsdl/NfeRetAutorizacao";
        internal const string RespostaAutorizarMetodo = "http://www.portalfiscal.inf.br/nfe/wsdl/NfeRetAutorizacao/nfeRetAutorizacaoLote";
    }
}
