using NFeFacil.Sincronizacao;
using System;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

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

        async void DefinirBackground(object sender, TappedRoutedEventArgs e)
        {
            var brushAtual = MainPage.Current.FrameBackground as ImageBrush;
            if (IDBackgroung == default(Guid))
            {
                IDBackgroung = Guid.NewGuid();
            }
            var caixa = new DefinirImagem(IDBackgroung, brushAtual?.ImageSource);
            if (await caixa.ShowAsync() == ContentDialogResult.Primary)
            {
                var brush = new ImageBrush
                {
                    ImageSource = caixa.Imagem
                };
                MainPage.Current.FrameBackground = brush;
            }
        }

        internal static Guid IDBackgroung
        {
            get
            {
                var valor = ApplicationData.Current.LocalSettings.Values["IDBackgroung"];
                return valor != null ? (Guid)valor : Guid.Empty;
            }
            set => ApplicationData.Current.LocalSettings.Values["IDBackgroung"] = value;
        }
    }
}
