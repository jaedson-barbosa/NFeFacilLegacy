using BibliotecaCentral.WebService.WebServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaCentral.WebService
{
    internal sealed class EnderecosConexao
    {
        private string siglaUF;

        internal EnderecosConexao(string siglaUF)
        {
            this.siglaUF = siglaUF;
        }

        internal DadosServico ObterConjuntoConexao(bool homologacao, Operacoes operacaoRequirida)
        {
            var conjunto = ObterEnderecoConexao();
            string end;
            string metodo;
            string servico;
            switch (operacaoRequirida)
            {
                case Operacoes.Consultar:
                    end = homologacao ? conjunto.ConsultarHomologacao : conjunto.ConsultarProducao;
                    metodo = ConsultarMetodo;
                    servico = ConsultarServico;
                    break;
                case Operacoes.Autorizar:
                    end = homologacao ? conjunto.AutorizarHomologacao : conjunto.AutorizarProducao;
                    metodo = AutorizarMetodo;
                    servico = AutorizarServico;
                    break;
                case Operacoes.RespostaAutorizar:
                    end = homologacao ? conjunto.RespostaAutorizarHomologacao : conjunto.RespostaAutorizarProducao;
                    metodo = RespostaAutorizarMetodo;
                    servico = RespostaAutorizarServico;
                    break;
                case Operacoes.RecepcaoEvento:
                    end = homologacao ? conjunto.RecepcaoEventoHomologacao : conjunto.RecepcaoEventoProducao;
                    metodo = RecepcaoEventoMetodo;
                    servico = RecepcaoEventoServico;
                    break;
                default:
                    throw new ArgumentException();
            }
            return new DadosServico(end, servico, metodo); ;
        }

        private IWebService ObterEnderecoConexao()
        {
            string[] SVAN = { "MA", "PA", "PI" };
            string[] SVRS = { "AC", "AL", "AP", "DF", "ES", "PB", "RJ", "RN", "RO", "RR", "SC", "SE", "TO" };

            if (SVAN.Contains(siglaUF))
            {
                return new SVAN();
            }
            else if (SVRS.Contains(siglaUF))
            {
                return new SVRS();
            }
            else
            {
                throw new Exception("Estado ainda não suportado por esta aplicação.");
            }
        }

        internal const string ConsultarServico = "http://www.portalfiscal.inf.br/nfe/wsdl/NfeConsulta2";
        internal const string ConsultarMetodo = "http://www.portalfiscal.inf.br/nfe/wsdl/NfeConsulta2/nfeConsultaNF2";

        internal const string AutorizarServico = "http://www.portalfiscal.inf.br/nfe/wsdl/NfeAutorizacao";
        internal const string AutorizarMetodo = "http://www.portalfiscal.inf.br/nfe/wsdl/NfeAutorizacao/nfeAutorizacaoLote";

        internal const string RespostaAutorizarServico = "http://www.portalfiscal.inf.br/nfe/wsdl/NfeRetAutorizacao";
        internal const string RespostaAutorizarMetodo = "http://www.portalfiscal.inf.br/nfe/wsdl/NfeRetAutorizacao/nfeRetAutorizacaoLote";

        internal const string RecepcaoEventoServico = "http://www.portalfiscal.inf.br/nfe/wsdl/RecepcaoEvento";
        internal const string RecepcaoEventoMetodo = "http://www.portalfiscal.inf.br/nfe/wsdl/RecepcaoEvento/nfeRecepcaoEvento";
    }

    internal enum Operacoes
    {
        Consultar,
        Autorizar,
        RespostaAutorizar,
        RecepcaoEvento
    }
}
