using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class Configuracoes : Page, IHambuguer
    {
        public Configuracoes()
        {
            InitializeComponent();
        }

        public ListView ConteudoMenu
        {
            get
            {
                var lista = new ListView();
                lista.ItemsSource = new ObservableCollection<Controles.ItemHambuguer>
                {
                    new Controles.ItemHambuguer
                    {
                        Icone = new SymbolIcon(Symbol.Permissions),
                        Descricao = "Certificação"
                    },
                    new Controles.ItemHambuguer
                    {
                        Icone = new SymbolIcon(Symbol.Import),
                        Descricao = "Importação"
                    }
                };
                lista.SelectedIndex = 0;
                main.SelectionChanged += (sender, e) => lista.SelectedIndex = main.SelectedIndex;
                lista.SelectionChanged += (sender, e) => main.SelectedIndex = lista.SelectedIndex;
                return lista;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            MainPage.Current.SeAtualizar(Symbol.Setting, "Configurações");
        }
    }
}
