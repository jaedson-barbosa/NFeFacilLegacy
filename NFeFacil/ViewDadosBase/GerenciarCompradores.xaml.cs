using NFeFacil.ItensBD;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewDadosBase
{
    [View.DetalhePagina(Symbol.Manage, "Gerenciar compradores")]
    public sealed partial class GerenciarCompradores : Page
    {
        ExibicaoComprador[] TodosCompradores { get; }
        ObservableCollection<ExibicaoComprador> Compradores { get; }

        public GerenciarCompradores()
        {
            InitializeComponent();
            using (var repo = new Repositorio.Leitura())
            {
                TodosCompradores = repo.ObterCompradores().Select(x => new ExibicaoComprador
                {
                    Root = x.Item2,
                    NomeEmpresa = x.Item1
                }).ToArray();
                Compradores = TodosCompradores.GerarObs();
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

        private void Buscar(object sender, TextChangedEventArgs e)
        {
            var busca = ((TextBox)sender).Text;
            for (int i = 0; i < TodosCompradores.Length; i++)
            {
                var atual = TodosCompradores[i];
                bool valido = (DefinicoesPermanentes.ModoBuscaComprador == 0
                    ? atual.Root.Nome : atual.NomeEmpresa).ToUpper().Contains(busca.ToUpper());
                if (valido && !Compradores.Contains(atual))
                {
                    Compradores.Add(atual);
                }
                else if (!valido && Compradores.Contains(atual))
                {
                    Compradores.Remove(atual);
                }
            }
        }
    }

    struct ExibicaoComprador
    {
        public Comprador Root { get; set; }
        public string NomeEmpresa { get; set; }
    }
}
