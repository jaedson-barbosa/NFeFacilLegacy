using NFeFacil.ItensBD;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewDadosBase
{
    [View.DetalhePagina(Symbol.Manage, "Gerenciar compradores")]
    public sealed partial class GerenciarCompradores : Page
    {
        ObservableCollection<ExibicaoComprador> Compradores { get; }

        public GerenciarCompradores()
        {
            InitializeComponent();
            using (var repo = new Repositorio.Leitura())
            {
                Compradores = new ObservableCollection<ExibicaoComprador>();
                foreach (var atual in repo.ObterCompradores())
                {
                    Compradores.Add(new ExibicaoComprador()
                    {
                        Root = atual.Item2,
                        NomeEmpresa = atual.Item1
                    });
                }
            }
        }

        private void AdicionarComprador(object sender, RoutedEventArgs e)
        {
            MainPage.Current.Navegar<AdicionarComprador>();
        }

        private void EditarComprador(object sender, RoutedEventArgs e)
        {
            var contexto = ((FrameworkElement)sender).DataContext;
            MainPage.Current.Navegar<AdicionarComprador>(((ExibicaoComprador)contexto).Root);
        }

        private void InativarComprador(object sender, RoutedEventArgs e)
        {
            var contexto = ((FrameworkElement)sender).DataContext;
            var compr = (ExibicaoComprador)contexto;

            using (var repo = new Repositorio.Escrita())
            {
                repo.InativarDadoBase(compr.Root, DefinicoesTemporarias.DateTimeNow);
                Compradores.Remove(compr);
            }
        }
    }

    struct ExibicaoComprador
    {
        public Comprador Root { get; set; }
        public string NomeEmpresa { get; set; }
    }
}
