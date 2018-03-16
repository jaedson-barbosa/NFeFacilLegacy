using NFeFacil.Certificacao;
using NFeFacil.Sincronizacao;
using System;
using System.IO;
using Windows.Storage.Pickers;
using Windows.System.Profile;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View
{
    [DetalhePagina(Symbol.Setting, "Configurações")]
    public sealed partial class Configuracoes : Page
    {
        public Configuracoes()
        {
            InitializeComponent();
            ItensMenu = new string[]
            {
                "Geral",
                "Backup",
                "Modos de busca",
                "Background",
                "DANFE NFCe",
                "Controle de estoque",
                "Compras"
            };
            AnalisarCompras();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            DefinicoesPermanentes.ConfiguracoesEstoque.SalvarModificacoes();
        }

        string[] ItensMenu { get; }
        int ItemMenuSelecionado
        {
            get => mvwPrincipal.SelectedIndex;
            set => mvwPrincipal.SelectedIndex = value;
        }

        int FuncaoSincronizacao
        {
            get => (int)ConfiguracoesSincronizacao.Tipo;
            set => ConfiguracoesSincronizacao.Tipo = (TipoAppSincronizacao)value;
        }

        int OrigemCertificacao
        {
            get => (int)ConfiguracoesCertificacao.Origem;
            set => ConfiguracoesCertificacao.Origem = (OrigemCertificado)value;
        }
        bool InstalacaoLiberada => AnalyticsInfo.VersionInfo.DeviceFamily.Contains("Desktop");

        bool DesconsiderarHorarioVerao
        {
            get => DefinicoesPermanentes.SuprimirHorarioVerao;
            set => DefinicoesPermanentes.SuprimirHorarioVerao = value;
        }

        bool CalcularNumeroNFe
        {
            get => DefinicoesPermanentes.CalcularNumeroNFe;
            set => DefinicoesPermanentes.CalcularNumeroNFe = value;
        }

        int VersaoSoap
        {
            get => DefinicoesPermanentes.UsarSOAP12 ? 1 : 0;
            set => DefinicoesPermanentes.UsarSOAP12 = value == 1;
        }

        int ModoBuscaProduto
        {
            get => DefinicoesPermanentes.ModoBuscaProduto;
            set => DefinicoesPermanentes.ModoBuscaProduto = value;
        }

        int ModoBuscaCliente
        {
            get => DefinicoesPermanentes.ModoBuscaCliente;
            set => DefinicoesPermanentes.ModoBuscaCliente = value;
        }

        int ModoBuscaComprador
        {
            get => DefinicoesPermanentes.ModoBuscaComprador;
            set => DefinicoesPermanentes.ModoBuscaComprador = value;
        }

        int ModoBuscaMotorista
        {
            get => DefinicoesPermanentes.ModoBuscaMotorista;
            set => DefinicoesPermanentes.ModoBuscaMotorista = value;
        }

        int ModoBuscaVendedor
        {
            get => DefinicoesPermanentes.ModoBuscaVendedor;
            set => DefinicoesPermanentes.ModoBuscaVendedor = value;
        }

        double LarguraDANFENFCe
        {
            get => DefinicoesPermanentes.LarguraDANFENFCe;
            set => DefinicoesPermanentes.LarguraDANFENFCe = value;
        }

        double MargemDANFENFCe
        {
            get => DefinicoesPermanentes.MargemDANFENFCe;
            set => DefinicoesPermanentes.MargemDANFENFCe = value;
        }

        async void UsarImagem(object sender, TappedRoutedEventArgs e)
        {
            var brushAtual = MainPage.Current.ImagemBackground;
            if (DefinicoesPermanentes.IDBackgroung == default(Guid))
            {
                DefinicoesPermanentes.IDBackgroung = Guid.NewGuid();
            }
            var caixa = new DefinirImagem(DefinicoesPermanentes.IDBackgroung, brushAtual);
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                MainPage.Current.ImagemBackground = caixa.Imagem;
                MainPage.Current.DefinirTipoBackground(TiposBackground.Imagem);
            }
        }

        void UsarCor(object sender, TappedRoutedEventArgs e)
        {
            MainPage.Current.DefinirTipoBackground(TiposBackground.Cor);
        }

        async void EscolherTransparencia(object sender, TappedRoutedEventArgs e)
        {
            var caixa = new EscolherTransparencia(DefinicoesPermanentes.OpacidadeBackground);
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                DefinicoesPermanentes.OpacidadeBackground = caixa.Opacidade;
                MainPage.Current.DefinirOpacidadeBackground(caixa.Opacidade);
            }
        }

        void Resetar(object sender, TappedRoutedEventArgs e)
        {
            DefinicoesPermanentes.OpacidadeBackground = 1;
            MainPage.Current.DefinirTipoBackground(TiposBackground.Padrao);
        }

        async void SalvarBackup(object sender, TappedRoutedEventArgs e)
        {
            var objeto = new ConjuntoBanco();
            objeto.AtualizarPadrao();
            var xml = objeto.ToXElement<ConjuntoBanco>().ToString();

            var caixa = new FileSavePicker();
            caixa.FileTypeChoices.Add("Arquivo XML", new string[] { ".xml" });
            var arq = await caixa.PickSaveFileAsync();
            if (arq != null)
            {
                var stream = await arq.OpenStreamForWriteAsync();
                using (StreamWriter escritor = new StreamWriter(stream))
                {
                    await escritor.WriteAsync(xml);
                    await escritor.FlushAsync();
                }
            }
        }

        void AnalisarCompras()
        {
            var comprado = ComprasInApp.Resumo[Compras.NFCe];
            btnComprarNFCe.IsEnabled = !comprado;
            comprado = ComprasInApp.Resumo[Compras.Personalizacao];
            btnComprarBackground.IsEnabled = !comprado;
            itnBackground.IsEnabled = comprado;
        }

        async void ComprarNFCe(object sender, RoutedEventArgs e)
        {
            var comprado = await ComprasInApp.Comprar(Compras.NFCe);
            btnComprarNFCe.IsEnabled = !comprado;
        }

        async void ComprarBackground(object sender, RoutedEventArgs e)
        {
            var comprado = await ComprasInApp.Comprar(Compras.Personalizacao);
            btnComprarBackground.IsEnabled = !comprado;
            itnBackground.IsEnabled = comprado;
        }

        async void ReanalizarCompras(object sender, RoutedEventArgs e)
        {
            await ComprasInApp.AnalisarCompras();
        }
    }
}
