using Microsoft.Toolkit.Uwp.Notifications;
using Windows.UI.Notifications;

namespace BibliotecaCentral.Log
{
    public struct Toast : ILog
    {
        public void Escrever(TitulosComuns título, string texto)
        {
            var conteudo = new ToastBindingGeneric();
            conteudo.Children.Add(new AdaptiveText()
            {
                Text = título.ToString()
            });
            conteudo.Children.Add(new AdaptiveText()
            {
                Text = texto
            });
            ToastContent toast = new ToastContent()
            {
                Visual = new ToastVisual
                {
                    BindingGeneric = conteudo,
                }
            };
            
            var toastNotifier = ToastNotificationManager.CreateToastNotifier();
            toastNotifier.Show(new ToastNotification(toast.GetXml()));

        }

        public void CriarTileInterativa(string nome)
        {
            var conteudo = new ToastBindingGeneric();
            conteudo.Children.Add(new AdaptiveText()
            {
                Text = "Liberar acesso ao servidor?"
            });
            conteudo.Children.Add(new AdaptiveText()
            {
                Text = $"O dispositivo de {nome} deseja se conectar a este servidor."
            });

            var acoes = new ToastActionsCustom();
            acoes.Buttons.Add(new ToastButton("Permitir", "Permitir"));
            acoes.Buttons.Add(new ToastButton("Negar", "Negar"));
            ToastContent toast = new ToastContent()
            {
                Visual = new ToastVisual
                {
                    BindingGeneric = conteudo,
                },
                Actions = acoes
            };
        }
    }
}
