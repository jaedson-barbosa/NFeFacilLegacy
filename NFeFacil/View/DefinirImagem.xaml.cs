using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View
{
    public sealed partial class DefinirImagem : ContentDialog
    {
        Guid Id { get; }
        byte[] bytes;
        internal ImageSource Imagem { get; private set; }

        public DefinirImagem(Guid id, ImageSource imagem)
        {
            this.InitializeComponent();
            Id = id;
            imgAtual.Source = imagem;
        }

        void Concluir(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (bytes == null && imgAtual.Source != null)
            {
                return;
            }
            using (var db = new AplicativoContext())
            {
                var img = db.Imagens.Find(Id);
                if (img != null)
                {
                    img.UltimaData = DateTimeNow;
                    img.Bytes = bytes;
                    db.Imagens.Update(img);
                }
                else
                {
                    img = new ItensBD.Imagem()
                    {
                        Id = Id,
                        UltimaData = DateTimeNow,
                        Bytes = bytes
                    };
                    db.Imagens.Add(img);
                }
                db.SaveChanges();
            }
            Imagem = imgAtual.Source;
        }

        async void Buscar(object sender, RoutedEventArgs e)
        {
            var open = new Windows.Storage.Pickers.FileOpenPicker();
            open.FileTypeFilter.Add(".jpg");
            open.FileTypeFilter.Add(".jpeg");
            open.FileTypeFilter.Add(".png");
            var arq = await open.PickSingleFileAsync();

            var buffer = await FileIO.ReadBufferAsync(arq);
            bytes = buffer.ToArray();

            var source = new BitmapImage();
            using (var stream = new InMemoryRandomAccessStream())
            {
                await stream.WriteAsync(buffer);
                stream.Seek(0);
                source.SetSource(stream);
                imgAtual.Source = source;
            }
        }

        void Apagar(object sender, RoutedEventArgs e)
        {
            bytes = null;
            imgAtual.Source = null;
        }
    }
}
