using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes;
using System;
using NFeFacil.Controles;
using System.Threading.Tasks;
using Windows.UI.Popups;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesProdutoOuServico;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using System.Collections.Generic;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewNFe
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class ManipulacaoProdutoCompleto : Page, IHambuguer, IValida
    {
        public DetalhesProdutos ProdutoCompleto { get; private set; }

        public ObservableCollection<DeclaracaoImportacao> ListaDI { get; } = new ObservableCollection<DeclaracaoImportacao>();
        public ObservableCollection<GrupoExportacao> ListaGE { get; } = new ObservableCollection<GrupoExportacao>();

        public ImpostoDevol ContextoImpostoDevol
        {
            get
            {
                if (ProdutoCompleto.ImpostoDevol == null)
                {
                    ProdutoCompleto.ImpostoDevol = new ImpostoDevol();
                }
                return ProdutoCompleto.ImpostoDevol;
            }
        }

        string TipoEspecialEscolhido
        {
            get
            {
                var prod = ProdutoCompleto.Produto;
                if (prod.veicProd != null)
                {
                    return "1";
                }
                else if (prod.medicamentos != null)
                {
                    return "2";
                }
                else if (prod.armas != null)
                {
                    return "3";
                }
                else if (prod.comb != null)
                {
                    return "4";
                }
                else if (prod.NRECOPI != null)
                {
                    return "5";
                }
                else
                {
                    return "0";
                }
            }
            set
            {
                var prod = ProdutoCompleto.Produto;
                switch (int.Parse(value))
                {
                    case 0:
                        prod.veicProd = null;
                        prod.medicamentos = null;
                        prod.armas = null;
                        prod.comb = null;
                        prod.NRECOPI = null;
                        break;
                    case 1:
                        MainPage.Current.Navegar<CaixasEspeciaisProduto.DefinirVeiculo>();
                        break;
                    case 2:
                        MainPage.Current.Navegar<CaixasEspeciaisProduto.DefinirMedicamentos>();
                        break;
                    case 3:
                        MainPage.Current.Navegar<CaixasEspeciaisProduto.DefinirArmamentos>();
                        break;
                    case 4:
                        MainPage.Current.Navegar<CaixasEspeciaisProduto.DefinirCombustivel>();
                        break;
                    case 5:
                        DefinirPapel();
                        break;
                    default:
                        break;
                }

                async void DefinirPapel()
                {
                    var caixa = new CaixasEspeciaisProduto.DefinirPapel();
                    if (await caixa.ShowAsync() == ContentDialogResult.Primary)
                    {
                        prod.veicProd = null;
                        prod.medicamentos = null;
                        prod.armas = null;
                        prod.comb = null;
                        prod.NRECOPI = caixa.NRECOPI;
                    }
                }
            }
        }

        public ManipulacaoProdutoCompleto()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var produto = (DetalhesProdutos)e.Parameter;
            ProdutoCompleto = produto;
        }

        public ObservableCollection<ItemHambuguer> ConteudoMenu => new ObservableCollection<ItemHambuguer>
        {
            new ItemHambuguer(Symbol.Tag, "Dados"),
            new ItemHambuguer("\uE825", "Imposto devolvido"),
            new ItemHambuguer(Symbol.Comment, "Info adicional"),
            new ItemHambuguer(Symbol.World, "Importação"),
            new ItemHambuguer(Symbol.World, "Exportação"),
            new ItemHambuguer(Symbol.Target, "Produto específico")
        };

        public int SelectedIndex { set => main.SelectedIndex = value; }

        private void Concluir_Click(object sender, RoutedEventArgs e)
        {
            ProdutoCompleto.Produto.DI = new List<DeclaracaoImportacao>(ListaDI);
            ProdutoCompleto.Produto.GrupoExportação = new List<GrupoExportacao>(ListaGE);

            var porcentDevolv = ProdutoCompleto.ImpostoDevol.pDevol;
            if (string.IsNullOrEmpty(porcentDevolv) || int.Parse(porcentDevolv) == 0)
            {
                ProdutoCompleto.ImpostoDevol = null;
            }
            MainPage.Current.Navegar<ImpostosProduto>(ProdutoCompleto);
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            MainPage.Current.Retornar();
        }

        async Task<bool> IValida.Verificar()
        {
            var mensagem = new MessageDialog("Se você sair agora, os dados serão perdidos, se tiver certeza, escolha Sair, caso contrário, escolha Cancelar.\r\n" +
                "Mas lembre-se que, caso o produto já tenha sido salvo, as alterações não terão efeito, e caso contrário, o produto não será adicionado.", "Atenção");
            mensagem.Commands.Add(new UICommand("Sair"));
            mensagem.Commands.Add(new UICommand("Cancelar"));
            var resultado = await mensagem.ShowAsync();
            return resultado.Label == "Sair";
        }

        async void AdicionarDeclaracaoImportacao(object sender, RoutedEventArgs e)
        {
            var caixa = new CaixasDialogoProduto.AdicionarDeclaracaoImportacao();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                ListaDI.Add(caixa.Declaracao);
            }
        }

        void RemoverDeclaracaoImportacao(object sender, RoutedEventArgs e)
        {
            var contexto = ((FrameworkElement)sender).DataContext;
            ListaDI.Remove((DeclaracaoImportacao)contexto);
        }

        async void AdicionarDeclaracaoExportacao(object sender, RoutedEventArgs e)
        {
            var caixa = new CaixasDialogoProduto.EscolherTipoDeclaracaoExportacao();
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                var tipo = caixa.TipoEscolhido;
                if (tipo == CaixasDialogoProduto.TiposDeclaracaoExportacao.Direta)
                {
                    var caixa2 = new CaixasDialogoProduto.AddDeclaracaoExportacaoDireta();
                    if (await caixa2.ShowAsync() == ContentDialogResult.Primary)
                    {
                        ListaGE.Add(caixa2.Declaracao);
                    }
                }
                else
                {
                    var caixa2 = new CaixasDialogoProduto.AddDeclaracaoExportacaoIndireta();
                    if (await caixa2.ShowAsync() == ContentDialogResult.Primary)
                    {
                        ListaGE.Add(caixa2.Declaracao);
                    }
                }
            }
        }

        void RemoverDeclaracaoExportacao(object sender, RoutedEventArgs e)
        {
            var contexto = ((FrameworkElement)sender).DataContext;
            ListaGE.Remove((GrupoExportacao)contexto);
        }
    }
}
