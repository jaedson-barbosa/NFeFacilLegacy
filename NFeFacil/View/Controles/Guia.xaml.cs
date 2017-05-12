using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace NFeFacil.View.Controles
{
    [ContentProperty(Name = "Icone")]
    public sealed partial class Guia : UserControl
    {
        public IconElement Icone { get; set; }
        public string Label { get; set; }

        public Guia()
        {
            InitializeComponent();
        }
    }
}
