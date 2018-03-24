using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace NFeFacil.Controles
{
    [ContentProperty(Name = "Icone")]
    public sealed partial class ItemPadraoSimples : UserControl
    {
        public ItemPadraoSimples()
        {
            InitializeComponent();
        }

        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public IconElement Icone { get; set; }
    }
}
