using NFeFacil.Log;
using NFeFacil.Validacao;
using NFeFacil.ViewModel.NotaFiscal;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class AdicionarMotorista : Page, IEsconde
    {
        private MotoristaDataContext Transp => DataContext as MotoristaDataContext;
        private ILog Log = new Popup();

        public AdicionarMotorista()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            DataContext = (MotoristaDataContext)e.Parameter;
        }

        private void Confirmar_Click(object sender, RoutedEventArgs e)
        {
            if (new ValidadorMotorista(Transp.Motorista).Validar(Log))
            {
                using (var db = new AplicativoContext())
                {
                    db.Add(new ItensBD.MotoristaDI(Transp.Motorista));
                    db.SaveChanges();
                }
                Log.Escrever(TitulosComuns.Sucesso, "Motorista salvo com sucesso.");
                Propriedades.Intercambio.Retornar();
            }
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            Propriedades.Intercambio.Retornar();
        }

        public async Task EsconderAsync()
        {
            ocultarGrid.Begin();
            await Task.Delay(250);
        }
    }
}
