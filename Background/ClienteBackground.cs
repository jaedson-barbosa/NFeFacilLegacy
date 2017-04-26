using BibliotecaCentral.Log;
using BibliotecaCentral.Sincronizacao;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace Background
{
    public sealed class ClienteBackground : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();
            var toast = new Toast();
            try
            {
                toast.Escrever(TitulosComuns.Iniciando, "Iniciando sincronização em background.");
                var task = Task.Run(() => new GerenciadorCliente(toast).Sincronizar(DadosSincronizaveis.Tudo, true));
                task.Wait();
            }
            catch (System.Exception e)
            {
                toast.Escrever(TitulosComuns.ErroCatastrófico, e.Message);
            }
            deferral.Complete();
        }
    }
}
