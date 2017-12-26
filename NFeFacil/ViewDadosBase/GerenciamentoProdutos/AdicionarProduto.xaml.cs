using NFeFacil.Validacao;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System;
using NFeFacil.ItensBD;
using System.Xml.Linq;
using NFeFacil.ModeloXML;
using NFeFacil.ViewNFe.ProdutoEspecial;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewDadosBase.GerenciamentoProdutos
{
    [View.DetalhePagina(Symbol.Shop, "Produto")]
    public sealed partial class AdicionarProduto : Page
    {
        ProdutoDI Produto;

        int TipoEspecialEscolhido
        {
            get
            {
                if (string.IsNullOrEmpty(Produto.ProdutoEspecial)) return 0;
                else
                {
                    var xml = XElement.Parse(Produto.ProdutoEspecial);
                    return (int)Enum.Parse(typeof(ProdutoDI.TiposProduto), xml.Name.LocalName);
                }
            }
            set
            {
                var prod = (IProdutoEspecial)Produto;
                switch (value)
                {
                    case 0:
                        Produto.ResetEspecial();
                        break;
                    case 1:
                        MainPage.Current.Navegar<DefinirVeiculo>(Produto);
                        break;
                    case 2:
                        MainPage.Current.Navegar<DefinirMedicamentos>(Produto);
                        break;
                    case 3:
                        MainPage.Current.Navegar<DefinirArmamentos>(Produto);
                        break;
                    case 4:
                        MainPage.Current.Navegar<DefinirCombustivel>(Produto);
                        break;
                    case 5:
                        DefinirPapel();
                        break;
                    default:
                        break;
                }

                async void DefinirPapel()
                {
                    var caixa = new DefinirPapel(Produto);
                    if (await caixa.ShowAsync() == ContentDialogResult.Primary)
                    {
                        Produto.ResetEspecial();
                        prod.NRECOPI = caixa.NRECOPI;
                    }
                }
            }
        }

        public AdicionarProduto()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Produto = (ProdutoDI)e.Parameter;
        }

        private void Confirmar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (new ValidarDados().ValidarTudo(true,
                    (string.IsNullOrEmpty(Produto.CodigoProduto), "Não foi informado o código do Produto"),
                    (string.IsNullOrEmpty(Produto.Descricao), "Não foi informada uma breve descrição do Produto"),
                    (string.IsNullOrEmpty(Produto.CFOP), "Não foi informado o CFOP do Produto")))
                {
                    using (var repo = new Repositorio.Escrita())
                    {
                        repo.SalvarItemSimples(Produto, DefinicoesTemporarias.DateTimeNow);
                    }
                    MainPage.Current.Retornar();
                }
            }
            catch (Exception erro)
            {
                erro.ManipularErro();
            }
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            MainPage.Current.Retornar();
        }

        void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            TipoEspecialEscolhido = TipoEspecialEscolhido;
        }
    }
}
