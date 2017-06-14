using CodigoBarras.Symbologies;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CodigoBarras.Controles
{
    public sealed partial class Code128 : UserControl
    {
        public Rectangle[] Barras
        {
            set
            {
                stkBarras.Children.Clear();
                for (int i = 0; i < value.Length; i++)
                {
                    stkBarras.Children.Add(value[i]);
                }
            }
        }

        public string Data
        {
            get => (string)GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }
        public static readonly DependencyProperty DataProperty = DependencyProperty.Register(nameof(Data), typeof(string), typeof(Code128), new PropertyMetadata(string.Empty, new PropertyChangedCallback(BarcodeChanged)));

        public Code128Types Type
        {
            get => (Code128Types)GetValue(TypeProperty);
            set => SetValue(TypeProperty, value);
        }
        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(nameof(Type), typeof(Code128Types), typeof(Code128), new PropertyMetadata(Code128Types.C, new PropertyChangedCallback(BarcodeChanged)));

        static void BarcodeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var code = (Code128)sender;
            if (!string.IsNullOrEmpty(code.Data))
            {
                code.Barras = new Barcode(code.Data).Encode(code.Type);
            }
        }

        public Code128()
        {
            InitializeComponent();
        }
    }
}
