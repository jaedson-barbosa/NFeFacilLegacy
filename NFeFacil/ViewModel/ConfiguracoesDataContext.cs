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
using BibliotecaCentral.Log;
using BibliotecaCentral.Configuracoes;
using BibliotecaCentral.Sincronizacao.Cliente;
using BibliotecaCentral.Sincronizacao;
using BibliotecaCentral.Sincronizacao.Pacotes;
using NFeFacil.View;
using System.Collections.Generic;
using static BibliotecaCentral.Configuracoes.ConfiguracoesSincronizacao;
using BibliotecaCentral.Importacao;
using System.Text;

namespace NFeFacil.ViewModel
{
    public sealed class ConfiguracoesDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnProperyChanged(string propriedade)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propriedade));
        }

        private readonly ILog LogPopUp = new Popup();

        #region Sincronização

        public bool IsCliente
        {
            get => Tipo == TipoAppSincronizacao.Cliente;
            set
            {
                Tipo = value ? TipoAppSincronizacao.Cliente : TipoAppSincronizacao.Servidor;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsServidor)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsCliente)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StatusCliente)));
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
            get => InícioAutomático; set
            {
                if (value != ServerRodando && value) IniciarServidor();
                InícioAutomático = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IniciarManualmente)));
            }
        }

        public bool IniciarManualmente => !InícioAutomático;
        public bool ServerRodando => Propriedades.Server.Rodando;

        public InfoEstabelecerConexao Informacoes { get; private set; }
        public ImageSource QRGerado
        {
            get
            {
                try
                {
                    var hosts = NetworkInformation.GetHostNames();
                    if (hosts?.Count > 0)
                    {
                        Informacoes = new InfoEstabelecerConexao
                        {
                            IP = hosts.First(x => x.IPInformation != null && x.Type == HostNameType.Ipv4).ToString(),
                            SenhaTemporaria = SenhaTemporária = new Random().Next(1000, 10000)
                        };
                        OnProperyChanged(nameof(Informacoes));
                        return QRCode.GerarQR(JsonConvert.SerializeObject(Informacoes), 1920, 1920);
                    }
                    return null;
                }
                catch (Exception)
                {
                    return null;
                }
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
                if (value) RegistroClienteBackground.Registrar();
                else RegistroClienteBackground.Desrregistrar();
                ConfiguracoesSincronizacao.SincronizarAutomaticamente = value;
            }
        }

        public ConfiguracoesDataContext()
        {
            GerarQRTemporárioCommand = new Comando(GerarQRTemporário, true);
            LerQRTemporárioCommand = new Comando(LerQRTemporário, true);
            InserirDadosManualmenteCommand = new Comando(InserirDadosManualmente, true);
            IniciarServidorCommand = new Comando(IniciarServidor, true);
            SincronizarAgoraCommand = new Comando(SincronizarAgora, true);
            FecharBrechaSeguranca = new Comando(PararDeAceitarNovasConexoes, true);
            ImportarNotaFiscalCommand = new Comando(ImportarNotaFiscal, true);
            ImportarDadoBaseCommand = new Comando(ImportarDadoBase, true);
        }

        public ICommand GerarQRTemporárioCommand { get; }
        public ICommand LerQRTemporárioCommand { get; }
        public ICommand InserirDadosManualmenteCommand { get; }
        public ICommand IniciarServidorCommand { get; }
        public ICommand SincronizarAgoraCommand { get; }
        public ICommand FecharBrechaSeguranca { get; }

        public double ValorMaximo { get; } = 120;
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
        public delegate void MostrarQRChangedDelegate(ConfiguracoesDataContext sender, MostrarQRChangeEventArgs e);
        public class MostrarQRChangeEventArgs : EventArgs
        {
            public bool DadoAtual { get; set; }
        }

        private bool brechaAberta = false;
        public async void GerarQRTemporário()
        {
            Propriedades.Server.AbrirBrecha(TimeSpan.FromSeconds(ValorMaximo));
            OnProperyChanged(nameof(QRGerado));
            ValorAtual = 0;
            PropertyChanged(this, new PropertyChangedEventArgs("ValorAtual"));
            await Task.Delay(500);
            MostrarQR = true;
            await Task.Delay(1000);
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
                MostrarQR = false;
                await Task.Delay(1000);
                brechaAberta = false;
            }
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
                await EstabelecerConexaoAsync(sender.DataContext as InfoEstabelecerConexao);
            };
            await caixa.ShowAsync();
        }

        private async Task EstabelecerConexaoAsync(InfoEstabelecerConexao info)
        {
            IPServidor = info.IP;
            var cliente = new ClienteBrechaSeguranca(LogPopUp);
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

        #endregion

        #region Certificação

        private BibliotecaCentral.Repositorio.Certificados repo = new BibliotecaCentral.Repositorio.Certificados();

        public IEnumerable<string> Certificados => from c in repo.Registro
                                                   orderby c.Subject
                                                   select c.Subject;

        public string CertificadoEscolhido
        {
            get => repo.SerialEscolhido != null ? repo.Registro.First(x => x.SerialNumber == repo.SerialEscolhido).Subject : null;
            set => repo.SerialEscolhido = repo.Registro.First(x => x.Subject == value).SerialNumber;
        }

        #endregion

        #region Importação

        public ICommand ImportarNotaFiscalCommand { get; }
        public ICommand ImportarDadoBaseCommand { get; }

        private async void ImportarNotaFiscal()
        {
            var resultado = await new ImportarNotaFiscal().ImportarAsync();
            if (resultado.Analise == ResumoRelatorioImportacao.Sucesso)
            {
                LogPopUp.Escrever(TitulosComuns.Sucesso, "As notas fiscais foram importadas com sucesso.");
            }
            else
            {
                StringBuilder stringErros = new StringBuilder();
                stringErros.AppendLine("As seguintes notas fiscais não foram reconhecidas por terem a tag raiz diferente de nfeProc e de NFe.");
                resultado.Erros.ForEach(x => stringErros.AppendLine($"Nome arquivo: {x.NomeArquivo}; Tag raiz: Encontrada: {x.TagRaiz}"));
                LogPopUp.Escrever(TitulosComuns.ErroSimples, stringErros.ToString());
            }
        }

        private async void ImportarDadoBase()
        {
            var resultado = await new ImportarDadoBase(TipoBásicoSelecionado).ImportarAsync();
            if (resultado.Analise == ResumoRelatorioImportacao.Sucesso)
            {
                LogPopUp.Escrever(TitulosComuns.Sucesso, "As informações base foram importadas com sucesso.");
            }
            else
            {
                StringBuilder stringErros = new StringBuilder();
                stringErros.AppendLine("Os seguintes dados base não foram reconhecidos por terem a tag raiz diferente do esperado.");
                resultado.Erros.ForEach(x => stringErros.AppendLine($"Nome arquivo: {x.NomeArquivo}; Tag raiz encontrada: {x.TagRaiz}; Tags raiz esperadas: {x.TagsEsperadas[0]} ou {x.TagsEsperadas[1]}"));
                LogPopUp.Escrever(TitulosComuns.ErroSimples, stringErros.ToString());
            }
        }

        public IEnumerable<TiposDadoBasico> TiposBásicos => BibliotecaCentral.Extensoes.ObterItens<TiposDadoBasico>();
        public TiposDadoBasico TipoBásicoSelecionado { get; set; }

        #endregion
    }
}
