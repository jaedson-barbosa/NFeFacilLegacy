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
            try
            {
                toast.Escrever(TitulosComuns.Iniciando, "Iniciando sincronização em background.");
                await new GerenciadorCliente(toast).Sincronizar(DadosSincronizaveis.Tudo, true);
            }
            catch (System.Exception e)
            {
                toast.Escrever(TitulosComuns.ErroCatastrófico, e.Message);
            }
            deferral.Complete();
        }
    }
}
