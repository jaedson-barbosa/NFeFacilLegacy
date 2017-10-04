using System.Collections;
using System.Collections.ObjectModel;
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

        public IEnumerable ConteudoMenu => new ObservableCollection<Controles.ItemHambuguer>
        {
            new Controles.ItemHambuguer(Symbol.Permissions, "Certificação"),
            new Controles.ItemHambuguer(Symbol.Import, "Importação")
        };

        public void AtualizarMain(int index) => main.SelectedIndex = index;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            MainPage.Current.SeAtualizar(Symbol.Setting, "Configurações");
        }

        private void TelaMudou(object sender, SelectionChangedEventArgs e)
        {
            var index = ((FlipView)sender).SelectedIndex;
            MainPage.Current.AlterarSelectedIndexHamburguer(index);
        }
    }
}
