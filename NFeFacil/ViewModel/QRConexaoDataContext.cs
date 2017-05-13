using BibliotecaCentral.Sincronizacao;
using BibliotecaCentral.Sincronizacao.Pacotes;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Networking;
using Windows.Networking.Connectivity;
using Windows.UI.Xaml.Media;

namespace NFeFacil.ViewModel
{
    public sealed class QRConexaoDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public InfoEstabelecerConexao Informacoes { get; }
        public ImageSource QRGerado { get; private set; }

        public double ValorMaximo { get; } = 120;
        public double ValorAtual { get; private set; } = 0;

        public QRConexaoDataContext()
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
            FecharBrechaSeguranca = new Comando(PararDeAceitarNovasConexoes, true);
            AbrirBrecha();
        }

        public ICommand FecharBrechaSeguranca { get; }

        private bool brechaAberta = false;
        public async void AbrirBrecha()
        {
            Propriedades.Server.AbrirBrecha(TimeSpan.FromSeconds(ValorMaximo));
            await Task.Delay(200);
            //A geração do QR é feita no método assíncrono para não paralisar a tela.
            QRGerado = QRCode.GerarQR(JsonConvert.SerializeObject(Informacoes), 1920, 1920);
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(QRGerado)));
            brechaAberta = true;
            while (ValorAtual <= ValorMaximo && brechaAberta)
            {
                ValorAtual += 0.1;
                PropertyChanged(this, new PropertyChangedEventArgs("ValorAtual"));
                await Task.Delay(100);
            }
            PararDeAceitarNovasConexoes();
        }

        private async void PararDeAceitarNovasConexoes()
        {
            if (brechaAberta)
            {
                Propriedades.Server.FecharBrecha();
                await Task.Delay(1000);
                brechaAberta = false;
            }
            Propriedades.Intercambio.Retornar();
        }
    }
}
