using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace NFeFacil.Backup
{
    struct CriarZip : IDisposable
    {
        StorageFile Arquivo;

        public void Dispose()
        {
            var pasta = ApplicationData.Current.TemporaryFolder;
            var task = Arquivo.DeleteAsync(StorageDeleteOption.PermanentDelete).AsTask();
            task.Start();
            task.Wait();
            GC.SuppressFinalize(this);
        }

        internal async Task<StorageFile> Zipar(string nome, string informacao)
        {
            var pasta = ApplicationData.Current.TemporaryFolder;
            Arquivo = await pasta.CreateFileAsync(nome);
            var stream = await Arquivo.OpenStreamForWriteAsync();
            using (StreamWriter escritor = new StreamWriter(stream))
            {
                await escritor.WriteAsync(informacao);
                escritor.Flush();
            }
            System.IO.Compression.ZipFile.CreateFromDirectory(pasta.Path, $@"{pasta.Path}\{nome}.zip");
            return await pasta.GetFileAsync($"{nome}.zip");
        }
    }
}
