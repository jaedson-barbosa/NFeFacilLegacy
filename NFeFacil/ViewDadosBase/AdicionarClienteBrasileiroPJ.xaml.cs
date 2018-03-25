using BaseGeral.View;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewDadosBase
{
    [DetalhePagina(Symbol.People, "Cliente")]
    public sealed partial class AdicionarClienteBrasileiroPJ : Page
    {
        BaseAdicaoDestinatario Base { get; set; }

        public string IndicadorIESelecionado
        {
            get
            {
                var ind = Base.Cliente.IndicadorIE;
                if (ind == 9)
                {
                    if (string.IsNullOrEmpty(Base.Cliente.InscricaoEstadual))
                    {
                        txtIE.IsEnabled = false;
                        return "2";
                    }
                    else
                    {
                        return "3";
                    }
                }
                else if (ind == 2)
                {
                    txtIE.IsEnabled = false;
                    return "1";
                }
                else
                {
                    return "0";
                }
            }
            set
            {
                if (value == "0")
                {
                    txtIE.IsEnabled = true;
                    Base.Cliente.IndicadorIE = 1;
                }
                else if (value == "1")
                {
                    txtIE.IsEnabled = false;
                    Base.Cliente.InscricaoEstadual = null;
                    Base.Cliente.IndicadorIE = 2;
                }
                else if (value == "2")
                {
                    txtIE.IsEnabled = false;
                    Base.Cliente.InscricaoEstadual = null;
                    Base.Cliente.IndicadorIE = 9;
                }
                else
                {
                    txtIE.IsEnabled = true;
                    Base.Cliente.IndicadorIE = 9;
                }
                txtIE.Text = string.Empty;
            }
        }

        public AdicionarClienteBrasileiroPJ()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Base = new BaseAdicaoDestinatario(e);
        }

        void Confirmar_Click(object sender, RoutedEventArgs e)
        {
            Base.Confirmar();
        }

        void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            Base.Cancelar();
        }
    }
}
