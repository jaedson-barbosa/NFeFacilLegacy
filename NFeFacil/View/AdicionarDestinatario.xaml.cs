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
    public sealed partial class AdicionarDestinatario : Page
    {
        private Destinatario cliente;
        private TipoOperacao tipoRequisitado;
        private ILog Log = new Popup();

        public AdicionarDestinatario()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            GrupoViewBanco<Destinatario> parametro;
            if (e.Parameter == null)
            {
                parametro = new GrupoViewBanco<Destinatario>
                {
                    ItemBanco = new Destinatario(),
                    OperacaoRequirida = TipoOperacao.Adicao
                };
            }
            else
            {
                parametro = (GrupoViewBanco<Destinatario>)e.Parameter;
            }
            cliente = parametro.ItemBanco;
            tipoRequisitado = parametro.OperacaoRequirida;
            switch (tipoRequisitado)
            {
                case TipoOperacao.Adicao:
                    BarraTitulo.Current.SeAtualizar(Telas.GerenciarDadosBase, Symbol.Add, "Adicionar cliente");
                    break;
                case TipoOperacao.Edicao:
                    BarraTitulo.Current.SeAtualizar(Telas.GerenciarDadosBase, Symbol.Edit, "Editar cliente");
                    break;
            }
            DataContext = cliente;
        }

        private void Confirmar_Click(object sender, RoutedEventArgs e)
        {
            if (new ValidadorDestinatario(cliente).Validar(Log))
            {
                using (var db = new Clientes())
                {
                    if (tipoRequisitado == TipoOperacao.Adicao)
                    {
                        db.Adicionar(cliente);
                        Log.Escrever(TitulosComuns.Sucesso, "Cliente salvo com sucesso.");
                    }
                    else
                    {
                        db.Atualizar(cliente);
                        Log.Escrever(TitulosComuns.Sucesso, "Cliente alterado com sucesso.");
                    }
                }
                BarraTitulo.Current.Retornar();
            }
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            BarraTitulo.Current.Retornar();
        }
    }
}
