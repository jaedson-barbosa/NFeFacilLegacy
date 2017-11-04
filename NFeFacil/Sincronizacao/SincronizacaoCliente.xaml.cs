using NFeFacil.Log;
using NFeFacil.Sincronizacao.Pacotes;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using ZXing.Mobile;
using static NFeFacil.Sincronizacao.ConfiguracoesSincronizacao;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.Sincronizacao
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
            MainPage.Current.SeAtualizar("\uE975", "Sincronização");
        }

        async void LerQRTemporario(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                var resposta = await new MobileBarcodeScanner
                {
                    UseCustomOverlay = false,
                    TopText = "Coloque a câmera em frente ao código QR",
                    BottomText = "A câmera irá lê-lo automaticamente"
                }.Scan();
                var str = resposta.Text;

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
                var caixa = new ConfigurarDadosConexao();
                if (await caixa.ShowAsync() == ContentDialogResult.Primary)
                {
                    await EstabelecerConexaoAsync(new InfoEstabelecerConexao()
                    {
                        IP = caixa.IP,
                        SenhaTemporaria = caixa.SenhaTemporaria
                    });
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
