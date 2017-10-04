using NFeFacil.Sincronizacao;
using NFeFacil.Sincronizacao.Pacotes;
using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Connectivity;
using Windows.UI.Xaml.Media;

namespace NFeFacil.ViewModel
{
    public sealed class QRConexaoDataContext : IValida
    {
        public InfoEstabelecerConexao Informacoes { get; }
        public ImageSource QRGerado { get; }

        public QRConexaoDataContext()
        {
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

                AbrirBrecha();
            }
            catch (Exception e)
            {
                e.ManipularErro();
            }
        }

        private bool brechaAberta = false;
        public async void AbrirBrecha()
        {
            try
            {
                brechaAberta = true;
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

        public async Task<bool> Verificar()
        {
            if (brechaAberta)
            {
                await PararDeAceitarNovasConexoes();
            }
            return true;
        }
    }
}
