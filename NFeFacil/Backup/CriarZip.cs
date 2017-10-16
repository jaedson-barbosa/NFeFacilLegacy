using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Windows.Storage;

namespace NFeFacil.Backup
{
    static class CriarZip
    {
        const string Nome = "Backup";

        internal static async Task Zipar(string informacao)
        {
            var pasta = ApplicationData.Current.TemporaryFolder;
            pasta = await pasta.CreateFolderAsync("Temp", CreationCollisionOption.ReplaceExisting);
            var arquivo = await pasta.CreateFileAsync($"{Nome}.json");
            var stream = await arquivo.OpenStreamForWriteAsync();
            using (StreamWriter escritor = new StreamWriter(stream))
            {
                await escritor.WriteAsync(informacao);
                escritor.Flush();
            }

            pasta = ApplicationData.Current.TemporaryFolder;
            await Task.Run(() => ZipFile.CreateFromDirectory(pasta.Path + @"\Temp", $@"{pasta.Path}\Backup.zip", CompressionLevel.Optimal, false));
        }

        internal static async Task<StorageFile> RetornarArquivo()
        {
            var pasta = ApplicationData.Current.TemporaryFolder;
            return await pasta.GetFileAsync(Nome + ".zip");
        }

        internal static async Task ExcluirArquivo()
        {
            var arquivo = await RetornarArquivo();
            await arquivo.DeleteAsync(StorageDeleteOption.PermanentDelete).AsTask();
        }
    }
}
