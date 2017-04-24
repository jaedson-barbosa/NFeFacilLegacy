using BibliotecaCentral.Log;
using BibliotecaCentral.Sincronizacao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
