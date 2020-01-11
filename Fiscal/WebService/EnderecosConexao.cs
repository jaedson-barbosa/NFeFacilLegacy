using Fiscal.WebService.WebServices;
using System;
using System.Linq;

namespace Fiscal.WebService
{
    internal struct EnderecosConexao
    {
        private string siglaUF;

        internal EnderecosConexao(string siglaUF)
        {
            this.siglaUF = siglaUF;
        }

        internal DadosServico ObterConjuntoConexao(bool homologacao, Operacoes operacaoRequirida, bool isNFCe)
        {
            var conjunto = ObterEnderecoConexao();
            conjunto = new GenericWebService(conjunto, isNFCe);
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
                case Operacoes.Inutilizacao:
                    end = homologacao ? conjunto.InutilizacaoHomologacao : conjunto.InutilizacaoProducao;
                    metodo = InutilizacaoMetodo;
                    servico = InutilizacaoServico;
                    break;
                default:
                    throw new ArgumentException();
            }
            return new DadosServico(end, servico, metodo);
        }

        private IWebService ObterEnderecoConexao()
        {
            string[] SVAN = { "MA", "PA" };
            string[] SVRS = { "AC", "AL", "AP", "DF", "ES", "PB", "RJ", "RN", "RO", "RR", "SC", "SE", "TO", "PI" };

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
                switch (siglaUF)
                {
                    case "AM":
                        return new AM();
                    case "BA":
                        return new BA();
                    case "CE":
                        return new CE();
                    case "GO":
                        return new GO();
                    case "MG":
                        return new MG();
                    case "MS":
                        return new MS();
                    case "MT":
                        return new MT();
                    case "PE":
                        return new PE();
                    case "PR":
                        return new PR();
                    case "RS":
                        return new RS();
                    case "SP":
                        return new SP();
                    default:
                        throw new Exception("Estado ainda não cadastrado no aplicativo.");
                }
            }
        }

        const string ConsultarServico = "http://www.portalfiscal.inf.br/nfe/wsdl/NFeConsultaProtocolo4";
        const string ConsultarMetodo = "http://www.portalfiscal.inf.br/nfe/wsdl/NFeConsultaProtocolo4/nfeConsultaNF";

        const string AutorizarServico = "http://www.portalfiscal.inf.br/nfe/wsdl/NFeAutorizacao4";
        const string AutorizarMetodo = "http://www.portalfiscal.inf.br/nfe/wsdl/NFeAutorizacao4/nfeAutorizacaoLote";

        const string RespostaAutorizarServico = "http://www.portalfiscal.inf.br/nfe/wsdl/NFeRetAutorizacao4";
        const string RespostaAutorizarMetodo = "http://www.portalfiscal.inf.br/nfe/wsdl/NFeRetAutorizacao4/nfeRetAutorizacaoLote";

        const string RecepcaoEventoServico = "http://www.portalfiscal.inf.br/nfe/wsdl/NFeRecepcaoEvento4";
        const string RecepcaoEventoMetodo = "http://www.portalfiscal.inf.br/nfe/wsdl/NFeRecepcaoEvento4/nfeRecepcaoEvento";

        const string InutilizacaoServico = "http://www.portalfiscal.inf.br/nfe/wsdl/NFeInutilizacao4";
        const string InutilizacaoMetodo = "http://www.portalfiscal.inf.br/nfe/wsdl/NFeInutilizacao4/nfeInutilizacaoNF";
    }
}
