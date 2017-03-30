using System;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;

namespace NFeFacil.DANFE.Processamento
{
    public sealed class ViewUI
    {
        private WebView webView;

        public ViewUI(ref WebView view)
        {
            webView = view;
        }

        public async Task<Dimensoes> ObterDimensoesWeb(bool deveSerInt)
        {
            var widthString = await webView.InvokeScriptAsync("eval", new[] { "document.body.scrollWidth.toString()" });
            var heightString = await webView.InvokeScriptAsync("eval", new[] { "document.body.scrollHeight.toString()" });
            return new Dimensoes
            {
                Largura = deveSerInt ? int.Parse(widthString) : double.Parse(widthString),
                Altura = deveSerInt ? int.Parse(heightString) : double.Parse(heightString)
            };
        }

        public Dimensoes ObterDimensoesView()
        {
            return new Dimensoes
            {
                Largura = webView.ActualWidth,
                Altura = webView.ActualHeight
            };
        }

        public void DefinirDimensoesView(Dimensoes dimensoes)
        {
            webView.Width = dimensoes.Largura;
            webView.Height = dimensoes.Altura;
        }

        public async Task CaptureWebView(IRandomAccessStream output)
        {
            var dimensoes = await ObterDimensoesWeb(true);
            DefinirDimensoesView(dimensoes);
            webView.UpdateLayout();

            await webView.CapturePreviewToStreamAsync(output);
            await output.FlushAsync();
        }
    }
}
