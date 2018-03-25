using BaseGeral.Sincronizacao.Pacotes;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using OptimizedZXing;
using static BaseGeral.Sincronizacao.ConfiguracoesSincronizacao;
using BaseGeral.Sincronizacao;
using BaseGeral;
using BaseGeral.View;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.Sincronizacao
{
    [DetalhePagina("\uE975", "Sincronização")]
    public sealed partial class SincronizacaoCliente : Page
    {
        public SincronizacaoCliente()
        {
            InitializeComponent();
        }

        async void LerQRTemporario(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                var resposta = await new MobileBarcodeScanner(Window.Current.Dispatcher, MainPage.Current.Frame)
                {
                    TopText = "Coloque a câmera em frente ao código QR",
                    BottomText = "A câmera irá lê-lo automaticamente"
                }.Scan(new MobileBarcodeScanningOptions(BarcodeFormat.QR_CODE));
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
