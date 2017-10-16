using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace NFeFacil.View.Controles
{
    [ContentProperty(Name = "Icone")]
    public sealed partial class ItemPadrao : UserControl
    {
        public ItemPadrao()
        {
            InitializeComponent();
        }

        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public IconElement Icone { get; set; }
    }
}
