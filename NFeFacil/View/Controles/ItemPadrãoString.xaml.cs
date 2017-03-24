using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace NFeFacil.View.Controles
{
    public sealed partial class ItemPadrãoString : UserControl
    {
        public ItemPadrãoString()
        {
            InitializeComponent();
        }
        public string Símbolo
        {
            get
            {
                return Simbolo.Glyph;
            }
            set
            {
                Simbolo.Glyph = value;
            }
        }
        public string Título
        {
            get
            {
                return Titulo.Text;
            }
            set
            {
                Titulo.Text = value;
            }
        }
        public string Descrição
        {
            get
            {
                return Descricao.Text;
            }
            set
            {
                Descricao.Text = value;
            }
        }
        public static readonly DependencyProperty SímboloProperty =
            DependencyProperty.Register(nameof(Símbolo), typeof(Symbol), typeof(ItemPadrãoSymbol), new PropertyMetadata(null));
        public static readonly DependencyProperty TítuloProperty =
            DependencyProperty.Register(nameof(Título), typeof(string), typeof(ItemPadrãoSymbol), new PropertyMetadata(null));
        public static readonly DependencyProperty DescriçãoProperty =
            DependencyProperty.Register(nameof(Descrição), typeof(string), typeof(ItemPadrãoSymbol), new PropertyMetadata(null));
    }
}
