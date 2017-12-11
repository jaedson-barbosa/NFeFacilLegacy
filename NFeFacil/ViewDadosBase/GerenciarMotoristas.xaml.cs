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
    public sealed partial class GerenciarMotoristas : Page
    {
        ObservableCollection<MotoristaDI> Motoristas { get; }

        public GerenciarMotoristas()
        {
            InitializeComponent();
            using (var db = new AplicativoContext())
            {
                Motoristas = db.Motoristas.Where(x => x.Ativo).OrderBy(x => x.Nome).GerarObs();
            }
        }

        private void AdicionarMotorista(object sender, RoutedEventArgs e)
        {
            MainPage.Current.Navegar<AdicionarMotorista>();
        }

        private void EditarMotorista(object sender, RoutedEventArgs e)
        {
            var contexto = ((FrameworkElement)sender).DataContext;
            MainPage.Current.Navegar<AdicionarMotorista>((MotoristaDI)contexto);
        }

        private void InativarMotorista(object sender, RoutedEventArgs e)
        {
            var contexto = ((FrameworkElement)sender).DataContext;
            var mot = (MotoristaDI)contexto;

            using (var db = new AplicativoContext())
            {
                mot.Ativo = false;
                db.Update(mot);
                db.SaveChanges();
                Motoristas.Remove(mot);
            }
        }
    }
}
