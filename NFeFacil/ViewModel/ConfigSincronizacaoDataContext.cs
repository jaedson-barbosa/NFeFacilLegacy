using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Input;
using BibliotecaCentral.Log;
using BibliotecaCentral.Sincronizacao;
using BibliotecaCentral.Sincronizacao.Pacotes;
using static BibliotecaCentral.Sincronizacao.ConfiguracoesSincronizacao;
using Windows.UI.Xaml.Controls;

namespace NFeFacil.ViewModel
{
    public sealed class ConfigSincronizacaoDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

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

        public bool ServerRodando => GerenciadorServidor.Current.Rodando;

        public bool SincronizarAutomaticamente
        {
            get => ConfiguracoesSincronizacao.SincronizarAutomaticamente;
            set
            {
                try
                {
                    if (value) RegistroClienteBackground.Registrar();
                    else RegistroClienteBackground.Desrregistrar();
                    ConfiguracoesSincronizacao.SincronizarAutomaticamente = value;
                }
                catch (Exception e)
                {
                    e.ManipularErro();
                }
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

        private void ExibirQR()
        {
            MainPage.Current.AbrirFunçao(typeof(View.QRConexao));
        }

        public async void LerQRTemporário()
        {
            try
            {
                var str = await QRCode.DecodificarQRAsync();
                var partes = str.Split(':');
                var resultado = new InfoEstabelecerConexao
                {
                    IP = partes[0],
                    SenhaTemporaria = int.Parse(partes[1])
                };
                await EstabelecerConexaoAsync(resultado);
            }
            catch (Exception e)
            {
                e.ManipularErro();
            }
        }

        private async void InserirDadosManualmente()
        {
            try
            {
                var caixa = new View.CaixasDialogo.ConfigurarDadosConexao()
                {
                    DataContext = new InfoEstabelecerConexao()
                };
                if (await caixa.ShowAsync() == ContentDialogResult.Primary)
                {
                    await EstabelecerConexaoAsync((InfoEstabelecerConexao)caixa.DataContext);
                }
            }
            catch (Exception e)
            {
                e.ManipularErro();
            }
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
                await GerenciadorServidor.Current.IniciarServer();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ServerRodando)));
            }
            catch (COMException)
            {
                LogPopUp.Escrever(TitulosComuns.ErroSimples, "O servidor já está ativo.");
            }
            catch (Exception ex)
            {
                ex.ManipularErro();
            }
        }

        public async void SincronizarAgora()
        {
            try
            {
                var gerenc = new GerenciadorCliente(LogPopUp);
                await gerenc.Sincronizar(DadosSincronizaveis.Tudo, false);
            }
            catch (Exception e)
            {
                e.ManipularErro();
            }
        }

        private async void SincronizarTudo()
        {
            try
            {
                var gerenc = new GerenciadorCliente(LogPopUp);
                await gerenc.SincronizarTudo(DadosSincronizaveis.Tudo);
            }
            catch (Exception e)
            {
                e.ManipularErro();
            }
        }
    }
}
