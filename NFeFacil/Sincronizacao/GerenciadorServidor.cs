using NFeFacil.Sincronizacao.FastServer;
using NFeFacil.Sincronizacao.Servidor;
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
            var server = new HttpServer(8080);
            server.RegisterController<ControllerInformacoes>();
            server.RegisterController<ControllerSincronizacao>();
            await server.StartServerAsync();
            Rodando = true;
        }

        internal bool BrechaAberta { get; private set; }

        public void AbrirBrecha(TimeSpan tempoLimite) => BrechaAberta = true;
        public void FecharBrecha() => BrechaAberta = false;
    }
}
