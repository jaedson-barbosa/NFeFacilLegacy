using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace BaseGeral.Controles
{
    public sealed partial class ExibicaoImagem : UserControl
    {
        bool[] OK = new bool[3];

        ImageSource imagem;
        public ImageSource Imagem
        {
            get => imagem;
            set
            {
                imagem = value;
                OK[0] = true;
                Analisar();
            }
        }

        string principal;
        public string Principal
        {
            get => principal;
            set
            {
                principal = value;
                OK[1] = true;
                Analisar();
            }
        }

        string secundario;
        public string Secundario
        {
            get => secundario;
            set
            {
                secundario = value;
                OK[2] = true;
                Analisar();
            }
        }

        void Analisar()
        {
            if (OK[0] && OK[1] && OK[2])
            {
                InitializeComponent();
            }
        }
    }
}
