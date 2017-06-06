using BibliotecaCentral.Log;
using BibliotecaCentral.Validacao;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using BibliotecaCentral.Repositorio;
using BibliotecaCentral.ItensBD;
using NFeFacil.ViewModel;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class AdicionarMotorista : Page
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
            GrupoViewBanco<MotoristaDI> parametro;
            if (e.Parameter == null)
            {
                parametro = new GrupoViewBanco<MotoristaDI>
                {
                    ItemBanco = new MotoristaDI(),
                    OperacaoRequirida = TipoOperacao.Adicao
                };
            }
            else
            {
                parametro = (GrupoViewBanco<MotoristaDI>)e.Parameter;
            }
            motorista = parametro.ItemBanco;
            tipoRequisitado = parametro.OperacaoRequirida;
            switch (tipoRequisitado)
            {
                case TipoOperacao.Adicao:
                    MainPage.Current.SeAtualizar(Symbol.Add, "Adicionar motorista");
                    break;
                case TipoOperacao.Edicao:
                    MainPage.Current.SeAtualizar(Symbol.Edit, "Editar motorista");
                    break;
            }
            DataContext = new MotoristaDataContext(ref motorista);
        }

        private void Confirmar_Click(object sender, RoutedEventArgs e)
        {
            if (new ValidadorMotorista(motorista).Validar(Log))
            {
                using (var db = new Motoristas())
                {
                    if (tipoRequisitado == TipoOperacao.Adicao)
                    {
                        db.Adicionar(motorista);
                        Log.Escrever(TitulosComuns.Sucesso, "Motorista salvo com sucesso.");
                    }
                    else
                    {
                        db.Atualizar(motorista);
                        Log.Escrever(TitulosComuns.Sucesso, "Motorista alterado com sucesso.");
                    }
                }
                MainPage.Current.Retornar();
            }
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            MainPage.Current.Retornar();
        }
    }
}
