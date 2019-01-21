using BaseGeral.Sincronizacao.FastServer;
using BaseGeral.Sincronizacao.Servidor;
using System.Threading.Tasks;

namespace BaseGeral.Sincronizacao
{
    public static class GerenciadorServidor
    {
        public static bool AceitarNovasConexoes { get; set; }
        public static bool Inativo { get; private set; } = true;

        public static async Task IniciarServer()
        {
            var server = new HttpServer(8080);
            server.RegisterController<ControllerInformacoes>();
            server.RegisterController<ControllerSincronizacao>();
            await server.StartServerAsync();
            Inativo = false;
        }
    }
}
