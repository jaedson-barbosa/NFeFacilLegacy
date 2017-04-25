using BibliotecaCentral.Log;
using BibliotecaCentral.Validacao;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;
using BibliotecaCentral.Repositorio;
using BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class AdicionarProduto : Page, IEsconde
    {
        private BaseProdutoOuServico Produto;
        private TipoOperacao tipoRequisitado;
        private ILog Log = new Popup();

        public AdicionarProduto()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var parametro = (GrupoViewBanco<BaseProdutoOuServico>)e.Parameter;
            Produto = parametro.ItemBanco ?? new BaseProdutoOuServico();
            tipoRequisitado = parametro.OperacaoRequirida;
            switch (tipoRequisitado)
            {
                case TipoOperacao.Adicao:
                    Propriedades.Intercambio.SeAtualizar(Telas.GerenciarDadosBase, Symbol.Add, "Adicionar produto");
                    break;
                case TipoOperacao.Edicao:
                    Propriedades.Intercambio.SeAtualizar(Telas.GerenciarDadosBase, Symbol.Edit, "Editar produto");
                    break;
                default:
                    break;
            }
            DataContext = Produto;
        }

        private void Confirmar_Click(object sender, RoutedEventArgs e)
        {
            if (new ValidadorProduto(Produto).Validar(Log))
            {
                using (var db = new Produtos())
                {
                    if (tipoRequisitado == TipoOperacao.Adicao)
                    {
                        db.Adicionar(Produto);
                        Log.Escrever(TitulosComuns.Sucesso, "Produto salvo com sucesso.");
                    }
                    else
                    {
                        db.Atualizar(Produto);
                        Log.Escrever(TitulosComuns.Sucesso, "Produto alterado com sucesso.");
                    }
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
