using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage;

namespace NFeFacil
{
    internal sealed class PastaNotasFiscais
    {
        private StorageFolder PastaArquivos = ApplicationData.Current.LocalFolder;

        public async Task AdicionarOuAtualizar(XElement xml, string nome)
        {
            var arq = await PastaArquivos.CreateFileAsync(nome + ".xml", CreationCollisionOption.ReplaceExisting);
            using (var stream = await arq.OpenStreamForWriteAsync())
            {
                xml.Save(stream);
                await stream.FlushAsync();
            }
        }

        public async Task Remover(string nome)
        {
            try
            {
                var arq = await PastaArquivos.GetFileAsync(nome + ".xml");
                await arq.DeleteAsync(StorageDeleteOption.PermanentDelete);
            }
            catch (FileNotFoundException) { }
            catch (Exception) { throw; }
        }

        public async Task<XElement> Retornar(string nome)
        {
            var arq = await PastaArquivos.GetFileAsync(nome + ".xml");
            using (var stream = await arq.OpenStreamForReadAsync())
            {
                return XElement.Load(stream);
            }
        }

        public async Task<XElement[]> RegistroCompleto()
        {
            var arqs = await PastaArquivos.GetFilesAsync();
            var quantidade = arqs.Count(x => x.FileType == ".xml");
            var retorno = new XElement[quantidade];
            for (int i = 0; i < arqs.Count(); i++)
            {
                if (arqs.ElementAt(i).FileType == ".xml")
                {
                    using (var stream = await arqs.ElementAt(i).OpenStreamForReadAsync())
                    {
                        retorno[i] = XElement.Load(stream);
                    }
                }
            }
            return retorno;
        }
    }
}
