using BibliotecaCentral.Log;
using BibliotecaCentral.ModeloXML;
using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace NFeFacil.DANFE
{
    public sealed class GerenciadorExportacao : GerenciadorWebView
    {
        private Rectangle retangulo;

        public GerenciadorExportacao(Processo processo, ref WebView webView, ref Rectangle retangulo) : base(processo, ref webView)
        {
            this.retangulo = retangulo;
        }

        public async Task Salvar(byte[] pixels)
        {
            const string nome = "Imagens";
            var imagens = await ApplicationData.Current.LocalFolder.CreateFolderAsync(nome, CreationCollisionOption.ReplaceExisting);

            var arquivo = await imagens.CreateFileAsync($"Página 0.png");
            using (IRandomAccessStream stream = await arquivo.OpenAsync(FileAccessMode.ReadWrite))
            {
                BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);
                encoder.SetPixelData(BitmapPixelFormat.Bgra8,
                                 BitmapAlphaMode.Ignore,
                                 (uint)retangulo.Width, (uint)retangulo.Height,
                                 0, 0, pixels);
                await encoder.FlushAsync();
            }

            await CriarArquivoZipAsync(imagens, $"NFe{Chave}");
            await imagens.DeleteAsync(StorageDeleteOption.PermanentDelete);
        }

        private async static Task CriarArquivoZipAsync(StorageFolder sourcefolder, string nomeArquivo)
        {
            var picker = new FolderPicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
                ViewMode = PickerViewMode.List,
                CommitButtonText = "Salvar aqui"
            };
            picker.FileTypeFilter.Add(".zip");
            var destinationfolder = await picker.PickSingleFolderAsync();
            ILog log = new Popup();
            if (destinationfolder != null)
            {
                StorageApplicationPermissions.FutureAccessList.AddOrReplace("PickedFolderToken", destinationfolder);
                await Task.Run(() =>
                {
                    var caminhoOrigem = sourcefolder.Path;
                    var caminhoDestino = $"{destinationfolder.Path}\\{nomeArquivo}.zip";
                    try
                    {
                        ZipFile.CreateFromDirectory(caminhoOrigem, caminhoDestino, CompressionLevel.Optimal, false);
                        log.Escrever(TitulosComuns.Sucesso, "DANFE salvo com sucesso.");
                    }
                    catch (IOException)
                    {
                        log.Escrever(TitulosComuns.ErroSimples, "Este arquivo já foi salvo nesta pasta.");
                    }
                });
            }
            else
            {
                log.Escrever(TitulosComuns.OperaçãoCancelada, "Não foi escolhida nenhuma pasta para salvar o arquivo.");
            }
        }
    }
}
