using Windows.UI.Notifications;

namespace NFeFacil.Log
{
    public sealed class Toast : ILog
    {
        public void Escrever(TitulosComuns título, string texto)
        {
            var toastNotifier = ToastNotificationManager.CreateToastNotifier();
            var toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
            var textNodes = toastXml.GetElementsByTagName("text");
            textNodes[0].AppendChild(toastXml.CreateTextNode(título.ToString()));
            textNodes[1].AppendChild(toastXml.CreateTextNode(texto));
            toastNotifier.Show(new ToastNotification(toastXml));
        }
    }
}
