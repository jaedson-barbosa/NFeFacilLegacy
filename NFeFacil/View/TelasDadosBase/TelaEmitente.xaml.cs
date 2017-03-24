using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace NFeFacil.View.TelasDadosBase
{
    public sealed partial class TelaEmitente : UserControl
    {
        public TelaEmitente()
        {
            InitializeComponent();
        }

        /* Como este é um recurso opcional
         * Ele só será implementado em futuros updates
        private async void btnLocalizar_Click(object sender, RoutedEventArgs e)
        {
            BitmapImage imagem = new BitmapImage();
            var arquivo = await new Importação(".jpg", ".png").ImportarArquivo();
            imagem.SetSource(await arquivo.OpenAsync(Windows.Storage.FileAccessMode.Read));
            imgLogotipo.Source = imagem;
        }

        private void btnApagar_Click(object sender, RoutedEventArgs e)
        {
            imgLogotipo.ClearValue(Image.SourceProperty);
        }*/
    }
}
