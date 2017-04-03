using NFeFacil.Log;
using NFeFacil.Validacao;
using NFeFacil.ViewModel.NotaFiscal;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;
using NFeFacil.ItensBD;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class AdicionarMotorista : Page, IEsconde
    {
        private MotoristaDI motorista;
        private TipoOperacao tipoRequisitado;
        private ILog Log = new Popup();

        public AdicionarMotorista()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var parametro = (GrupoViewBanco<MotoristaDI>)e.Parameter;
            motorista = parametro.ItemBanco ?? new MotoristaDI();
            tipoRequisitado = parametro.OperacaoRequirida;
            switch (tipoRequisitado)
            {
                case TipoOperacao.Adicao:
                    Propriedades.Intercambio.SeAtualizar(Telas.GerenciarDadosBase, Symbol.Add, "Adicionar motorista");
                    break;
                case TipoOperacao.Edicao:
                    Propriedades.Intercambio.SeAtualizar(Telas.GerenciarDadosBase, Symbol.Edit, "Editar motorista");
                    break;
                default:
                    break;
            }
            DataContext = new MotoristaDataContext(ref motorista);
        }

        private void Confirmar_Click(object sender, RoutedEventArgs e)
        {
            if (new ValidadorMotorista(motorista).Validar(Log))
            {
                using (var db = new AplicativoContext())
                {
                    if (tipoRequisitado == TipoOperacao.Adicao)
                    {
                        db.Add(motorista);
                        Log.Escrever(TitulosComuns.Sucesso, "Motorista salvo com sucesso.");
                    }
                    else
                    {
                        db.Update(motorista);
                        Log.Escrever(TitulosComuns.Sucesso, "Motorista alterado com sucesso.");
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
