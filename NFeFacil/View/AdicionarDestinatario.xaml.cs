using System.Threading.Tasks;
using NFeFacil.ItensBD;
using NFeFacil.Log;
using NFeFacil.Validacao;
using NFeFacil.ViewModel.NotaFiscal;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class AdicionarDestinatario : Page, IEsconde
    {
        private ClienteDI cliente;
        private TipoOperacao tipoRequisitado;
        private ILog Log = new Popup();

        public AdicionarDestinatario()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var parametro = (GrupoViewBanco<ClienteDI>)e.Parameter;
            cliente = parametro.ItemBanco ?? new ClienteDI();
            tipoRequisitado = parametro.OperacaoRequirida;
            DataContext = new ClienteDataContext(ref cliente);
        }

        private void Confirmar_Click(object sender, RoutedEventArgs e)
        {
            if (new ValidadorDestinatario(cliente).Validar(Log))
            {
                using (var db = new AplicativoContext())
                {
                    if (tipoRequisitado == TipoOperacao.Adicao)
                    {
                        db.Add(cliente);
                        Log.Escrever(TitulosComuns.Sucesso, "Cliente salvo com sucesso.");
                    }
                    else
                    {
                        db.Update(cliente);
                        Log.Escrever(TitulosComuns.Sucesso, "Cliente alterado com sucesso.");
                    }
                    db.SaveChanges();
                }
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
