using NFeFacil.ItensBD;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewDadosBase
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class GerenciarCompradores : Page
    {
        ObservableCollection<ExibicaoComprador> Compradores { get; }

        public GerenciarCompradores()
        {
            this.InitializeComponent();
            using (var db = new AplicativoContext())
            {
                var original = db.Compradores.Where(x => x.Ativo).OrderBy(x => x.Nome).ToArray();
                Compradores = new ObservableCollection<ExibicaoComprador>();
                for (int i = 0; i < original.Length; i++)
                {
                    Compradores.Add(new ExibicaoComprador()
                    {
                        Root = original[i],
                        NomeEmpresa = db.Clientes.Find(original[i].IdEmpresa).Nome
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

            using (var db = new AplicativoContext())
            {
                compr.Root.Ativo = false;
                db.Update(compr.Root);
                db.SaveChanges();
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
