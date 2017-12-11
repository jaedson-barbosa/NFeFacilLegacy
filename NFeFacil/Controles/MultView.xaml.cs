using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace NFeFacil.Controles
{
    [ContentProperty(Name = "Children")]
    public sealed partial class MultView : UserControl
    {
        public MultView()
        {
            InitializeComponent();
            Children = grdPrincipal.Children;
        }

        public static readonly DependencyProperty ChildrenProperty = DependencyProperty.Register(
           nameof(Children), 
           typeof(UIElementCollection),
           typeof(MultView),
           new PropertyMetadata(null));

        public UIElementCollection Children
        {
            get => (UIElementCollection)GetValue(ChildrenProperty);
            private set => SetValue(ChildrenProperty, value);
        }

        public int SelectedIndex
        {
            get
            {
                for (int i = 0; i < grdPrincipal.Children.Count; i++)
                {
                    if (grdPrincipal.Children[i].Visibility == Visibility.Visible) return i;
                }
                return -1;
            }
            set
            {
                if (grdPrincipal.Children.Count > value && value >= 0)
                {
                    var index = SelectedIndex;
                    if (index != -1)
                    {
                        grdPrincipal.Children[index].Visibility = Visibility.Collapsed;
                    }
                    grdPrincipal.Children[value].Visibility = Visibility.Visible;
                }
            }
        }

        private void grdPrincipal_Loaded(object sender, RoutedEventArgs e)
        {
            var quant = grdPrincipal.Children.Count;
            for (int i = 0; i < quant; i++)
            {
                var filho = (FrameworkElement)grdPrincipal.Children[i];
                filho.Visibility = Visibility.Collapsed;
            }
            SelectedIndex = 0;
        }
    }
}
