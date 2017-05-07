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

        public async Task<(double largura, double altura)> ObterDimensoesWeb(bool deveSerInt)
        {
            var widthString = await webView.InvokeScriptAsync("eval", new[] { "document.body.scrollWidth.toString()" });
            var heightString = await webView.InvokeScriptAsync("eval", new[] { "document.body.scrollHeight.toString()" });
            return (deveSerInt ? int.Parse(widthString) : double.Parse(widthString), deveSerInt ? int.Parse(heightString) : double.Parse(heightString));
        }

        public (double largura, double altura) ObterDimensoesView()
        {
            return (webView.ActualWidth, webView.ActualHeight);
        }

        public void DefinirDimensoesView(double largura, double altura)
        {
            webView.Width = largura;
            webView.Height = altura;
        }

        public async Task CaptureWebView(IRandomAccessStream output)
        {
            var dimensoes = await ObterDimensoesWeb(true);
            DefinirDimensoesView(dimensoes.largura, dimensoes.altura);
            webView.UpdateLayout();

            await webView.CapturePreviewToStreamAsync(output);
            await output.FlushAsync();
        }
    }
}
