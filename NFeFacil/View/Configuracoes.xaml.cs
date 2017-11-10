using NFeFacil.Sincronizacao;
using System;
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
        public Configuracoes()
        {
            this.InitializeComponent();
        }

        bool Servidor
        {
            get => ConfiguracoesSincronizacao.Tipo == TipoAppSincronizacao.Servidor;
            set => ConfiguracoesSincronizacao.Tipo = value ? TipoAppSincronizacao.Servidor : TipoAppSincronizacao.Cliente;
        }

        bool Cliente => !Servidor;

        bool DesconsiderarHorarioVerao
        {
            get => ConfiguracoesPermanentes.SuprimirHorarioVerao;
            set => ConfiguracoesPermanentes.SuprimirHorarioVerao = value;
        }

        async void UsarImagem(object sender, TappedRoutedEventArgs e)
        {
            //StoreContext storeContext = StoreContext.GetDefault();
            //var resultadoAquisicao = await storeContext.RequestPurchaseAsync("9P70MWLRCS54");
            //string[] productKinds = new string[] { "Consumable", "Durable", "UnmanagedConsumable" };
            //List<String> filterList = new List<string>(productKinds);
            //StoreProductQueryResult addOns = await storeContext.GetAssociatedStoreProductsAsync(filterList);
            //var produtos = addOns.Products.ToDictionary(x => x.Key, y => y);
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

        private void UsarCor(object sender, TappedRoutedEventArgs e)
        {
            MainPage.Current.DefinirTipoBackground(TiposBackground.Cor);
        }

        async void EscolherTransparencia(object sender, TappedRoutedEventArgs e)
        {
            var caixa = new EscolherTransparencia(ConfiguracoesPermanentes.OpacidadeBackground);
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                ConfiguracoesPermanentes.OpacidadeBackground = caixa.Opacidade;
                MainPage.Current.DefinirOpacidadeBackground(caixa.Opacidade);
            }
        }

        private void Resetar(object sender, TappedRoutedEventArgs e)
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
