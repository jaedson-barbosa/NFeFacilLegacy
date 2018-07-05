using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace BaseGeral.Controles
{
    [ContentProperty(Name = "Icone")]
    public sealed partial class ItemPadrao : UserControl
    {
        public event RoutedEventHandler Click;

        public ItemPadrao()
        {
            InitializeComponent();
        }

        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public IconElement Icone { get; set; }

        private void Clicado(object sender, RoutedEventArgs e)
        {
            Click?.Invoke(this, e);

        }
    }
}
