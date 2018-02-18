using NFeFacil.Certificacao;
using NFeFacil.Sincronizacao;
using System;
using System.IO;
using Windows.Storage.Pickers;
using Windows.System.Profile;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

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
                "Modos de busca",
                "Background",
                "DANFE NFCe",
                "Compras"
            };
            AnalisarCompras();
        }

        string[] ItensMenu { get; }
        int ItemMenuSelecionado
        {
            get => mvwPrincipal.SelectedIndex;
            set => mvwPrincipal.SelectedIndex = value;
        }

        bool Servidor
        {
            get => ConfiguracoesSincronizacao.Tipo == TipoAppSincronizacao.Servidor;
            set => ConfiguracoesSincronizacao.Tipo = value ? TipoAppSincronizacao.Servidor : TipoAppSincronizacao.Cliente;
        }

        bool Cliente => !Servidor;

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

        bool UsarSOAP11 => !UsarSOAP12;
        bool UsarSOAP12
        {
            get => DefinicoesPermanentes.UsarSOAP12;
            set => DefinicoesPermanentes.UsarSOAP12 = value;
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

        async void UsarCor(object sender, TappedRoutedEventArgs e)
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

        async void Resetar(object sender, TappedRoutedEventArgs e)
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

        async void AnalisarCompras()
        {
            var comprado = await ComprasInApp.ObterProduto(Compras.NFCe);
            btnComprarNFCe.IsEnabled = !comprado.IsInUserCollection;
            comprado = await ComprasInApp.ObterProduto(Compras.Personalizacao);
            btnComprarBackground.IsEnabled = !comprado.IsInUserCollection;
            itnBackground.IsEnabled = comprado.IsInUserCollection;
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
    }
}
