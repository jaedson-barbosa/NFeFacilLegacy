using BaseGeral;
using BaseGeral.ItensBD;
using BaseGeral.View;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewDadosBase
{
    [DetalhePagina(Symbol.Manage, "Gerenciar motoristas")]
    public sealed partial class GerenciarMotoristas : Page
    {
        MotoristaDI[] TodosMotoristas { get; }
        ObservableCollection<MotoristaDI> Motoristas { get; }

        public GerenciarMotoristas()
        {
            InitializeComponent();
            using (var repo = new BaseGeral.Repositorio.Leitura())
            {
                TodosMotoristas = repo.ObterMotoristas().ToArray();
                Motoristas = TodosMotoristas.GerarObs();
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

            using (var repo = new BaseGeral.Repositorio.Escrita())
            {
                repo.InativarDadoBase(mot, DefinicoesTemporarias.DateTimeNow);
                Motoristas.Remove(mot);
            }
        }

        private void Buscar(object sender, TextChangedEventArgs e)
        {
            var busca = ((TextBox)sender).Text;
            for (int i = 0; i < TodosMotoristas.Length; i++)
            {
                var atual = TodosMotoristas[i];
                bool valido = DefinicoesPermanentes.ModoBuscaMotorista == 0
                    ? atual.Nome.ToUpper().Contains(busca.ToUpper())
                    : atual.Documento.Contains(busca);
                if (valido && !Motoristas.Contains(atual))
                {
                    Motoristas.Add(atual);
                }
                else if (!valido && Motoristas.Contains(atual))
                {
                    Motoristas.Remove(atual);
                }
            }
        }
    }
}
