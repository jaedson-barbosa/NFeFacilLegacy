using static BibliotecaCentral.Sincronizacao.ConfiguracoesSincronizacao;
using BibliotecaCentral.Sincronizacao.Pacotes;
using Restup.Webserver.Attributes;
using Restup.Webserver.Models.Contracts;
using Restup.Webserver.Models.Schemas;
using System;
using Windows.ApplicationModel.Core;

namespace BibliotecaCentral.Sincronizacao.Servidor
{
    [RestController(InstanceCreationType.PerCall)]
    internal sealed class ControllerInformacoes
    {
        [UriFormat("/BrechaSeguranca/{senha}")]
        public IGetResponse BrechaSeguranca(int senha)
        {
            return SupervisorOperacao.Supervisionar(() =>
            {
                if ((bool)CoreApplication.Properties["BrechaAberta"])
                {
                    if (senha != SenhaTemporária)
                        throw new SenhaErrada(senha);

                    return new GetResponse(GetResponse.ResponseStatus.OK, new InfoSegurancaConexao
                    {
                        Senha = ConfiguracoesSincronizacao.SenhaPermanente
                    });
                }
                else
                {
                    return new GetResponse(GetResponse.ResponseStatus.NotFound);
                }
            }, DateTime.Now, TipoDado.SenhaDeAcesso);
        }

        [UriFormat("/Configuracoes/{senha}")]
        public IGetResponse Configuracoes(int senha)
        {
            return SupervisorOperacao.Supervisionar(() =>
            {
                if (senha != SenhaPermanente)
                    throw new SenhaErrada(senha);

                return new GetResponse(GetResponse.ResponseStatus.OK, new Pacotes.ConfiguracoesServidor
                {
                    DadosBase = SincDadoBase,
                    Notas = SincNotaFiscal
                });
            }, DateTime.Now, TipoDado.Configuracao);
        }
    }
}
