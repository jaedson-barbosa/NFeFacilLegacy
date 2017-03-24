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
using NFeFacil.NavegacaoUI;
using NFeFacil.Sincronizacao.Cliente;
using NFeFacil.Sincronizacao;
using NFeFacil.Sincronizacao.Pacotes;

namespace NFeFacil.ViewModel.Configurações
{
    public sealed class Sincronizacao : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnProperyChanged(string propriedade)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propriedade));
        }

        private InfoEstabelecerConexao Pacote;
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
                if (Pacote == null) return null;
                return QRCode.GerarQR(JsonConvert.SerializeObject(Pacote), (int)View.GridQR.ActualWidth, (int)View.GridQR.ActualHeight);
            }
        }

        public double Raio
        {
            get
            {
                double tamanho;
                if (View.GridCarregamento.ActualWidth < View.GridCarregamento.ActualHeight)
                    tamanho = View.GridCarregamento.ActualWidth / 2;
                else
                    tamanho = View.GridCarregamento.ActualHeight / 2;
                if (tamanho == 0) return 0;
                else if (tamanho > 200) return 180;
                else return tamanho - Thickness;
            }
        }

        public double Thickness { get { return 16; } }

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

        private View.Configuracoes View;
        public Sincronizacao(View.Configuracoes config)
        {
            View = config;
            GerarQRTemporárioCommand = new ComandoSemParametros(GerarQRTemporário, true);
            LerQRTemporárioCommand = new ComandoSemParametros(LerQRTemporário, true);
            IniciarServidorCommand = new ComandoSemParametros(IniciarServidor, true);
            SincronizarAgoraCommand = new ComandoSemParametros(SincronizarAgora, true);
        }

        public ICommand GerarQRTemporárioCommand { get; }
        public ICommand LerQRTemporárioCommand { get; }
        public ICommand IniciarServidorCommand { get; }
        public ICommand SincronizarAgoraCommand { get; }

        public async void GerarQRTemporário()
        {
            await View.MostrarQRTemporario();
            var senhaTemp = new Random().Next(1000, 10000);
            SenhaTemporária = senhaTemp;
            var hosts = from host in NetworkInformation.GetHostNames()
                        where host.IPInformation != null && host.Type == HostNameType.Ipv4
                        select host;
            Pacote = new InfoEstabelecerConexao
            {
                IP = hosts.First().ToString(),
                SenhaTemporaria = senhaTemp
            };
            Propriedades.Server.AbrirBrecha(TimeSpan.FromSeconds(View.Carregamento.MaxValue));
            OnProperyChanged(nameof(QRGerado));
            if (View.Carregamento.ActualValue > 0) View.Carregamento.ActualValue = 0;
            var tempoRestante = View.Carregamento.MaxValue;
            while (tempoRestante >= 0)
            {
                View.Carregamento.ActualValue++;
                View.Carregamento.Elemento = tempoRestante--.ToString();
                await Task.Delay(1000);
            }
            View.OcultarQRTemporario();
            Pacote = null;
            OnProperyChanged(nameof(QRGerado));
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
