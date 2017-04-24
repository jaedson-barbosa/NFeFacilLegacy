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
            var gerenc = new GerenciadorCliente(new Toast());
            await gerenc.Sincronizar(DadosSincronizaveis.Tudo, true);
            deferral.Complete();
        }
    }
}
