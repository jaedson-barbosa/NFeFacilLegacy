using NFeFacil.ItensBD;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class Inutilizacoes : Page
    {
        ObservableCollection<Inutilizacao> Itens { get; }

        public Inutilizacoes()
        {
            this.InitializeComponent();
            using (var repo = new Repositorio.Leitura())
            {
                Itens = repo.ObterInutilizacoes().GerarObs();
            }
        }

        private void AdicionarInutilizacao(object sender, RoutedEventArgs e)
        {

        }
    }
}
