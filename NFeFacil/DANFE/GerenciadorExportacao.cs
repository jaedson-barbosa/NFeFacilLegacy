using BibliotecaCentral.Log;
using BibliotecaCentral.ModeloXML;
using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;

namespace NFeFacil.DANFE
{
    public sealed class GerenciadorExportacao : GerenciadorWebView
    {
        public GerenciadorExportacao(Processo processo, ref WebView webView) : base(processo, ref webView)
        { }

        public async Task Salvar()
        {
            const string nome = "Imagens";
            var imagens = await ApplicationData.Current.LocalFolder.CreateFolderAsync(nome, CreationCollisionOption.ReplaceExisting);
            await ObterPaginasWeb(async i =>
            {
                var arquivo = await imagens.CreateFileAsync($"Página {i + 1}.png");
                using (IRandomAccessStream stream = await arquivo.OpenAsync(FileAccessMode.ReadWrite))
                {
                    await UI.CaptureWebView(stream);
                }
            });
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
