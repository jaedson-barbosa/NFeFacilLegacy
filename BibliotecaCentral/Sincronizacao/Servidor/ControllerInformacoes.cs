using static BibliotecaCentral.Sincronizacao.ConfiguracoesSincronizacao;
using BibliotecaCentral.Sincronizacao.Pacotes;
using Restup.Webserver.Attributes;
using Restup.Webserver.Models.Contracts;
using Restup.Webserver.Models.Schemas;
using System;

namespace BibliotecaCentral.Sincronizacao.Servidor
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

        [UriFormat("/Configuracoes/{senha}")]
        public IGetResponse Configuracoes(int senha)
        {
            if (senha != SenhaPermanente)
                throw new SenhaErrada(senha);

            return new GetResponse(GetResponse.ResponseStatus.OK, new ConfiguracoesServidor
            {
                DadosBase = SincDadoBase,
                Notas = SincNotaFiscal
            });
        }
    }
}
