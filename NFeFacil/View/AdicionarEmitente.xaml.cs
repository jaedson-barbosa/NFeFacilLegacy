using BibliotecaCentral.Log;
using BibliotecaCentral.Validacao;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using BibliotecaCentral.Repositorio;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class AdicionarEmitente : Page
    {
        private Emitente emitente;
        private TipoOperacao tipoRequisitado;
        private ILog Log = new Popup();

        public AdicionarEmitente()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            GrupoViewBanco<Emitente> parametro;
            if (e.Parameter == null)
            {
                parametro = new GrupoViewBanco<Emitente>
                {
                    ItemBanco = new Emitente(),
                    OperacaoRequirida = TipoOperacao.Adicao
                };
            }
            else
            {
                parametro = (GrupoViewBanco<Emitente>)e.Parameter;
            }
            emitente = parametro.ItemBanco;
            tipoRequisitado = parametro.OperacaoRequirida;
            switch (tipoRequisitado)
            {
                case TipoOperacao.Adicao:
                    MainPage.Current.SeAtualizar(Telas.GerenciarDadosBase, Symbol.Add, "Adicionar emitente");
                    break;
                case TipoOperacao.Edicao:
                    MainPage.Current.SeAtualizar(Telas.GerenciarDadosBase, Symbol.Edit, "Editar emitente");
                    break;
            }
            DataContext = emitente;
        }

        private void Confirmar_Click(object sender, RoutedEventArgs e)
        {
            if (new ValidadorEmitente(emitente).Validar(Log))
            {
                using (var db = new Emitentes())
                {
                    if (tipoRequisitado == TipoOperacao.Adicao)
                    {
                        db.Adicionar(emitente);
                        Log.Escrever(TitulosComuns.Sucesso, "Emitente salvo com sucesso.");
                    }
                    else
                    {
                        db.Atualizar(emitente);
                        Log.Escrever(TitulosComuns.Sucesso, "Emitente alterado com sucesso.");
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
