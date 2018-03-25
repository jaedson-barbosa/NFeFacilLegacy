using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace RegistroComum
{
    public sealed partial class MotivoCancelamento : ContentDialog
    {
        public string Motivo { get; private set; }

        public MotivoCancelamento()
        {
            InitializeComponent();
        }
    }
}
