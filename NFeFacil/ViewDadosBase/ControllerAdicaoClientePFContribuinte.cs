using BaseGeral.ItensBD;
using Windows.UI.Xaml;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewDadosBase
{
    public sealed class ControllerAdicaoClientePFContribuinte : ControllerAdicaoClienteGeral
    {
        public override string Documento
        {
            get => Cliente.CPF;
            set => Cliente.CPF = value;
        }

        public ControllerAdicaoClientePFContribuinte(ClienteDI cliente = null) : base(cliente, 1,
            visIndicadorIE: Visibility.Collapsed,
            visIE: Visibility.Visible,
            visISUF: Visibility.Collapsed,
            visEndNacional: Visibility.Visible,
            visEndEstrageiro: Visibility.Collapsed) { }
    }
}