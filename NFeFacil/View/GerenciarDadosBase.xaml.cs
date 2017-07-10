using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class GerenciarDadosBase : Page, IHambuguer
    {
        public GerenciarDadosBase()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            MainPage.Current.SeAtualizar(Symbol.Manage, "Dados base");
        }

        public ListView ConteudoMenu
        {
            get
            {
                var lista = new ListView()
                {
                    ItemsSource = new ObservableCollection<Controles.ItemHambuguer>
                    {
                        new Controles.ItemHambuguer(Symbol.People, "Emitentes"),
                        new Controles.ItemHambuguer(Symbol.People, "Clientes"),
                        new Controles.ItemHambuguer(Symbol.People, "Motoristas"),
                        new Controles.ItemHambuguer(Symbol.Shop, "Produtos"),
                        new Controles.ItemHambuguer(Symbol.People, "Vendedores")
                    },
                    SelectedIndex = 0
                };
                flipView.SelectionChanged += (sender, e) => lista.SelectedIndex = flipView.SelectedIndex;
                lista.SelectionChanged += (sender, e) => flipView.SelectedIndex = lista.SelectedIndex;
                return lista;
            }
        }
    }
}
