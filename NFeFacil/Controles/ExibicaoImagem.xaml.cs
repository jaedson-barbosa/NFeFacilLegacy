using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace NFeFacil.Controles
{
    public sealed partial class ExibicaoImagem : UserControl
    {
        public ImageSource Imagem
        {
            get => (ImageSource)GetValue(ImagemProperty);
            set => SetValue(ImagemProperty, value);
        }

        public string Principal
        {
            get => (string)GetValue(PrincipalProperty);
            set => SetValue(PrincipalProperty, value);
        }

        public string Secundario
        {
            get => (string)GetValue(SecundarioProperty);
            set => SetValue(SecundarioProperty, value);
        }

        public static readonly DependencyProperty ImagemProperty =
            DependencyProperty.Register("Imagem", typeof(ImageSource), typeof(ExibicaoImagem),
                new PropertyMetadata(null, new PropertyChangedCallback(ImagemChanged)));
        public static readonly DependencyProperty PrincipalProperty =
            DependencyProperty.Register("Principal", typeof(string), typeof(ExibicaoImagem),
                new PropertyMetadata(null, new PropertyChangedCallback(PrincipalChanged)));
        public static readonly DependencyProperty SecundarioProperty =
            DependencyProperty.Register("Secundario", typeof(string), typeof(ExibicaoImagem),
                new PropertyMetadata(null, new PropertyChangedCallback(SecundarioChanged)));

        public ExibicaoImagem()
        {
            InitializeComponent();
        }

        static void ImagemChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var input = (ExibicaoImagem)sender;
            input.imagem.Source = (ImageSource)e.NewValue;
        }

        static void PrincipalChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var input = (ExibicaoImagem)sender;
            input.principal.Text = (string)e.NewValue;
        }

        static void SecundarioChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var input = (ExibicaoImagem)sender;
            input.secundario.Text = (string)e.NewValue;
        }
    }
}
