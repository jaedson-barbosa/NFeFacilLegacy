using BaseGeral;
using BaseGeral.ItensBD;
using BaseGeral.Log;
using BaseGeral.View;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.Login
{
    [DetalhePagina(Symbol.Home, "Escolher empresa")]
    public sealed partial class EscolhaEmitente : Page
    {
        public EscolhaEmitente()
        {
            try
            {
                InitializeComponent();
            }
            catch (System.Exception e)
            {
                if (DefinicoesPermanentes.UsarFluent)
                {
                    var log = Popup.Current;
                    log.Escrever(TitulosComuns.Erro, "Parece que este dispositivo não suporta o Fluent Design, ele será desativado e o aplicativo será reiniciado. Depois de atualizar o Windows você poderá reativa o Fluent Design.");
                }
                else e.ManipularErro();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Frame.BackStack.Clear();
            using (var repo = new BaseGeral.Repositorio.Leitura())
            {
                var conjuntos = new ObservableCollection<ConjuntoBasicoExibicao<EmitenteDI>>();
                foreach (var atual in repo.ObterEmitentes())
                {
                    var novoConjunto = new ConjuntoBasicoExibicao<EmitenteDI>
                    {
                        Objeto = atual.Item1,
                        Principal = atual.Item1.NomeFantasia,
                        Secundario = atual.Item1.Nome,
                        Imagem = atual.Item2?.GetSource()
                    };
                    conjuntos.Add(novoConjunto);
                }
                grdEmitentes.ItemsSource = conjuntos;
            }
        }

        private void EmitenteEscolhido(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var item = e.AddedItems[0];
                MainPage.Current.Navegar<GeralEmitente>(item);
            }
        }

        private void Cadastrar(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            MainPage.Current.Navegar<AdicionarEmitente>();
        }
    }
}
