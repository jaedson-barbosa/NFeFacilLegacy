namespace Fiscal.WebService
{
    sealed class GenericWebService : IWebService
    {
        public string ConsultarProducao { get; }
        public string AutorizarProducao { get; }
        public string RespostaAutorizarProducao { get; }
        public string RecepcaoEventoProducao { get; }
        public string InutilizacaoProducao { get; }

        public string ConsultarHomologacao { get; }
        public string AutorizarHomologacao { get; }
        public string RespostaAutorizarHomologacao { get; }
        public string RecepcaoEventoHomologacao { get; }
        public string InutilizacaoHomologacao { get; }

        public GenericWebService(IWebService original, bool isNFCe)
        {
            if (isNFCe && original is IWebServiceProducaoNFCe producao)
            {
                ConsultarProducao = producao.ConsultarProducaoNFCe;
                AutorizarProducao = producao.AutorizarProducaoNFCe;
                RespostaAutorizarProducao = producao.RespostaAutorizarProducaoNFCe;
                RecepcaoEventoProducao = producao.RecepcaoEventoProducaoNFCe;
                InutilizacaoProducao = producao.InutilizacaoProducaoNFCe;
            }
            else
            {
                ConsultarProducao = original.ConsultarProducao;
                AutorizarProducao = original.AutorizarProducao;
                RespostaAutorizarProducao = original.RespostaAutorizarProducao;
                RecepcaoEventoProducao = original.RecepcaoEventoProducao;
                InutilizacaoProducao = original.InutilizacaoProducao;
            }

            if (isNFCe && original is IWebServiceHomologacaoNFCe homologacao)
            {
                ConsultarHomologacao = homologacao.ConsultarHomologacaoNFCe;
                AutorizarHomologacao = homologacao.AutorizarHomologacaoNFCe;
                RespostaAutorizarHomologacao = homologacao.RespostaAutorizarHomologacaoNFCe;
                RecepcaoEventoHomologacao = homologacao.RecepcaoEventoHomologacaoNFCe;
                InutilizacaoHomologacao = homologacao.InutilizacaoHomologacaoNFCe;
            }
            else
            {
                ConsultarHomologacao = original.ConsultarHomologacao;
                AutorizarHomologacao = original.AutorizarHomologacao;
                RespostaAutorizarHomologacao = original.RespostaAutorizarHomologacao;
                RecepcaoEventoHomologacao = original.RecepcaoEventoHomologacao;
                InutilizacaoHomologacao = original.InutilizacaoHomologacao;
            }
        }
    }
}
