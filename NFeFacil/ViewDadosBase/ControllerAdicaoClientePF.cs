using BaseGeral.ItensBD;
using Windows.UI.Xaml;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewDadosBase
{
    public sealed class ControllerAdicaoClientePF : ControllerAdicaoClienteGeral
    {
        public override string Documento
        {
            get => Cliente.CPF;
            set => Cliente.CPF = value;
        }

        public ControllerAdicaoClientePF(ClienteDI cliente = null) : base(cliente, 9,
            visIndicadorIE: Visibility.Collapsed,
            visIE: Visibility.Collapsed,
            visISUF: Visibility.Collapsed,
            visEndNacional: Visibility.Visible,
            visEndEstrageiro: Visibility.Collapsed) { }
    }
}