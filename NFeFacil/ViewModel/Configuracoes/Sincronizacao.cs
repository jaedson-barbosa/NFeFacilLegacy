using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Networking;
using Windows.Networking.Connectivity;
using Windows.UI.Xaml.Media;
using Newtonsoft.Json;
using NFeFacil.Log;
using NFeFacil.Configuracoes;
using static NFeFacil.Configuracoes.ConfiguracoesSincronizacao;
using NFeFacil.Sincronizacao.Cliente;
using NFeFacil.Sincronizacao;
using NFeFacil.Sincronizacao.Pacotes;
using NFeFacil.View;

namespace NFeFacil.ViewModel.Configuracoes
{
    public sealed class Sincronizacao : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnProperyChanged(string propriedade)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propriedade));
        }

        private readonly ILog LogPopUp = new Popup();

        public bool IsCliente
        {
            get { return Tipo == TipoAppSincronizacao.Cliente; }
            set
            {
                Tipo = value ? TipoAppSincronizacao.Cliente : TipoAppSincronizacao.Servidor;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsServidor)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsCliente)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StatusCliente)));
            }
        }

        public bool IsServidor
        {
            get { return !IsCliente; }
        }

        public bool SincronizarDadoBase
        {
            get { return SincDadoBase; }
            set { SincDadoBase = value; }
        }

        public bool SincronizarNotaFiscal
        {
            get { return SincNotaFiscal; }
            set { SincNotaFiscal = value; }
        }

        public bool IniciarAutomaticamente
        {
            get { return InícioAutomático; }
            set
            {
                if (value != ServerRodando && value) IniciarServidor();
                InícioAutomático = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IniciarManualmente)));
            }
        }

        public bool IniciarManualmente
        {
            get { return !InícioAutomático; }
        }

        public bool ServerRodando
        {
            get { return Propriedades.Server.Rodando; }
        }

        public ImageSource QRGerado
        {
            get
            {
                return QRCode.GerarQR(JsonConvert.SerializeObject(new InfoEstabelecerConexao
                {
                    IP = NetworkInformation.GetHostNames().First(x => x.IPInformation != null && x.Type == HostNameType.Ipv4).ToString(),
                    SenhaTemporaria = SenhaTemporária = new Random().Next(1000, 10000)
                }), 1920, 1920);
            }
        }

        public string StatusCliente
        {
            get
            {
                if (!string.IsNullOrEmpty(IPServidor) && SenhaPermanente != default(int))
                    return "Servidor já cadastrado";
                else
                    return "Não há nenhum servidor cadastrado";
            }
        }

        public bool SincronizarAutomaticamente
        {
            get { return ConfiguracoesSincronizacao.SincronizarAutomaticamente; }
            set
            {
                if (value) RegistrarCliente.Registrar();
                else RegistrarCliente.Desrregistrar();
                ConfiguracoesSincronizacao.SincronizarAutomaticamente = value;
            }
        }

        public Sincronizacao()
        {
            GerarQRTemporárioCommand = new ComandoSemParametros(GerarQRTemporário, true);
            LerQRTemporárioCommand = new ComandoSemParametros(LerQRTemporário, true);
            IniciarServidorCommand = new ComandoSemParametros(IniciarServidor, true);
            SincronizarAgoraCommand = new ComandoSemParametros(SincronizarAgora, true);
        }

        public ICommand GerarQRTemporárioCommand { get; }
        public ICommand LerQRTemporárioCommand { get; }
        public ICommand IniciarServidorCommand { get; }
        public ICommand SincronizarAgoraCommand { get; }

        public double ValorMaximo { get; } = 30;
        public double ValorAtual { get; private set; } = 0;

        private bool mostrarQR;
        public bool MostrarQR
        {
            get => mostrarQR;
            set
            {
                mostrarQR = value;
                MostrarQRChanged(this, new MostrarQRChangeEventArgs
                {
                    DadoAtual = value
                });
            }
        }
        public event MostrarQRChangedDelegate MostrarQRChanged;
        public delegate void MostrarQRChangedDelegate(Sincronizacao sender, MostrarQRChangeEventArgs args);
        public class MostrarQRChangeEventArgs : EventArgs
        {
            public bool DadoAtual { get; set; }
        }

        public async void GerarQRTemporário()
        {
            Propriedades.Server.AbrirBrecha(TimeSpan.FromSeconds(ValorMaximo));
            OnProperyChanged(nameof(QRGerado));
            ValorAtual = 0;
            PropertyChanged(this, new PropertyChangedEventArgs("ValorAtual"));
            MostrarQR = true;
            await Task.Delay(1000);
            while (ValorAtual <= ValorMaximo)
            {
                ValorAtual += 0.05;
                PropertyChanged(this, new PropertyChangedEventArgs("ValorAtual"));
                await Task.Delay(50);
            }
            MostrarQR = false;
            await Task.Delay(1000);
        }

        public async void LerQRTemporário()
        {
            var str = await QRCode.DecodificarQRAsync();
            var resultado = JsonConvert.DeserializeObject<InfoEstabelecerConexao>(str);
            IPServidor = resultado.IP;
            var cliente = new ClienteBrechaSeguranca(LogPopUp);
            await cliente.EstabelecerConexao(resultado.SenhaTemporaria);
        }

        public async void IniciarServidor()
        {
            try
            {
                await Propriedades.Server.IniciarServer();
                OnProperyChanged(nameof(ServerRodando));
            }
            catch (COMException)
            {
                LogPopUp.Escrever(TitulosComuns.ErroSimples, "O servidor já está ativo.");
            }
            catch (Exception ex)
            {
                LogPopUp.Escrever(TitulosComuns.ErroCatastrófico, ex.StackTrace);
            }
        }

        public async void SincronizarAgora()
        {
            var gerenc = new GerenciadorCliente(LogPopUp);
            await gerenc.Sincronizar(DadosSincronizaveis.Tudo, false);
        }
    }
}
