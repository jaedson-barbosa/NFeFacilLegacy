using BibliotecaCentral.Sincronizacao.Pacotes;
using Restup.Webserver.Attributes;
using Restup.Webserver.Models.Contracts;
using Restup.Webserver.Models.Schemas;
using System;
using Windows.ApplicationModel.Core;

namespace BibliotecaCentral.Sincronizacao.Servidor
{
    [RestController(InstanceCreationType.PerCall)]
    internal sealed class ControllerBrechaSeguranca
    {
        [UriFormat("/BrechaSeguranca/GET/{senha}")]
        public IGetResponse Get(int senha)
        {
            return SupervisionarOperacao.Iniciar(() =>
            {
                if ((bool)CoreApplication.Properties["BrechaAberta"])
                {
                    if (senha != Configuracoes.ConfiguracoesSincronizacao.SenhaTemporária)
                        throw new SenhaErrada(senha);
                    return new GetResponse(GetResponse.ResponseStatus.OK, new InfoSegurancaConexao
                    {
                        Senha = Configuracoes.ConfiguracoesSincronizacao.SenhaPermanente
                    });
                }
                else
                {
                    return new GetResponse(GetResponse.ResponseStatus.NotFound);
                }
            }, DateTime.Now, TipoDado.SenhaDeAcesso);
        }
    }
}
