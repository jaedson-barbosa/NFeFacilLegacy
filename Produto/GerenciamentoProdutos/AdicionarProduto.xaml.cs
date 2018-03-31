using BaseGeral.Validacao;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System;
using BaseGeral.ItensBD;
using System.Xml.Linq;
using BaseGeral.ModeloXML;
using Venda.Impostos.DetalhamentoICMS;
using Venda.ProdutoEspecial;
using System.Collections.ObjectModel;
using Venda.Impostos;
using System.Threading.Tasks;
using System.Linq;
using Windows.UI.Xaml.Data;
using BaseGeral;
using BaseGeral.View;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Venda.GerenciamentoProdutos
{
    [DetalhePagina(Symbol.Shop, "Produto")]
    public sealed partial class AdicionarProduto : Page
    {
        ProdutoDI Produto;
        ProdutoDIExtended ExtendedProd;
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
                        BasicMainPage.Current.Navegar<DefinirVeiculo>(Produto);
                        break;
                    case 2:
                        BasicMainPage.Current.Navegar<DefinirMedicamentos>(Produto);
                        break;
                    case 3:
                        BasicMainPage.Current.Navegar<DefinirArmamentos>(Produto);
                        break;
                    case 4:
                        BasicMainPage.Current.Navegar<DefinirCombustivel>(Produto);
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
            ExtendedProd = Produto;
            ImpostosSimples = ExtendedProd.GetImpSimplesArmazenados()?.GerarObs()
                ?? new ObservableCollection<ImpSimplesArmazenado>();
            ICMS = ExtendedProd.GetICMSArmazenados()?.GerarObs()
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
                    var simples = grdImpostosSimples.SelectedItems.Cast<ImpostoArmazenado>();
                    var icmss = grdICMSs.SelectedItems.Cast<ImpostoArmazenado>();
                    ExtendedProd.SetImpostosPadrao(simples.Concat(icmss));
                    using (var repo = new BaseGeral.Repositorio.Escrita())
                    {
                        repo.SalvarItemSimples(Produto, DefinicoesTemporarias.DateTimeNow);
                    }
                    BasicMainPage.Current.Retornar();
                }
            }
            catch (Exception erro)
            {
                erro.ManipularErro();
            }
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e) => BasicMainPage.Current.Retornar();

        void EditarEspecial(object sender, RoutedEventArgs e) => TipoEspecialEscolhido = TipoEspecialEscolhido;

        void RemoverImpostoSimples(object sender, RoutedEventArgs e)
        {
            var imp = (ImpSimplesArmazenado)((FrameworkElement)sender).DataContext;
            ImpostosSimples.Remove(imp);
            ExtendedProd.RemoverImpostoSimples(imp);
        }

        void RemoverImpostoComplexo(object sender, RoutedEventArgs e)
        {
            var imp = (ICMSArmazenado)((FrameworkElement)sender).DataContext;
            ICMS.Remove(imp);
            ExtendedProd.RemoverICMS(imp);
        }

        async void AdicionarPIS(object sender, RoutedEventArgs e)
        {
            var imp = await AdicionarImpSimples<EscolherTipoPISouCOFINS>(PrincipaisImpostos.PIS);
            if (imp != null)
            {
                ExtendedProd.AdicionarImpostoSimples(imp);
                ImpostosSimples.Add(imp);
            }
        }

        async void AdicionarCOFINS(object sender, RoutedEventArgs e)
        {
            var imp = await AdicionarImpSimples<EscolherTipoPISouCOFINS>(PrincipaisImpostos.COFINS);
            if (imp != null)
            {
                ExtendedProd.AdicionarImpostoSimples(imp);
                ImpostosSimples.Add(imp);
            }
        }

        async void AdicionarIPI(object sender, RoutedEventArgs e)
        {
            var imp = await AdicionarImpSimples<EscolherTipoIPI>(PrincipaisImpostos.IPI);
            if (imp != null)
            {
                var caixa = new CadastroIPI();
                if (await caixa.ShowAsync() == ContentDialogResult.Primary)
                {
                    imp.IPI = caixa.Dados.ToXElement<ImpSimplesArmazenado.XMLIPIArmazenado>()
                        .ToString(SaveOptions.DisableFormatting);
                    ExtendedProd.AdicionarImpostoSimples(imp);
                    ImpostosSimples.Add(imp);
                }
            }
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
                        CST = int.Parse(rn?.CST ?? sn?.CSOSN),
                        IsRegimeNormal = cadastro.IsRegimeNormal,
                        NomeTemplate = cadastro.NomeModelo,
                        RegimeNormal = rn,
                        SimplesNacional = sn,
                        Tipo = PrincipaisImpostos.ICMS,
                        EdicaoAtivada = cadastro.EdicaoAtivada
                    };
                    ExtendedProd.AdicionarICMS(imp);
                    ICMS.Add(imp);

                }
            }
        }

        async Task<ImpSimplesArmazenado> AdicionarImpSimples<T>(PrincipaisImpostos tipo) where T : ContentDialog, IEscolherImpSimples, new()
        {
            var escolha = new T();
            if (await escolha.ShowAsync() == ContentDialogResult.Primary)
            {
                var cadastro = new CadastroImpSimples(escolha.TipoCalculo);
                if (await cadastro.ShowAsync() == ContentDialogResult.Primary)
                {
                    return new ImpSimplesArmazenado
                    {
                        Aliquota = cadastro.Aliquota,
                        CST = int.Parse(escolha.CST),
                        NomeTemplate = cadastro.NomeModelo,
                        Tipo = tipo,
                        TipoCalculo = escolha.TipoCalculo,
                        Valor = cadastro.Valor,
                        EdicaoAtivada = cadastro.EdicaoAtivada
                    };
                }
            }
            return null;
        }

        private void grdImpostosSimples_Loaded(object sender, RoutedEventArgs e)
        {
            if (grdImpostosSimples.Items?.Count > 0)
            {
                var imps = ExtendedProd.GetImpostosPadrao();
                if (imps != null)
                {
                    for (int i = 0; i < grdImpostosSimples.Items.Count; i++)
                    {
                        var atual = (ImpostoArmazenado)grdImpostosSimples.Items[i];
                        var (Tipo, NomeTemplate, CST) = imps.FirstOrDefault(x => x.Tipo == atual.Tipo && x.NomeTemplate == atual.NomeTemplate && x.CST == atual.CST);
                        if (!string.IsNullOrEmpty(NomeTemplate)) grdImpostosSimples.SelectRange(new ItemIndexRange(i, 1));
                    }
                }
            }
        }

        private void grdICMSs_Loaded(object sender, RoutedEventArgs e)
        {
            if (grdICMSs.Items?.Count > 0)
            {
                var imps = ExtendedProd.GetImpostosPadrao();
                if (imps != null)
                {
                    for (int i = 0; i < grdICMSs.Items.Count; i++)
                    {
                        var atual = (ImpostoArmazenado)grdICMSs.Items[i];
                        var (Tipo, NomeTemplate, CST) = imps.FirstOrDefault(x => x.Tipo == atual.Tipo && x.NomeTemplate == atual.NomeTemplate && x.CST == atual.CST);
                        if (!string.IsNullOrEmpty(NomeTemplate)) grdICMSs.SelectRange(new ItemIndexRange(i, 1));
                    }
                }
            }
        }
    }
}
