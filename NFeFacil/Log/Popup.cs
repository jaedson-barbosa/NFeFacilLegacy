using System;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Popups;

namespace NFeFacil.Log
{
    public class Popup : ILog
    {
        private CoreDispatcherPriority Prioridade = CoreDispatcherPriority.Normal;
        private CoreDispatcher Dispachante { get; } = CoreApplication.MainView.CoreWindow.Dispatcher;

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
