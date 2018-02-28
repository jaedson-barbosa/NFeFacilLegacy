using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

// O modelo de item de Controle de Usuário está documentado em https://go.microsoft.com/fwlink/?LinkId=234236

namespace NFeFacil.Controles
{
    [ContentProperty(Name = "Principal")]
    public sealed partial class DuplaView : UserControl
    {
        public DuplaView()
        {
            InitializeComponent();
        }

        public FrameworkElement Principal
        {
            get => scrPrincipal.Content as FrameworkElement;
            set => scrPrincipal.Content = value;
        }

        public FrameworkElement Menu
        {
            get => scrMenu.Child as FrameworkElement;
            set => scrMenu.Child = value;
        }
    }
}
