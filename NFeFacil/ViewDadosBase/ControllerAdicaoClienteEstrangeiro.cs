using BaseGeral.ItensBD;
using Windows.UI.Xaml;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewDadosBase
{
    public sealed class ControllerAdicaoClienteEstrangeiro : ControllerAdicaoClienteGeral
    {
        public override string Documento
        {
            get => Cliente.IdEstrangeiro;
            set => Cliente.IdEstrangeiro = value;
        }

        public ControllerAdicaoClienteEstrangeiro(ClienteDI cliente = null) : base(cliente, 9,
            visIndicadorIE: Visibility.Collapsed,
            visIE: Visibility.Collapsed,
            visISUF: Visibility.Collapsed,
            visEndNacional: Visibility.Collapsed,
            visEndEstrageiro: Visibility.Visible) { }
    }
}