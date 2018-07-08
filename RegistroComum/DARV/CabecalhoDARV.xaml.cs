using BaseGeral.ItensBD;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// O modelo de item de Controle de Usuário está documentado em https://go.microsoft.com/fwlink/?LinkId=234236

namespace RegistroComum.DARV
{
    public sealed partial class CabecalhoDARV : UserControl
    {
        RegistroVenda Registro => Main.Registro;
        EmitenteDI Emitente => Main.Emitente;
        ClienteDI Cliente => Main.Cliente;
        string Vendedor => Main.Vendedor;
        Comprador Comprador => Main.Comprador;
        MotoristaDI Motorista => Main.Motorista;

        string Id => Registro.Id.ToString().ToUpper();
        string EnderecoCliente => Main.EnderecoCliente;

        Visibility VisibilidadeTransporte => Main.VisibilidadeTransporte;
        Visibility VisibilidadeNFeRelacionada => Main.VisibilidadeNFeRelacionada;
        Visibility VisibilidadeComprador => Main.VisibilidadeComprador;

        public ViewDARV Main { get; set; }

        public CabecalhoDARV()
        {
            InitializeComponent();
        }
    }
}
