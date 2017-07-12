using BibliotecaCentral.ItensBD;
using NFeFacil.ViewModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class ManipulacaoRegistroVenda : Page
    {
        public ManipulacaoRegistroVenda()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            GrupoViewBanco<RegistroVenda> parametro;
            if (e.Parameter == null)
            {
                parametro = new GrupoViewBanco<RegistroVenda>
                {
                    ItemBanco = new RegistroVenda(),
                    OperacaoRequirida = TipoOperacao.Adicao
                };
            }
            else
            {
                parametro = (GrupoViewBanco<RegistroVenda>)e.Parameter;
            }
            var venda = parametro.ItemBanco;
            switch (parametro.OperacaoRequirida)
            {
                case TipoOperacao.Adicao:
                    MainPage.Current.SeAtualizarEspecial("\uEC59", "Venda", ExibicaoExtra.EscolherVendedor, null);
                    break;
                case TipoOperacao.Edicao:
                    MainPage.Current.SeAtualizarEspecial("\uEC59", "Venda", ExibicaoExtra.ExibirVendedor, venda.Vendedor);
                    break;
            }
            DataContext = new RegistroVendaDataContext(venda, parametro.OperacaoRequirida);
        }
    }
}
