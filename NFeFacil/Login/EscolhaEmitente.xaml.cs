using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.Login
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class EscolhaEmitente : Page
    {
        public EscolhaEmitente()
        {
            this.InitializeComponent();
            using (var db = new AplicativoContext())
            {
                var emitentes = db.Emitentes.ToArray();
                var imagens = db.Imagens;
                var quantEmitentes = emitentes.Length;
                var conjuntos = new ObservableCollection<Conjunto>();
                for (int i = 0; i < quantEmitentes; i++)
                {
                    var atual = emitentes[i];
                    var novoConjunto = new Conjunto
                    {
                        Nome = atual.Nome
                    };
                    var img = imagens.Find(atual.Id);
                    if (img != null)
                    {
                        var task = img.GetSourceAsync();
                        task.Wait();
                        novoConjunto.Imagem = task.Result;
                    }
                    conjuntos.Add(novoConjunto);
                }
                grdEmitentes.ItemsSource = conjuntos;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            MainPage.Current.SeAtualizar(Symbol.Home, "Entrar no sistema");
        }

        struct Conjunto
        {
            public ImageSource Imagem { get; set; }
            public string Nome { get; set; }
        }
    }
}
