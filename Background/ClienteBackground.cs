using BibliotecaCentral.Log;
using BibliotecaCentral.Sincronizacao;
using Windows.ApplicationModel.Background;

namespace Background
{
    public sealed class ClienteBackground : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();
            var toast = new Toast();
            toast.Escrever(TitulosComuns.Iniciando, "Iniciando tarefa em background");
            var gerenc = new GerenciadorCliente(toast);
            await gerenc.Sincronizar(DadosSincronizaveis.Tudo, true);
            deferral.Complete();
        }
    }
}
