using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage;

namespace BibliotecaCentral
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
            }
        }

        public async Task Remover(string nome)
        {
            try
            {
                var arq = await PastaArquivos.GetFileAsync(nome + ".xml");
                await arq.DeleteAsync(StorageDeleteOption.PermanentDelete);
            }
            catch (FileNotFoundException)
            {
                new Log.Popup().Escrever(Log.TitulosComuns.ErroSimples, $"O arquivo {nome} não foi encontrado.");
            }
            catch (Exception) { throw; }
        }

        internal async Task<XElement> Retornar(string nome)
        {
            var arq = await PastaArquivos.GetFileAsync(nome + ".xml");
            using (var stream = await arq.OpenStreamForReadAsync())
            {
                return XElement.Load(stream);
            }
        }

        public async Task<T> Retornar<T>(string nome) where T : class
        {
            var arq = await PastaArquivos.GetFileAsync(nome + ".xml");
            using (var stream = await arq.OpenStreamForReadAsync())
            {
                return stream.FromXElement<T>();
            }
        }

        public async Task<List<(string nome, XElement xml)>> Registro()
        {
            var arqs = await PastaArquivos.GetFilesAsync();
            var quantidade = arqs.Count(x => x.FileType == ".xml");
            var retorno = new List<(string nome, XElement xml)>();
            for (int i = 0; i < arqs.Count(); i++)
            {
                if (arqs[i].FileType == ".xml")
                {
                    using (var stream = await arqs[i].OpenStreamForReadAsync())
                    {
                        retorno.Add((arqs[i].Name, XElement.Load(stream)));
                    }
                }
            }
            return retorno;
        }
    }
}
