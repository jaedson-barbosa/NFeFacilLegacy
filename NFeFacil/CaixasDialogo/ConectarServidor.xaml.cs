using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.CaixasDialogo
{
    public sealed partial class ConectarServidor : ContentDialog
    {
        public string IP { get; set; }

        public ConectarServidor()
        {
            InitializeComponent();
        }
    }
}
