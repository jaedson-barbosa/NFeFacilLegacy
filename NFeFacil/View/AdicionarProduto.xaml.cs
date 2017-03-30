using NFeFacil.ItensBD;
using NFeFacil.Log;
using NFeFacil.Validacao;
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
    public sealed partial class AdicionarProduto : Page, IEsconde
    {
        private ProdutoDI Produto;
        private TipoOperacao tipoRequisitado;
        private ILog Log = new Popup();

        public AdicionarProduto()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var parametro = (GrupoViewBanco<ProdutoDI>)e.Parameter;
            Produto = parametro.ItemBanco ?? new ProdutoDI();
            tipoRequisitado = parametro.OperacaoRequirida;
            DataContext = Produto;
        }

        private void Confirmar_Click(object sender, RoutedEventArgs e)
        {
            if (new ValidadorProduto(Produto).Validar(Log))
            {
                using (var db = new AplicativoContext())
                {
                    if (tipoRequisitado == TipoOperacao.Adicao)
                    {
                        db.Add(Produto);
                        Log.Escrever(TitulosComuns.Sucesso, "Produto salvo com sucesso.");
                    }
                    else
                    {
                        db.Update(Produto);
                        Log.Escrever(TitulosComuns.Sucesso, "Produto alterado com sucesso.");
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
