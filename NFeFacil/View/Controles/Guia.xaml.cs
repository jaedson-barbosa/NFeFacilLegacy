using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace NFeFacil.View.Controles
{
    public sealed partial class Guia : UserControl
    {
        public static readonly DependencyProperty SymbolProperty = DependencyProperty.Register(nameof(Symbol), typeof(Symbol), typeof(Guia), null);
        public Symbol Symbol
        {
            get { return (Symbol)GetValue(SymbolProperty); }
            set { SetValue(SymbolProperty, value); }
        }

        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register(nameof(Label), typeof(string), typeof(Guia), null);
        public string Label
        {
            get { return GetValue(LabelProperty) as string; }
            set { SetValue(LabelProperty, value); }
        }

        public Guia()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}
