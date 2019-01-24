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
                var caixa = new CadastroIPI();
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
                    Impostos.Add(Convert(imp));
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
