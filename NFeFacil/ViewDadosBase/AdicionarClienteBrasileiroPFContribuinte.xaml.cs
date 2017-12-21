using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewDadosBase
{
    [DetalhePagina(Symbol.People, "Cliente")]
    public sealed partial class AdicionarClienteBrasileiroPFContribuinte : Page
    {
        BaseAdicaoDestinatario Base { get; set; }

        public AdicionarClienteBrasileiroPFContribuinte()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Base = new BaseAdicaoDestinatario(e);
            Base.Cliente.IndicadorIE = 1;
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
