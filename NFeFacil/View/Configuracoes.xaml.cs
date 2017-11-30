using NFeFacil.Certificacao;
using NFeFacil.Sincronizacao;
using System;
using Windows.System.Profile;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class Configuracoes : Page
    {
        readonly ComprasInApp Compra = new ComprasInApp(ComprasInApp.Compras.Personalizacao);

        public Configuracoes()
        {
            InitializeComponent();
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
            get => ConfiguracoesPermanentes.SuprimirHorarioVerao;
            set => ConfiguracoesPermanentes.SuprimirHorarioVerao = value;
        }

        bool CalcularNumeroNFe
        {
            get => ConfiguracoesPermanentes.CalcularNumeroNFe;
            set => ConfiguracoesPermanentes.CalcularNumeroNFe = value;
        }

        async void UsarImagem(object sender, TappedRoutedEventArgs e)
        {
            if (await Compra.AnalisarCompra())
            {
                var brushAtual = MainPage.Current.ImagemBackground;
                if (ConfiguracoesPermanentes.IDBackgroung == default(Guid))
                {
                    ConfiguracoesPermanentes.IDBackgroung = Guid.NewGuid();
                }
                var caixa = new DefinirImagem(ConfiguracoesPermanentes.IDBackgroung, brushAtual);
                if (await caixa.ShowAsync() == ContentDialogResult.Primary)
                {
                    MainPage.Current.ImagemBackground = caixa.Imagem;
                    MainPage.Current.DefinirTipoBackground(TiposBackground.Imagem);
                }
            }
        }

        async void UsarCor(object sender, TappedRoutedEventArgs e)
        {
            if (await Compra.AnalisarCompra())
            {
                MainPage.Current.DefinirTipoBackground(TiposBackground.Cor);
            }
        }

        async void EscolherTransparencia(object sender, TappedRoutedEventArgs e)
        {
            if (await Compra.AnalisarCompra())
            {
                var caixa = new EscolherTransparencia(ConfiguracoesPermanentes.OpacidadeBackground);
                if (await caixa.ShowAsync() == ContentDialogResult.Primary)
                {
                    ConfiguracoesPermanentes.OpacidadeBackground = caixa.Opacidade;
                    MainPage.Current.DefinirOpacidadeBackground(caixa.Opacidade);
                }
            }
        }

        async void Resetar(object sender, TappedRoutedEventArgs e)
        {
            if (await Compra.AnalisarCompra())
            {
                using (var db = new AplicativoContext())
                {
                    if (ConfiguracoesPermanentes.IDBackgroung != default(Guid))
                    {
                        var img = db.Imagens.Find(ConfiguracoesPermanentes.IDBackgroung);
                        if (img?.Bytes != null)
                        {
                            img.Bytes = null;
                            db.Update(img);
                            db.SaveChanges();
                        }
                    }
                }
                ConfiguracoesPermanentes.OpacidadeBackground = 1;
                MainPage.Current.DefinirTipoBackground(TiposBackground.Padrao);
            }
        }
    }
}
