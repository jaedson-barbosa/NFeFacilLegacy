using NFeFacil.ViewModel;
using NFeFacil.ViewModel.ImpostosProduto;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesImpostos;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using System;
using System.Collections;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class ManipulacaoProdutoCompleto : Page, IHambuguer
    {
        bool finalizacaoCompleta;

        public ManipulacaoProdutoCompleto()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is GrupoViewBanco<DetalhesProdutos> grupo)
            {
                var produto = grupo.ItemBanco;
                MainPage.Current.SeAtualizar(Symbol.View, "Produto");
                DataContext = new ProdutoCompletoDataContext(produto, false);
                finalizacaoCompleta = true;
            }
            else
            {
                var produto = e.Parameter as DetalhesProdutos;
                if (produto.Impostos.impostos.Count > 0)
                {
                    MainPage.Current.SeAtualizar(Symbol.Edit, "Produto");
                    DataContext = new ProdutoCompletoDataContext(produto);
                    finalizacaoCompleta = false;
                }
                else
                {
                    MainPage.Current.SeAtualizar(Symbol.Add, "Produto");
                    DataContext = new ProdutoCompletoDataContext(produto);
                    finalizacaoCompleta = true;
                }
            }
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

        public IEnumerable ConteudoMenu
        {
            get
            {
                var retorno = new ObservableCollection<Controles.ItemHambuguer>
                {
                    new Controles.ItemHambuguer(Symbol.Tag, "Dados"),
                    new Controles.ItemHambuguer("\uE825", "Tributos"),
                    new Controles.ItemHambuguer(Symbol.Comment, "Info adicional"),
                    new Controles.ItemHambuguer(Symbol.World, "Importação"),
                    new Controles.ItemHambuguer(Symbol.World, "Exportação"),
                    new Controles.ItemHambuguer(Symbol.Target, "Produto específico")
                };
                main.SelectionChanged += (sender, e) => MainMudou?.Invoke(this, new NewIndexEventArgs { NewIndex = main.SelectedIndex });
                return retorno;
            }
        }

        public event EventHandler MainMudou;
        public void AtualizarMain(int index) => main.SelectedIndex = index;

        private void Concluir_Click(object sender, RoutedEventArgs e)
        {
            if (finalizacaoCompleta)
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
            }

            MainPage.Current.Retornar();
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            MainPage.Current.Retornar();
        }
    }
}
