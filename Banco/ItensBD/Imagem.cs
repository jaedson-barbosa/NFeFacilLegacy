using System;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Banco.ItensBD
{
    public sealed class Imagem
    {
        public Guid Id { get; set; }
        public byte[] Bytes { get; set; }

        public async Task FromStorageFileAsync(StorageFile arquivo)
        {
            var buffer = await FileIO.ReadBufferAsync(arquivo);
            Bytes = buffer.ToArray();
        }

        public async Task<ImageSource> GetSourceAsync()
        {
            var imagem = new BitmapImage();
            using (var stream = new InMemoryRandomAccessStream())
            {
                await stream.WriteAsync(Bytes.AsBuffer());
                stream.Seek(0);
                imagem.SetSource(stream);
                return imagem;
            }
        }
    }
}
