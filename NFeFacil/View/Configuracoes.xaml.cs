using BaseGeral;
using BaseGeral.Certificacao;
using BaseGeral.Sincronizacao;
using BaseGeral.View;
using System;
using System.ComponentModel;
using System.IO;
using Windows.Storage.Pickers;
using Windows.System.Profile;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View
{
    [DetalhePagina(Symbol.Setting, "Configurações")]
    public sealed partial class Configuracoes : Page, INotifyPropertyChanged
    {
        public Configuracoes()
        {
            InitializeComponent();
            ItensMenu = new string[]
            {
                "Geral",
                "Modos de busca",
                "Personalização",
                "DANFE NFCe",
                "Controle de estoque",
                "Compras"
            };
            AnalisarCompras();
        }

        public event PropertyChangedEventHandler PropertyChanged;

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

        bool PacotePersonalizacaoComprado { get; set; }
        bool FluentDesign
        {
            get => DefinicoesPermanentes.UsarFluent;
            set => DefinicoesPermanentes.UsarFluent = value;
        }

        async void UsarImagem(object sender, RoutedEventArgs e)
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

        void UsarCor(object sender, RoutedEventArgs e)
        {
            MainPage.Current.DefinirTipoBackground(TiposBackground.Cor);
        }

        async void EscolherTransparencia(object sender, RoutedEventArgs e)
        {
            var caixa = new EscolherTransparencia(DefinicoesPermanentes.OpacidadeBackground);
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                MainPage.Current.DefinirOpacidadeBackground(caixa.Opacidade);
            }
        }

        void Resetar(object sender, RoutedEventArgs e)
        {
            DefinicoesPermanentes.OpacidadeBackground = 1;
            MainPage.Current.DefinirTipoBackground(TiposBackground.Padrao);
        }

        async void SalvarBackup(object sender, RoutedEventArgs e)
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
            PacotePersonalizacaoComprado = comprado;
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
            PacotePersonalizacaoComprado = comprado;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PacotePersonalizacaoComprado)));
        }

        async void ReanalizarCompras(object sender, RoutedEventArgs e)
        {
            await ComprasInApp.AnalisarCompras();
        }
    }
}
