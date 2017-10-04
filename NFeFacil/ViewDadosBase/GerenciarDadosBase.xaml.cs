using System;
using System.Collections;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewDadosBase
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
            MainPage.Current.SeAtualizar(Symbol.Manage, "Dados base");
        }

        public IEnumerable ConteudoMenu
        {
            get
            {
                var retorno = new ObservableCollection<View.Controles.ItemHambuguer>
                {
                    new View.Controles.ItemHambuguer(Symbol.People, "Clientes"),
                    new View.Controles.ItemHambuguer(Symbol.People, "Motoristas"),
                    new View.Controles.ItemHambuguer(Symbol.Shop, "Produtos"),
                    new View.Controles.ItemHambuguer(Symbol.People, "Vendedores")
                };
                flipView.SelectionChanged += (sender, e) => MainMudou?.Invoke(this, new NewIndexEventArgs { NewIndex = flipView.SelectedIndex });
                return retorno;
            }
        }

        public event EventHandler MainMudou;
        public void AtualizarMain(int index) => flipView.SelectedIndex = index;
    }
}
