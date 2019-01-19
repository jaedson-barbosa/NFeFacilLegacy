using BaseGeral.ItensBD;
using Windows.UI.Xaml;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewDadosBase
{
    public sealed class ControllerAdicaoClientePJ : ControllerAdicaoClienteGeral
    {
        public override string Documento
        {
            get => Cliente.CNPJ;
            set => Cliente.CNPJ = value;
        }

        public override int IndicadorIESelecionado
        {
            get
            {
                if (Cliente.IndicadorIE == 9)
                {
                    if (string.IsNullOrEmpty(Cliente.InscricaoEstadual))
                    {
                        EnabledIE = false;
                        return 2;
                    }
                    else return 3;
                }
                else if (Cliente.IndicadorIE == 2)
                {
                    EnabledIE = false;
                    return 1;
                }
                else return 0;
            }
            set
            {
                if (value == 0)
                {
                    EnabledIE = true;
                    Cliente.IndicadorIE = 1;
                }
                else if (value == 1)
                {
                    EnabledIE = false;
                    IE = null;
                    Cliente.IndicadorIE = 2;
                }
                else if (value == 2)
                {
                    EnabledIE = false;
                    IE = null;
                    Cliente.IndicadorIE = 9;
                }
                else
                {
                    EnabledIE = true;
                    Cliente.IndicadorIE = 9;
                }
            }
        }

        public ControllerAdicaoClientePJ(ClienteDI cliente = null) : base(cliente, 0,
            visIndicadorIE: Visibility.Visible,
            visIE: Visibility.Visible,
            visISUF: Visibility.Visible,
            visEndNacional: Visibility.Visible,
            visEndEstrageiro: Visibility.Collapsed) { }
    }
}