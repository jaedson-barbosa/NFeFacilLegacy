using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;
using NFeFacil.ViewModel;
using NFeFacil.ViewModel.ImpostosProduto;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class ManipulacaoProdutoCompleto : Page, IHambuguer
    {
        public ManipulacaoProdutoCompleto()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var produto = e.Parameter as DetalhesProdutos;
            if (produto.Impostos.impostos.Count > 0)
            {
                MainPage.Current.SeAtualizar(Symbol.Edit, "Produto");
            }
            else
            {
                MainPage.Current.SeAtualizar(Symbol.Add, "Produto");
            }
            DataContext = new ProdutoCompletoDataContext(produto);
        }

        private Impostos ImpostosFiltrados
        {
            get
            {
                var lista = new List<Imposto>();
                for (int i = 0; i < pvtImpostos.Items.Count; i++)
                {
                    var filho = (pvtImpostos.Items[i] as PivotItem).Content as FrameworkElement;
                    var contexto = filho.DataContext;
                    if (contexto is Imposto imposto)
                    {
                        lista.Add(imposto);
                    }
                    else if (contexto is IImpostoDataContext impostoContexto)
                    {
                        lista.Add(impostoContexto.ImpostoBruto);
                    }
                    else if (contexto is IImpostosUnidos impostos)
                    {
                        lista.AddRange(impostos.SepararImpostos());
                    }
                }
                return new Impostos(from i in lista
                                    where i != null && i.IsValido
                                    select i);
            }
        }

        public ListView ConteudoMenu
        {
            get
            {
                var lista = new ListView()
                {
                    ItemsSource = new ObservableCollection<Controles.ItemHambuguer>
                    {
                        new Controles.ItemHambuguer(Symbol.Tag, "Dados"),
                        new Controles.ItemHambuguer("\uE825", "Tributos"),
                        new Controles.ItemHambuguer(Symbol.Comment, "Info adicional"),
                        new Controles.ItemHambuguer(Symbol.World, "Importação"),
                        new Controles.ItemHambuguer(Symbol.World, "Exportação"),
                        new Controles.ItemHambuguer(Symbol.Target, "Produto específico")
                    },
                    SelectedIndex = 0
                };
                main.SelectionChanged += (sender, e) => lista.SelectedIndex = main.SelectedIndex;
                lista.SelectionChanged += (sender, e) => main.SelectedIndex = lista.SelectedIndex;
                return lista;
            }
        }

        private void Salvar_Click(object sender, RoutedEventArgs e)
        {
            var parametro = Frame.BackStack[Frame.BackStack.Count - 1].Parameter as ConjuntoManipuladorNFe;
            var info = parametro.NotaSalva.Informações;

            var data = DataContext as ProdutoCompletoDataContext;
            data.ProdutoCompleto.Impostos = ImpostosFiltrados;

            var detalhes = data.ProdutoCompleto;
            if (detalhes.Número == 0)
            {
                detalhes.Número = info.produtos.Count + 1;
                info.produtos.Add(detalhes);
            }
            else
            {
                info.produtos[detalhes.Número - 1] = detalhes;
            }
            info.total = new Total(info.produtos);

            MainPage.Current.Retornar();
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            MainPage.Current.Retornar();
        }
    }
}
