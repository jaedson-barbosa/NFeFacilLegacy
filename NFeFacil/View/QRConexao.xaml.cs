using NFeFacil.Sincronizacao;
using NFeFacil.Sincronizacao.Pacotes;
using NFeFacil.ViewModel;
using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Connectivity;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
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
                QRGerado = QRCode.GerarQR($"{Informacoes.IP}:{Informacoes.SenhaTemporaria}", 1920, 1920);

                Iniciar();
            }
            catch (Exception e)
            {
                e.ManipularErro();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            MainPage.Current.SeAtualizar(Symbol.View, "QR");
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
