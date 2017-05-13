using BibliotecaCentral.Log;
using BibliotecaCentral.Sincronizacao.Servidor;
using Restup.Webserver.Http;
using Restup.Webserver.Rest;
using System;
using System.Threading.Tasks;

namespace BibliotecaCentral.Sincronizacao
{
    public sealed class GerenciadorServidor
    {
        public bool Rodando { get; private set; } = false;
        private ILog Log;

        public GerenciadorServidor(ILog log)
        {
            Log = log;
        }

        public async Task IniciarServer()
        {
            Log.Escrever(TitulosComuns.Iniciando, "Criando servidor REST");
            var rest = new RestRouteHandler();

            Log.Escrever(TitulosComuns.Processando, "Registrando e configurando servidor.");
            rest.RegisterController<ControllerInformacoes>();
            rest.RegisterController<ControllerSincronizacaoNotas>();
            rest.RegisterController<ControllerSincronizacaoDadosBase>();

            var config = new HttpServerConfiguration()
                .RegisterRoute(rest)
                .ListenOnPort(8080)
                .EnableCors();
            var httpServer = new HttpServer(config);

            Log.Escrever(TitulosComuns.Iniciando, "Iniciando serviço de sincronização");
            await httpServer.StartServerAsync();

            Log.Escrever(TitulosComuns.Sucesso, "Serviço iniciado.");
            Rodando = true;
        }

        public static bool BrechaAberta { get; private set; }

        public void AbrirBrecha(TimeSpan tempoLimite)
        {
            BrechaAberta = true;
            string strExtra;
            if (tempoLimite.Minutes == 1)
                strExtra = "Abrindo brecha na segurança por um minuto.";
            else
                strExtra = $"Abrindo brecha na segurança por {tempoLimite.Seconds} segundos.";
            Log.Escrever(TitulosComuns.Log, strExtra);
        }

        public void FecharBrecha()
        {
            BrechaAberta = false;
        }
    }
}
