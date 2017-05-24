using Windows.Graphics.Display;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace NFeFacil.DANFE.Processamento
{
    public sealed class ViewUI
    {
        private WebView webView;

        public ViewUI(ref WebView view)
        {
            webView = view;
        }

        public (double largura, double altura) DimensoesWeb
        {
            get
            {
                return (CentimeterToPixel(21), CentimeterToPixel(29.7));

                int CentimeterToPixel(double Centimeter)
                {
                    var dpi = DisplayInformation.GetForCurrentView().LogicalDpi;
                    return (int)(Centimeter * dpi / 2.54d);
                }
            }
        }

        public (double largura, double altura) ObterDimensoesView()
        {
            return (webView.ActualWidth, webView.ActualHeight);
        }

        public WebViewBrush CaptureWebView()
        {
            var _Brush = new WebViewBrush
            {
                Stretch = Stretch.Uniform
            };
            _Brush.SetSource(webView);
            _Brush.Redraw();
            return _Brush;
        }
    }
}
