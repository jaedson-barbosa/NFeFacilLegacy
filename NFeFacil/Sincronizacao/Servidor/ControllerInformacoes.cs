using static NFeFacil.Sincronizacao.ConfiguracoesSincronizacao;
using NFeFacil.Sincronizacao.Pacotes;
using Restup.Webserver.Attributes;
using Restup.Webserver.Models.Contracts;
using Restup.Webserver.Models.Schemas;
using System;

namespace NFeFacil.Sincronizacao.Servidor
{
    [RestController(InstanceCreationType.PerCall)]
    internal sealed class ControllerInformacoes
    {
        [UriFormat("/BrechaSeguranca/{senha}")]
        public IGetResponse BrechaSeguranca(int senha)
        {
            if (GerenciadorServidor.Current.BrechaAberta)
            {
                if (senha != SenhaTemporária)
                    throw new SenhaErrada(senha);

                var resposta = new GetResponse(GetResponse.ResponseStatus.OK, new InfoSegurancaConexao
                {
                    Senha = SenhaPermanente
                });

                return resposta;
            }
            else
            {
                return new GetResponse(GetResponse.ResponseStatus.NotFound);
            }
        }
    }
}
