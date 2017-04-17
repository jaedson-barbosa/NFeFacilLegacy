using NFeFacil.ModeloXML;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Graphics.Printing;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Printing;
using Windows.UI.Xaml.Shapes;

namespace NFeFacil.DANFE
{
    public sealed class GerenciadorImpressao : GerenciadorWebView, IDisposable
    {
        public event EventHandler PaginasCarregadas;
        private void OnPaginasCarregadas()
        {
            PaginasCarregadas?.Invoke(this, new EventArgs());
        }

        private PrintDocument printDoc;
        private IPrintDocumentSource printDocSource;
        private List<UIElement> paginas = new List<UIElement>();

        public GerenciadorImpressao(Processo processo, ref WebView webView) : base(processo, ref webView)
        {
            RegistrarImpressão();
            webView.NavigationCompleted += async (x, y) =>
            {
                await ExibiçãoDados.ExibirTodasAsPáginas();
                OnPaginasCarregadas();
            };
        }

        private void RegistrarImpressão()
        {
            printDoc = new PrintDocument();
            printDocSource = printDoc.DocumentSource;
            printDoc.Paginate += PrintDoc_Paginate;
            printDoc.GetPreviewPage += PrintDoc_GetPreviewPage;
            printDoc.AddPages += PrintDoc_AddPages; ;

            var printMan = PrintManager.GetForCurrentView();
            printMan.PrintTaskRequested += PrintTaskRequested;
        }

        private void DesregistrarImpressão()
        {
            if (printDoc == null) return;
            printDoc.Paginate -= PrintDoc_Paginate;
            printDoc.GetPreviewPage -= PrintDoc_GetPreviewPage;
            printDoc.AddPages -= PrintDoc_AddPages;

            var printMan = PrintManager.GetForCurrentView();
            printMan.PrintTaskRequested -= PrintTaskRequested;
        }

        #region Eventos impressão
        private void PrintDoc_AddPages(object sender, AddPagesEventArgs e)
        {
            foreach (var item in paginas) printDoc.AddPage(item);
            printDoc.AddPagesComplete();
        }

        private void PrintDoc_GetPreviewPage(object sender, GetPreviewPageEventArgs e)
        {
            printDoc.SetPreviewPage(e.PageNumber, paginas[e.PageNumber - 1]);
        }

        private void PrintDoc_Paginate(object sender, PaginateEventArgs e)
        {
            printDoc.SetPreviewPageCount(paginas.Count, PreviewPageCountType.Final);
        }

        private void PrintTaskRequested(PrintManager sender, PrintTaskRequestedEventArgs args)
        {
            PrintTask printTask = null;
            printTask = args.Request.CreatePrintTask("Print", x =>
            {
                printTask.Options.MediaSize = PrintMediaSize.IsoA4;
                x.SetSource(printDocSource);
            });
        }
        #endregion

        public async Task Imprimir()
        {
            paginas.Clear();
            await ObterPaginasWeb(async i =>
            {
                var dimensoes = await UI.ObterDimensoesWeb(false);
                var imagem = new BitmapImage();
                using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
                {
                    await UI.CaptureWebView(stream);
                    await imagem.SetSourceAsync(stream);
                }
                paginas.Add(new Rectangle
                {
                    Height = dimensoes.Altura,
                    Width = dimensoes.Largura,
                    Margin = new Thickness(4),
                    Fill = new ImageBrush
                    {
                        ImageSource = imagem,
                        Stretch = Stretch.UniformToFill,
                        AlignmentY = AlignmentY.Top
                    }
                });
            });
            await PrintManager.ShowPrintUIAsync();
        }

        public void Dispose()
        {
            DesregistrarImpressão();
        }
    }
}
