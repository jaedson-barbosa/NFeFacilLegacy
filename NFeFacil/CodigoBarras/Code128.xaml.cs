using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace NFeFacil.CodigoBarras
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
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register(nameof(Data), typeof(string), typeof(Code128),
                new PropertyMetadata(string.Empty, new PropertyChangedCallback(BarcodeChanged)));

        public Code128Types Type
        {
            get => (Code128Types)GetValue(TypeProperty);
            set => SetValue(TypeProperty, value);
        }
        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register(nameof(Type), typeof(Code128Types), typeof(Code128),
                new PropertyMetadata(Code128Types.C, new PropertyChangedCallback(BarcodeChanged)));

        static void BarcodeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var code = (Code128)sender;
            if (!string.IsNullOrEmpty(code.Data))
            {
                var barcode = new Barcode(code.Data, code.Type);
                barcode.Preencode();
                var width = Math.Floor(code.ActualWidth / barcode.EncodedValue.Length);
                if (width == 0) width = 1;
                var height = double.IsNaN(code.Height) ? 30 : code.Height;
                code.Barras = barcode.Encode((int)width, height);
            }
        }

        public Code128()
        {
            InitializeComponent();
        }

        private void TamanhoMudou(object sender, SizeChangedEventArgs e)
        {
            BarcodeChanged(this, null);
        }
    }
}
