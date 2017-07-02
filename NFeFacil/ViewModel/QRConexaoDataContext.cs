using BibliotecaCentral.Sincronizacao;
using BibliotecaCentral.Sincronizacao.Pacotes;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Connectivity;
using Windows.UI.Xaml.Media;

namespace NFeFacil.ViewModel
{
    public sealed class QRConexaoDataContext : INotifyPropertyChanged, IValida
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public InfoEstabelecerConexao Informacoes { get; }
        public ImageSource QRGerado { get; private set; }

        public double ValorMaximo { get; } = 120;
        public double ValorAtual { get; private set; } = 0;

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
                GerenciadorServidor.Current.AbrirBrecha(TimeSpan.FromSeconds(ValorMaximo));
                await Task.Delay(200);
                //A geração do QR é feita no método assíncrono para não paralisar a tela.
                QRGerado = QRCode.GerarQR($"{Informacoes.IP}:{Informacoes.SenhaTemporaria}", 1920, 1920);
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(QRGerado)));
                brechaAberta = true;
                while (ValorAtual <= ValorMaximo && brechaAberta)
                {
                    ValorAtual += 0.1;
                    PropertyChanged(this, new PropertyChangedEventArgs("ValorAtual"));
                    await Task.Delay(100);
                }
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
