using NFeFacil.Validacao;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System;
using NFeFacil.ItensBD;
using System.Xml.Linq;
using NFeFacil.ModeloXML;
using NFeFacil.ViewNFe.Impostos.DetalhamentoICMS;
using NFeFacil.ViewNFe.ProdutoEspecial;
using System.Collections.ObjectModel;
using NFeFacil.ItensBD.Produto;
using NFeFacil.ViewNFe.Impostos;
using NFeFacil.ViewNFe;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.ViewDadosBase.GerenciamentoProdutos
{
    [View.DetalhePagina(Symbol.Shop, "Produto")]
    public sealed partial class AdicionarProduto : Page
    {
        ProdutoDI Produto;
        ObservableCollection<ImpSimplesArmazenado> ImpostosSimples { get; set; }
        ObservableCollection<ICMSArmazenado> ICMS { get; set; }

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
            ImpostosSimples = Produto.GetImpSimplesArmazenados()?.GerarObs()
                ?? new ObservableCollection<ImpSimplesArmazenado>();
            ICMS = Produto.GetICMSArmazenados()?.GerarObs()
                ?? new ObservableCollection<ICMSArmazenado>();
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

        private void Cancelar_Click(object sender, RoutedEventArgs e) => MainPage.Current.Retornar();

        void EditarEspecial(object sender, RoutedEventArgs e) => TipoEspecialEscolhido = TipoEspecialEscolhido;

        void RemoverImpostoSimples(object sender, RoutedEventArgs e)
        {
            var imp = (ImpSimplesArmazenado)((FrameworkElement)sender).DataContext;
            ImpostosSimples.Remove(imp);
            Produto.RemoverImpostoSimples(imp);
        }

        void RemoverImpostoComplexo(object sender, RoutedEventArgs e)
        {
            var imp = (ICMSArmazenado)((FrameworkElement)sender).DataContext;
            ICMS.Remove(imp);
            Produto.RemoverICMS(imp);
        }

        void AdicionarPIS(object sender, RoutedEventArgs e)
        {
            AdicionarImpSimples<EscolherTipoPISouCOFINS>(PrincipaisImpostos.PIS);
        }

        void AdicionarCOFINS(object sender, RoutedEventArgs e)
        {
            AdicionarImpSimples<EscolherTipoPISouCOFINS>(PrincipaisImpostos.COFINS);
        }

        void AdicionarIPI(object sender, RoutedEventArgs e)
        {
            AdicionarImpSimples<EscolherTipoIPI>(PrincipaisImpostos.IPI);
        }

        async void AdicionarICMS(object sender, RoutedEventArgs e)
        {
            var escolha = new EscolherTipoICMS();
            if (await escolha.ShowAsync() == ContentDialogResult.Primary)
            {
                var cadastro = new CadastroICMS(escolha.TipoICMSRN, escolha.TipoICMSSN);
                if (await cadastro.ShowAsync() == ContentDialogResult.Primary)
                {
                    var (rn, sn) = Processamento.ProcessarTela(cadastro.Pagina, cadastro.IsRegimeNormal,
                        escolha.TipoICMSSN, escolha.TipoICMSRN, escolha.Origem);
                    var imp = new ICMSArmazenado
                    {
                        CST = rn?.CST ?? sn?.CSOSN,
                        IsRegimeNormal = cadastro.IsRegimeNormal,
                        NomeTemplate = cadastro.NomeModelo,
                        RegimeNormal = rn,
                        SimplesNacional = sn,
                        Tipo = PrincipaisImpostos.ICMS,
                        EdicaoAtivada = cadastro.EdicaoAtivada
                    };
                    Produto.AdicionarICMS(imp);
                    ICMS.Add(imp);

                }
            }
        }

        async void AdicionarImpSimples<T>(PrincipaisImpostos tipo) where T : ContentDialog, IEscolherImpSimples, new()
        {
            var escolha = new T();
            if (await escolha.ShowAsync() == ContentDialogResult.Primary)
            {
                var cadastro = new CadastroImpSimples(escolha.TipoCalculo);
                if (await cadastro.ShowAsync() == ContentDialogResult.Primary)
                {
                    var imp = new ImpSimplesArmazenado
                    {
                        Aliquota = cadastro.Aliquota,
                        CST = escolha.CST,
                        NomeTemplate = cadastro.NomeModelo,
                        Tipo = tipo,
                        TipoCalculo = escolha.TipoCalculo,
                        Valor = cadastro.Valor,
                        EdicaoAtivada = cadastro.EdicaoAtivada
                    };
                    Produto.AdicionarImpostoSimples(imp);
                    ImpostosSimples.Add(imp);
                }
            }
        }
    }
}
