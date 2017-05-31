using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Graphics.Printing;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Printing;

namespace NFeFacil.DANFE
{
    public sealed class GerenciadorImpressao : IDisposable
    {
        private PrintDocument printDoc;
        private IPrintDocumentSource printDocSource;
        private List<UIElement> paginas = new List<UIElement>();

        public GerenciadorImpressao()
        {
            RegistrarImpressão();
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
                printTask.Options.PrintQuality = PrintQuality.High;
                x.SetSource(printDocSource);
            });
        }
        #endregion

        public async Task Imprimir(UIElement rect)
        {
            paginas.Clear();
            paginas.Add(rect);
            await PrintManager.ShowPrintUIAsync();
        }

        public void Dispose()
        {
            DesregistrarImpressão();
        }
    }
}
