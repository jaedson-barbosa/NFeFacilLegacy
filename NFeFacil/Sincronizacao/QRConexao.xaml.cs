using NFeFacil.Sincronizacao.Pacotes;
using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Connectivity;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZXing.Mobile;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.Sincronizacao
{
    [DetalhePagina(Symbol.View, "QR")]
    public sealed partial class QRConexao : Page
    {
        readonly InfoEstabelecerConexao Informacoes;
        readonly ImageSource QRGerado;
        bool brechaAberta = false;

        public QRConexao()
        {
            InitializeComponent();

            try
            {
                var hosts = NetworkInformation.GetHostNames();
                if (hosts.Count > 0)
                {
                    Informacoes = new InfoEstabelecerConexao
                    {
                        IP = hosts.First(x => x.IPInformation != null && x.Type == HostNameType.Ipv4).ToString(),
                        SenhaTemporaria = ConfiguracoesSincronizacao.SenhaTemporária = new Random().Next(1000, 10000)
                    };
                }
                else
                {
                    Informacoes = new InfoEstabelecerConexao();
                }
                GerenciadorServidor.Current.AbrirBrecha(TimeSpan.FromSeconds(60));

                QRGerado = new BarcodeWriter
                {
                    Format = ZXing.BarcodeFormat.QR_CODE,
                    Options = new ZXing.Common.EncodingOptions
                    {
                        Width = 1920,
                        Height = 1920,
                        Margin = 0
                    }
                }.Write($"{Informacoes.IP}:{Informacoes.SenhaTemporaria}");

                Iniciar();
            }
            catch (Exception e)
            {
                e.ManipularErro();
            }
        }

        protected override async void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            await PararDeAceitarNovasConexoes();
        }

        async void Iniciar()
        {
            await Task.Delay(100);
            try
            {
                brechaAberta = true;
                teste.Begin();
                await Task.Delay(60000);
                await PararDeAceitarNovasConexoes();
                MainPage.Current.Retornar();
            }
            catch (Exception e)
            {
                e.ManipularErro();
            }
        }

        private async Task PararDeAceitarNovasConexoes()
        {
            if (brechaAberta)
            {
                GerenciadorServidor.Current.FecharBrecha();
                await Task.Delay(1000);
                brechaAberta = false;
            }
        }
    }
}
