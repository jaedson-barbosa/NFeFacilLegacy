using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace NFeFacil.View.Controles
{
    [Windows.UI.Xaml.Markup.ContentProperty(Name = "Icone")]
    public partial class SplitViewItem : UserControl
    {
        public SplitViewItem()
        {
            InitializeComponent();
        }

        public string Texto { get; set; }
        public IconElement Icone { get; set; }
    }
}
