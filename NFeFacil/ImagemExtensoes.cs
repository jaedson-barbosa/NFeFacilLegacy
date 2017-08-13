using NFeFacil.ItensBD;
using System;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace NFeFacil
{
    static class ImagemExtensoes
    {
        public static async Task FromStorageFileAsync(this Imagem imagem, StorageFile arquivo)
        {
            var buffer = await FileIO.ReadBufferAsync(arquivo);
            imagem.Bytes = buffer.ToArray();
        }

        public static async Task<ImageSource> GetSourceAsync(this Imagem imagem)
        {
            var retorno = new BitmapImage();
            using (var stream = new InMemoryRandomAccessStream())
            {
                await stream.WriteAsync(imagem.Bytes.AsBuffer());
                stream.Seek(0);
                retorno.SetSource(stream);
                return retorno;
            }
        }
    }
}
