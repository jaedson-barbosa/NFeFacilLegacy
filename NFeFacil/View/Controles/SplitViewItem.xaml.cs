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
        public string TextoDoBotão
        {
            get
            {
                return txtTexto.Text;
            }
            set
            {
                txtTexto.Text = value;
            }
        }
        public Symbol Ícone
        {
            get
            {
                return íconeBotão.Symbol;
            }
            set
            {
                íconeBotão.Symbol = value;
            }
        }
        public static readonly DependencyProperty TextoDoBotãoProperty =
            DependencyProperty.Register(nameof(TextoDoBotão), typeof(string), typeof(SplitViewItem), null);
        public static readonly DependencyProperty ÍconeProperty =
            DependencyProperty.Register(nameof(Ícone), typeof(Symbol), typeof(SplitViewItem), null);
    }
}
