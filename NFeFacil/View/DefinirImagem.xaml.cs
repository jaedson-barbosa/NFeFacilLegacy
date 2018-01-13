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
            InitializeComponent();
            Id = id;
            imgAtual.Source = imagem;
        }

        void Concluir(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (bytes == null && imgAtual.Source != null)
            {
                return;
            }
            using (var repo = new Repositorio.Escrita())
            {
                repo.SalvarImagem(Id, DefinicoesTemporarias.DateTimeNow, bytes);
            }
            Imagem = imgAtual.Source;
        }

        async void Buscar(object sender, RoutedEventArgs e)
        {
            try
            {
                var open = new Windows.Storage.Pickers.FileOpenPicker();
                open.FileTypeFilter.Add(".jpg");
                open.FileTypeFilter.Add(".jpeg");
                open.FileTypeFilter.Add(".png");
                var arq = await open.PickSingleFileAsync();
                if (arq != null)
                {
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
            }
            catch { }
        }

        void Apagar(object sender, RoutedEventArgs e)
        {
            bytes = null;
            imgAtual.Source = null;
        }
    }
}
