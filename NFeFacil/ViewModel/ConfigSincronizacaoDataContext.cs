using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using BibliotecaCentral.Log;
using BibliotecaCentral.Sincronizacao;
using BibliotecaCentral.Sincronizacao.Pacotes;
using static BibliotecaCentral.Sincronizacao.ConfiguracoesSincronizacao;

namespace NFeFacil.ViewModel
{
    public sealed class ConfigSincronizacaoDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnProperyChanged(string propriedade)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propriedade));
        }

        private readonly ILog LogPopUp = new Popup();

        public bool IsCliente
        {
            get => Tipo == TipoAppSincronizacao.Cliente;
            set
            {
                Tipo = value ? TipoAppSincronizacao.Cliente : TipoAppSincronizacao.Servidor;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsServidor)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsCliente)));
            }
        }
        public bool IsServidor => !IsCliente;

        public bool SincronizarDadoBase
        {
            get => SincDadoBase;
            set => SincDadoBase = value;
        }

        public bool SincronizarNotaFiscal
        {
            get => SincNotaFiscal;
            set => SincNotaFiscal = value;
        }

        public bool IniciarAutomaticamente
        {
            get => InícioAutomático;
            set => InícioAutomático = value;
        }

        public bool ServerRodando => Propriedades.Server.Rodando;

        public bool SincronizarAutomaticamente
        {
            get { return ConfiguracoesSincronizacao.SincronizarAutomaticamente; }
            set
            {
                if (value) RegistroClienteBackground.Registrar();
                else RegistroClienteBackground.Desrregistrar();
                ConfiguracoesSincronizacao.SincronizarAutomaticamente = value;
            }
        }

        public ConfigSincronizacaoDataContext()
        {
            ExibirQRCommand = new Comando(ExibirQR, true);
            LerQRTemporárioCommand = new Comando(LerQRTemporário, true);
            InserirDadosManualmenteCommand = new Comando(InserirDadosManualmente, true);
            IniciarServidorCommand = new Comando(IniciarServidor, true);
            SincronizarAgoraCommand = new Comando(SincronizarAgora, true);
            SincronizarTudoCommand = new Comando(SincronizarTudo, true);
        }

        public ICommand ExibirQRCommand { get; }
        public ICommand LerQRTemporárioCommand { get; }
        public ICommand InserirDadosManualmenteCommand { get; }
        public ICommand IniciarServidorCommand { get; }
        public ICommand SincronizarAgoraCommand { get; }
        public ICommand FecharBrechaSeguranca { get; }
        public ICommand SincronizarTudoCommand { get; }

        private async void ExibirQR()
        {
            await Propriedades.Intercambio.AbrirFunçaoAsync(typeof(View.QRConexao));
        }

        public async void LerQRTemporário()
        {
            var str = await QRCode.DecodificarQRAsync();
            var resultado = JsonConvert.DeserializeObject<InfoEstabelecerConexao>(str);
            await EstabelecerConexaoAsync(resultado);
        }

        private async void InserirDadosManualmente()
        {
            var caixa = new View.CaixasDialogo.ConfigurarDadosConexao()
            {
                DataContext = new InfoEstabelecerConexao()
            };
            caixa.PrimaryButtonClick += async (sender, e) =>
            {
                await EstabelecerConexaoAsync((InfoEstabelecerConexao)sender.DataContext);
            };
            await caixa.ShowAsync();
        }

        private async Task EstabelecerConexaoAsync(InfoEstabelecerConexao info)
        {
            IPServidor = info.IP;
            var cliente = new GerenciadorCliente(LogPopUp);
            await cliente.EstabelecerConexao(info.SenhaTemporaria);
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

        private async void SincronizarTudo()
        {
            var gerenc = new GerenciadorCliente(LogPopUp);
            await gerenc.SincronizarTudo(DadosSincronizaveis.Tudo);
        }
    }
}
