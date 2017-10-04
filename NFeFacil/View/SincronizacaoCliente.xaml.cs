using NFeFacil.Sincronizacao;
using NFeFacil.Sincronizacao.Pacotes;
using NFeFacil.ViewModel;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using static NFeFacil.Sincronizacao.ConfiguracoesSincronizacao;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class SincronizacaoCliente : Page
    {
        public SincronizacaoCliente()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            MainPage.Current.SeAtualizar(Symbol.Sync, "Sincronização");
        }

        async void LerQRTemporario(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                var str = await QRCode.DecodificarQRAsync();
                var partes = str.Split(':');
                var resultado = new InfoEstabelecerConexao
                {
                    IP = partes[0],
                    SenhaTemporaria = int.Parse(partes[1])
                };
                await EstabelecerConexaoAsync(resultado);
            }
            catch (Exception erro)
            {
                erro.ManipularErro();
            }
        }

        async void InserirDadosManualmente(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                var caixa = new CaixasDialogo.ConfigurarDadosConexao()
                {
                    DataContext = new InfoEstabelecerConexao()
                };
                if (await caixa.ShowAsync() == ContentDialogResult.Primary)
                {
                    await EstabelecerConexaoAsync((InfoEstabelecerConexao)caixa.DataContext);
                }
            }
            catch (Exception erro)
            {
                erro.ManipularErro();
            }
        }

        private async Task EstabelecerConexaoAsync(InfoEstabelecerConexao info)
        {
            IPServidor = info.IP;
            try
            {
                await new GerenciadorCliente().EstabelecerConexao(info.SenhaTemporaria);
            }
            catch (Exception ex)
            {
                ex.ManipularErro();
            }
        }

        async void SincronizarAgora(object sender, RoutedEventArgs e)
        {
            try
            {
                await new GerenciadorCliente().Sincronizar();
            }
            catch (Exception erro)
            {
                erro.ManipularErro();
            }
        }

        async void SincronizarTudo(object sender, RoutedEventArgs e)
        {
            try
            {
                await new GerenciadorCliente().SincronizarTudo();
            }
            catch (Exception erro)
            {
                erro.ManipularErro();
            }
        }
    }
}
