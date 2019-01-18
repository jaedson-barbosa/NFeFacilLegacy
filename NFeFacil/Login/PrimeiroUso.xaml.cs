using BaseGeral.View;
using NFeFacil.Sincronizacao;
using System;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.Login
{
    [DetalhePagina(Symbol.Emoji, "Bem-vindo")]
    public sealed partial class PrimeiroUso : Page
    {
        public PrimeiroUso()
        {
            InitializeComponent();
        }

        void Manualmente(object sender, RoutedEventArgs e) => MainPage.Current.Navegar<AdicionarEmitente>();
        void Sincronizar(object sender, RoutedEventArgs e) => MainPage.Current.Navegar<SincronizacaoCliente>();
        async void RestaurarBackup(object sender, RoutedEventArgs e)
        {
            var caixa = new FileOpenPicker();
            caixa.FileTypeFilter.Add(".db");
            var arq = await caixa.PickSingleFileAsync();
            if (arq != null)
            {
                var folder = Windows.Storage.ApplicationData.Current.LocalFolder;
                await arq.CopyAsync(folder,
                    "informacoes.db",
                    Windows.Storage.NameCollisionOption.ReplaceExisting);
                MainPage.Current.Navegar<Loading>(true);
            }
        }
    }
}
