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
            rest.RegisterController<ControllerSincronizacao>();

            await new HttpServer(new HttpServerConfiguration()
                .RegisterRoute(rest)
                .ListenOnPort(8080)
                .EnableCors()).StartServerAsync();
            Rodando = true;
        }

        internal bool BrechaAberta { get; private set; }

        public void AbrirBrecha(TimeSpan tempoLimite) => BrechaAberta = true;
        public void FecharBrecha() => BrechaAberta = false;
    }
}
