using System;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Popups;

namespace NFeFacil.Log
{
    public sealed class Popup
    {
        const CoreDispatcherPriority Prioridade = CoreDispatcherPriority.Normal;

        private Popup() { }

        public async void Escrever(TitulosComuns título, string texto)
        {
            var mensag = new MessageDialog(texto, Titulos.ObterString(título));
            var dispachante = CoreApplication.MainView.CoreWindow.Dispatcher;
            await dispachante.RunAsync(Prioridade, async () =>
            {
                await mensag.ShowAsync();
            });
        }

        public static readonly Popup Current = new Popup();
    }
}
