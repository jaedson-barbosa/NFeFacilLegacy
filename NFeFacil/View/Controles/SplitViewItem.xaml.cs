using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace NFeFacil.View.Controles
{
    public partial class SplitViewItem : UserControl
    {
        public SplitViewItem()
        {
            InitializeComponent();
        }
        public string TextoDoBotao { get; set; }
        public Symbol Icone { get; set; }
        public static readonly DependencyProperty TextoDoBotãoProperty =
            DependencyProperty.Register(nameof(TextoDoBotao), typeof(string), typeof(SplitViewItem), null);
        public static readonly DependencyProperty ÍconeProperty =
            DependencyProperty.Register(nameof(Icone), typeof(Symbol), typeof(SplitViewItem), null);
    }
}
