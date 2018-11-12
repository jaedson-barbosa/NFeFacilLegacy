using BaseGeral;
using BaseGeral.ItensBD;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace RegistroComum
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class GeradorRelatorioProduto01 : Page
    {
        ObservableCollection<CategoriaDI> CategoriasDisponiveis, CategoriasEscolhidas;
        ObservableCollection<FornecedorDI> FornecedoresDisponiveis, FornecedoresEscolhidos;

        bool InserirProdutosSemCategoria, InserirProdutosSemFornecedor;

        public GeradorRelatorioProduto01()
        {
            IniciarListas();
            InitializeComponent();
        }

        void IniciarListas()
        {
            using (var leitura = new BaseGeral.Repositorio.Leitura())
            {
                CategoriasDisponiveis = leitura.ObterCategorias().GerarObs();
                FornecedoresDisponiveis = leitura.ObterFornecedores().GerarObs();
            }
            CategoriasEscolhidas = new ObservableCollection<CategoriaDI>();
            FornecedoresEscolhidos = new ObservableCollection<FornecedorDI>();
        }

        void CategoriaAdicionada(object sender, ItemClickEventArgs e) => AdicionarCategoria((CategoriaDI)e.ClickedItem);
        void FornecedorAdicionado(object sender, ItemClickEventArgs e) => AdicionarFornecedor((FornecedorDI)e.ClickedItem);
        void CategoriaRemovida(object sender, ItemClickEventArgs e) => RemoverCategoria((CategoriaDI)e.ClickedItem);
        void FornecedorRemovido(object sender, ItemClickEventArgs e) => RemoverFornecedor((FornecedorDI)e.ClickedItem);

        private void TodasCategorias(object sender, RoutedEventArgs e)
        {
            while (CategoriasDisponiveis.Count > 0)
                AdicionarCategoria(CategoriasDisponiveis[0]);
        }

        void TodosFornecedores(object sender, RoutedEventArgs e)
        {
            while (FornecedoresDisponiveis.Count > 0)
                AdicionarFornecedor(FornecedoresDisponiveis[0]);
        }

        void NenhumaCategoria(object sender, RoutedEventArgs e)
        {
            while (CategoriasEscolhidas.Count > 0)
                RemoverCategoria(CategoriasEscolhidas[0]);
        }

        void NenhumFornecedor(object sender, RoutedEventArgs e)
        {
            while (FornecedoresEscolhidos.Count > 0)
                RemoverFornecedor(FornecedoresEscolhidos[0]);
        }

        void AdicionarCategoria(CategoriaDI categoria)
        {
            CategoriasEscolhidas.Add(categoria);
            CategoriasDisponiveis.Remove(categoria);
        }

        void AdicionarFornecedor(FornecedorDI fornecedor)
        {
            FornecedoresEscolhidos.Add(fornecedor);
            FornecedoresDisponiveis.Remove(fornecedor);
        }

        void RemoverCategoria(CategoriaDI categoria)
        {
            CategoriasDisponiveis.Add(categoria);
            CategoriasEscolhidas.Remove(categoria);
        }

        void RemoverFornecedor(FornecedorDI fornecedor)
        {
            FornecedoresDisponiveis.Add(fornecedor);
            FornecedoresEscolhidos.Remove(fornecedor);
        }
    }
}
