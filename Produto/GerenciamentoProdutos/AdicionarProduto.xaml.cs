using BaseGeral.Validacao;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System;
using BaseGeral.ItensBD;
using System.Xml.Linq;
using Venda.Impostos.DetalhamentoICMS;
using System.Collections.ObjectModel;
using Venda.Impostos;
using System.Threading.Tasks;
using System.Linq;
using Windows.UI.Xaml.Data;
using BaseGeral;
using BaseGeral.View;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto;
using System.ComponentModel;
using Venda.Impostos.DetalhamentoICMS.DadosRN;
using Venda.Impostos.DetalhamentoICMS.DadosSN;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Venda.GerenciamentoProdutos
{
    [DetalhePagina(Symbol.Shop, "Produto")]
    public sealed partial class AdicionarProduto : Page, INotifyPropertyChanged
    {
        ProdutoDI Produto;
        ProdutoDIExtended ExtendedProd;

        public event PropertyChangedEventHandler PropertyChanged;

        ObservableCollection<FornecedorDI> Fornecedores { get; set; }
        ObservableCollection<CategoriaDI> Categorias { get; set; }
        ObservableCollection<ExibicaoGenerica> Impostos { get; set; }
        Visibility ClassificavelF { get; set; }
        Visibility ClassificavelC { get; set; }
        Visibility NaoClassificavel => ClassificavelF == Visibility.Visible || ClassificavelC == Visibility.Visible
            ? Visibility.Collapsed : Visibility.Visible;
        int IdCategoria
        {
            get => Categorias.IndexOf(Categorias.FirstOrDefault(x => x.Id == Produto.IdCategoria));
            set => Produto.IdCategoria = Categorias[value].Id;
        }

        bool IsCombustivel
        {
            get => Combustivel != null;
            set
            {
                if (!value) Produto.ResetEspecial();
                else Produto.Combustivel = new Combustivel();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Combustivel)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VisCombustivel)));
            }
        }
        Visibility VisCombustivel => IsCombustivel ? Visibility.Visible : Visibility.Collapsed;
        Combustivel Combustivel => Produto.Combustivel;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Produto = (ProdutoDI)e.Parameter;
            ExtendedProd = Produto;

            var impostosSimples = ExtendedProd.GetImpSimplesArmazenados()?.Select(Convert);
            var icms = ExtendedProd.GetICMSArmazenados()?.Select(Convert);
            if (impostosSimples != null && icms != null)
                Impostos = impostosSimples.Concat(icms).GerarObs();
            else
                Impostos = (impostosSimples ?? icms).GerarObs() ?? new ObservableCollection<ExibicaoGenerica>();

            using (var leitor = new BaseGeral.Repositorio.Leitura())
            {
                Categorias = leitor.ObterCategorias().GerarObs();
                ClassificavelC = Categorias.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
                Fornecedores = leitor.ObterFornecedores().GerarObs();
                ClassificavelF = Fornecedores.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
            }
            InitializeComponent();
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
                    ExtendedProd.SetImpostosPadrao(grdImpostos
                        .SelectedItems
                        .Cast<ExibicaoGenerica>()
                        .Select(x => Convert(x)));
                    using (var repo = new BaseGeral.Repositorio.Escrita())
                        repo.SalvarItemSimples(Produto, DefinicoesTemporarias.DateTimeNow);
                    BasicMainPage.Current.Retornar();
                }
            }
            catch (Exception erro)
            {
                erro.ManipularErro();
            }
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e) => BasicMainPage.Current.Retornar();

        private void RemoverImposto(object sender, RoutedEventArgs e)
        {
            var contexto = (ExibicaoGenerica)((FrameworkElement)sender).DataContext;
            if (contexto is ExibicaoEspecifica<ImpSimplesArmazenado> exib)
            {
                ImpSimplesArmazenado simples = exib;
                ExtendedProd.RemoverImpostoSimples(simples);
            }
            else if (contexto is ExibicaoEspecifica<ICMSArmazenado> exib1)
            {
                ICMSArmazenado icms = exib1;
                ExtendedProd.RemoverICMS(icms);
            }
            Impostos.Remove(contexto);
        }

        async void AdicionarPIS(object sender, RoutedEventArgs e)
        {
            var imp = await AdicionarImpSimples<EscolherTipoPISouCOFINS>(PrincipaisImpostos.PIS);
            if (imp != null)
            {
                ExtendedProd.AdicionarImpostoSimples(imp);
                Impostos.Add(Convert(imp));
            }
        }

        async void AdicionarCOFINS(object sender, RoutedEventArgs e)
        {
            var imp = await AdicionarImpSimples<EscolherTipoPISouCOFINS>(PrincipaisImpostos.COFINS);
            if (imp != null)
            {
                ExtendedProd.AdicionarImpostoSimples(imp);
                Impostos.Add(Convert(imp));
            }
        }

        async void AdicionarIPI(object sender, RoutedEventArgs e)
        {
            var imp = await AdicionarImpSimples<EscolherTipoIPI>(PrincipaisImpostos.IPI);
            if (imp != null)
            {
                var caixa = new CadastroIPI(new Impostos.DetalhamentoIPI.Detalhamento
                {
                    CST = imp.CST,
                    TipoCalculo = imp.TipoCalculo
                });
                if (await caixa.ShowAsync() == ContentDialogResult.Primary)
                {
                    imp.IPI = caixa.Dados.ToXElement<ImpSimplesArmazenado.XMLIPIArmazenado>()
                        .ToString(SaveOptions.DisableFormatting);
                    ExtendedProd.AdicionarImpostoSimples(imp);
                    Impostos.Add(Convert(imp));
                }
            }
        }

        async void AdicionarICMS(object sender, RoutedEventArgs e)
        {
            var escolha = new EscolherTipoICMS();
            if (await escolha.ShowAsync() == ContentDialogResult.Primary)
            {
                var det = new Detalhamento()
                {
                    Origem = escolha.Origem,
                    TipoICMSRN = escolha.TipoICMSRN,
                    TipoICMSSN = escolha.TipoICMSSN
                };
                var cadastro = new CadastroICMS(det);
                if (await cadastro.ShowAsync() == ContentDialogResult.Primary)
                {
                    var (rn, sn) = ProcessarTela(cadastro.Pagina, cadastro.IsRegimeNormal,
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
                    Impostos.Add(Convert(imp));
                }
            }
        }

        static (BaseRN rn, BaseSN sn) ProcessarTela(UserControl Tela, bool normal, string CSOSN, string CST, int origem)
        {
            if (!normal)
            {
                var csosn = int.Parse(CSOSN);
                BaseSN baseSN;
                switch (csosn)
                {
                    case 101:
                        var tipo101 = (Impostos.DetalhamentoICMS.TelasSN.Tipo101)Tela;
                        baseSN = new Tipo101(tipo101);
                        break;
                    case 201:
                        var tipo201 = (Impostos.DetalhamentoICMS.TelasSN.Tipo201)Tela;
                        baseSN = new Tipo201(tipo201);
                        break;
                    case 202:
                        var tipo202 = (Impostos.DetalhamentoICMS.TelasSN.Tipo202)Tela;
                        baseSN = new Tipo202(tipo202);
                        break;
                    case 203:
                        tipo202 = (Impostos.DetalhamentoICMS.TelasSN.Tipo202)Tela;
                        baseSN = new Tipo202(tipo202);
                        break;
                    case 500:
                        var tipo500 = (Impostos.DetalhamentoICMS.TelasSN.Tipo500)Tela;
                        baseSN = new Tipo500(tipo500);
                        break;
                    case 900:
                        var tipo900 = (Impostos.DetalhamentoICMS.TelasSN.Tipo900)Tela;
                        baseSN = new Tipo900(tipo900);
                        break;
                    default:
                        baseSN = new TipoNT();
                        break;
                }
                baseSN.CSOSN = CSOSN;
                baseSN.Origem = origem;
                return (null, baseSN);
            }
            else
            {
                var cst = int.Parse(CST);
                BaseRN baseRN;
                switch (cst)
                {
                    case 0:
                        var tipo00 = (Impostos.DetalhamentoICMS.TelasRN.Tipo0)Tela;
                        baseRN = new Tipo0(tipo00);
                        break;
                    case 10:
                        var tipo10 = (Impostos.DetalhamentoICMS.TelasRN.Tipo10)Tela;
                        baseRN = new Tipo10(tipo10);
                        break;
                    case 1010:
                        var tipoPart = (Impostos.DetalhamentoICMS.TelasRN.TipoPart)Tela;
                        baseRN = new TipoPart(tipoPart);
                        break;
                    case 20:
                        var tipo20 = (Impostos.DetalhamentoICMS.TelasRN.Tipo20)Tela;
                        baseRN = new Tipo20(tipo20);
                        break;
                    case 30:
                        var tipo30 = (Impostos.DetalhamentoICMS.TelasRN.Tipo30)Tela;
                        baseRN = new Tipo30(tipo30);
                        break;
                    case 40:
                        var tipo40 = (Impostos.DetalhamentoICMS.TelasRN.Tipo40_41_50)Tela;
                        baseRN = new Tipo40_41_50(tipo40);
                        break;
                    case 41:
                        tipo40 = (Impostos.DetalhamentoICMS.TelasRN.Tipo40_41_50)Tela;
                        baseRN = new Tipo40_41_50(tipo40);
                        break;
                    case 4141:
                        var tipoST = (Impostos.DetalhamentoICMS.TelasRN.TipoICMSST)Tela;
                        baseRN = new TipoICMSST(tipoST);
                        break;
                    case 50:
                        tipo40 = (Impostos.DetalhamentoICMS.TelasRN.Tipo40_41_50)Tela;
                        baseRN = new Tipo40_41_50(tipo40);
                        break;
                    case 51:
                        var tipo51 = (Impostos.DetalhamentoICMS.TelasRN.Tipo51)Tela;
                        baseRN = new Tipo51(tipo51);
                        break;
                    case 60:
                        var tipo60 = (Impostos.DetalhamentoICMS.TelasRN.Tipo60)Tela;
                        baseRN = new Tipo60(tipo60);
                        break;
                    case 70:
                        var tipo70 = (Impostos.DetalhamentoICMS.TelasRN.Tipo70)Tela;
                        baseRN = new Tipo70(tipo70);
                        break;
                    case 90:
                        var tipo90 = (Impostos.DetalhamentoICMS.TelasRN.Tipo90)Tela;
                        baseRN = new Tipo90(tipo90);
                        break;
                    case 9090:
                        tipoPart = (Impostos.DetalhamentoICMS.TelasRN.TipoPart)Tela;
                        baseRN = new TipoPart(tipoPart);
                        break;
                    default:
                        throw new Exception("CST desconhecido.");
                }
                baseRN.CST = CST.Substring(0, 2);
                baseRN.Origem = origem;
                return (baseRN, null);
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

        void GrdImpostos_Loaded(object sender, RoutedEventArgs e)
        {
            var grd = (GridView)sender;
            if (grd.Items?.Count > 0)
            {
                var imps = ExtendedProd.GetImpostosPadrao();
                if (imps != null)
                {
                    for (int i = 0; i < grd.Items.Count; i++)
                    {
                        var exib = (ExibicaoGenerica)grd.Items[i];
                        var atual = Convert(exib);
                        var (Tipo, NomeTemplate, CST) = imps.FirstOrDefault(x => x.Tipo == atual.Tipo && x.NomeTemplate == atual.NomeTemplate && x.CST == atual.CST);
                        if (!string.IsNullOrEmpty(NomeTemplate)) grd.SelectRange(new ItemIndexRange(i, 1));
                    }
                }
            }
        }

        ExibicaoGenerica Convert(ImpSimplesArmazenado imp)
        {
            return new ExibicaoEspecifica<ImpSimplesArmazenado>(
                imp,
                imp.Tipo.ToString(),
                imp.NomeTemplate,
                imp.CST.ToString());
        }

        ExibicaoGenerica Convert(ICMSArmazenado imp)
        {
            return new ExibicaoEspecifica<ICMSArmazenado>(
                imp,
                "ICMS",
                imp.NomeTemplate,
                imp.CST.ToString());
        }

        ImpostoArmazenado Convert(ExibicaoGenerica contexto)
        {
            if (contexto is ExibicaoEspecifica<ImpSimplesArmazenado> exib)
            {
                ImpSimplesArmazenado simples = exib;
                return simples;
            }
            else if (contexto is ExibicaoEspecifica<ICMSArmazenado> exib1)
            {
                ICMSArmazenado icms = exib1;
                return icms;
            }
            throw new ArgumentException();
        }
    }
}
