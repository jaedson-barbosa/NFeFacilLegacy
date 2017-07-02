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
                Task.Run(() => new GerenciadorCliente(toast).Sincronizar(DadosSincronizaveis.Tudo, true)).Wait();
            }
            catch (System.Exception) { }
            deferral.Complete();
        }
    }
}
