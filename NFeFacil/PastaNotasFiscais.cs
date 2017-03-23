using System;
using System.Collections.Generic;
using System.IO;
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

        public async Task<IEnumerable<XElement>> RegistroCompleto()
        {
            var arqs = await PastaArquivos.GetFilesAsync();
            var retorno = new List<XElement>();
            foreach (var item in arqs)
            {
                if (item.FileType == ".xml")
                {
                    using (var stream = await item.OpenStreamForReadAsync())
                    {
                        retorno.Add(XElement.Load(stream));
                    }
                }
            }
            return retorno;
        }
    }
}
