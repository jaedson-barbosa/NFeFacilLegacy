using System;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Popups;

namespace BibliotecaCentral.Log
{
    public struct Popup : ILog
    {
        private CoreDispatcherPriority Prioridade => CoreDispatcherPriority.Normal;
        private CoreDispatcher Dispachante => CoreApplication.MainView.CoreWindow.Dispatcher;

        public async void Escrever(TitulosComuns título, string texto)
        {
            var mensag = new MessageDialog(texto, Titulos.ObterString(título));
            await Dispachante.RunAsync(Prioridade, async () =>
            {
                await mensag.ShowAsync();
            });
        }
    }
}
