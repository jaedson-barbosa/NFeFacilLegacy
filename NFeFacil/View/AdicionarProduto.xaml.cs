using BibliotecaCentral.Log;
using BibliotecaCentral.Validacao;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using BibliotecaCentral.Repositorio;
using BibliotecaCentral.ItensBD;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class AdicionarProduto : Page
    {
        private ProdutoDI Produto;
        private TipoOperacao tipoRequisitado;
        private ILog Log = new Popup();

        public AdicionarProduto()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            GrupoViewBanco<ProdutoDI> parametro;
            if (e.Parameter == null)
            {
                parametro = new GrupoViewBanco<ProdutoDI>
                {
                    ItemBanco = new ProdutoDI(),
                    OperacaoRequirida = TipoOperacao.Adicao
                };
            }
            else
            {
                parametro = (GrupoViewBanco<ProdutoDI>)e.Parameter;
            }
            Produto = parametro.ItemBanco;
            tipoRequisitado = parametro.OperacaoRequirida;
            switch (tipoRequisitado)
            {
                case TipoOperacao.Adicao:
                    MainPage.Current.SeAtualizar(Symbol.Add, "Produto");
                    break;
                case TipoOperacao.Edicao:
                    MainPage.Current.SeAtualizar(Symbol.Edit, "Produto");
                    break;
            }
            DataContext = Produto;
        }

        private void Confirmar_Click(object sender, RoutedEventArgs e)
        {
            try
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
                    MainPage.Current.Retornar();
                }
            }
            catch (System.Exception erro)
            {
                erro.ManipularErro();
            }
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            MainPage.Current.Retornar();
        }
    }
}
