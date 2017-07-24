using NFeFacil.Sincronizacao.Servidor;
using Restup.Webserver.Http;
using Restup.Webserver.Rest;
using System;
using System.Threading.Tasks;

namespace NFeFacil.Sincronizacao
{
    public sealed class GerenciadorServidor
    {
        public static GerenciadorServidor Current { get; } = new GerenciadorServidor();

        public bool Rodando { get; private set; } = false;

        private GerenciadorServidor() { }

        public async Task IniciarServer()
        {
            var rest = new RestRouteHandler();
            rest.RegisterController<ControllerInformacoes>();
            rest.RegisterController<ControllerSincronizacaoNotas>();
            rest.RegisterController<ControllerSincronizacaoDadosBase>();

            var config = new HttpServerConfiguration()
                .RegisterRoute(rest)
                .ListenOnPort(8080)
                .EnableCors();
            var httpServer = new HttpServer(config);

            await httpServer.StartServerAsync();
            Rodando = true;
        }

        internal bool BrechaAberta { get; private set; }

        public void AbrirBrecha(TimeSpan tempoLimite) => BrechaAberta = true;
        public void FecharBrecha() => BrechaAberta = false;
    }
}
