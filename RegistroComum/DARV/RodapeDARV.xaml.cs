using BaseGeral.ItensBD;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// O modelo de item de Controle de Usuário está documentado em https://go.microsoft.com/fwlink/?LinkId=234236

namespace RegistroComum.DARV
{
    public sealed partial class RodapeDARV : UserControl
    {
        RegistroVenda Registro => Main.Registro;
        ClienteDI Cliente => Main.Cliente;
        Comprador Comprador => Main.Comprador;

        string Subtotal => Main.Subtotal;
        string Acrescimos => Main.Acrescimos;
        string Desconto => Main.Desconto;
        string Total => Main.Total;

        string NomeAssinatura => Comprador?.Nome ?? Cliente.Nome;
        string Observacoes => Registro.Observações;

        Visibility VisibilidadePagamento => Main.VisibilidadePagamento;
        Visibility VisibilidadeObservacoes => Main.VisibilidadeObservacoes;

        public ViewDARV Main { get; set; }

        public RodapeDARV()
        {
            InitializeComponent();
        }
    }
}
