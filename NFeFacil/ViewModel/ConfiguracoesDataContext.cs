using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using BibliotecaCentral.Log;
using BibliotecaCentral.Certificacao;
using System.Collections.Generic;
using BibliotecaCentral.Importacao;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Collections.ObjectModel;
using BibliotecaCentral;
using BibliotecaCentral.ItensBD;

namespace NFeFacil.ViewModel
{
    public sealed class ConfiguracoesDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnProperyChanged(string propriedade)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propriedade));
        }

        public ConfiguracoesDataContext()
        {
            ImportarNotaFiscalCommand = new Comando(ImportarNotaFiscal, true);
            ImportarDadoBaseCommand = new Comando(ImportarDadoBase, true);
            CertificadosRepositorio = repo.ObterRegistroRepositorio().GerarObs();
            CertificadosBanco = repo.ObterRegistroBanco().GerarObs();
        }

        private readonly ILog LogPopUp = new Popup();

        #region Certificação

        private Certificados repo = new Certificados();
        private ConfiguracoesCertificacao config = new ConfiguracoesCertificacao();

        public ObservableCollection<X509Certificate2> CertificadosRepositorio { get; private set; }
        public ObservableCollection<Certificado> CertificadosBanco { get; private set; }

        public bool UsarRepositorioWindows
        {
            get => config.FonteEscolhida == FonteCertificacao.RepositorioWindows;
            set
            {
                config.FonteEscolhida = value ? FonteCertificacao.RepositorioWindows : FonteCertificacao.PastaApp;
                OnProperyChanged(nameof(UsarRepositorioWindows));
            }
        }

        public bool UsarPastaApp
        {
            get => !UsarRepositorioWindows;
            set
            {
                UsarRepositorioWindows = !value;
                OnProperyChanged(nameof(UsarPastaApp));
            }
        }

        public string CertificadoEscolhido
        {
            get => config.CertificadoEscolhido;
            set => config.CertificadoEscolhido = value;
        }

        public ICommand ImportarCertificado => new Comando(async () => await new ImportarCertificado().ImportarAsync());

        public ICommand ImportarCertificadoRemoto => new Comando(() =>
        {

        });

        #endregion

        #region Importação

        public ICommand ImportarNotaFiscalCommand { get; }
        public ICommand ImportarDadoBaseCommand { get; }

        private async void ImportarNotaFiscal()
        {
            var resultado = await new ImportarNotaFiscal().ImportarAsync();
            if (resultado.Count == 0)
            {
                LogPopUp.Escrever(TitulosComuns.Sucesso, "As notas fiscais foram importadas com sucesso.");
            }
            else
            {
                StringBuilder stringErros = new StringBuilder();
                stringErros.AppendLine("As seguintes notas fiscais não foram reconhecidas por terem a tag raiz diferente de nfeProc e de NFe.");
                resultado.ForEach(y =>
                {
                    if (y is XmlNaoReconhecido x)
                    {
                        stringErros.AppendLine($"Nome arquivo: {x.NomeArquivo}; Tag raiz: Encontrada: {x.TagRaiz}");
                    }
                    else
                    {
                        stringErros.AppendLine($"Mensagem erro: {y.Message}.");
                    }
                });
                LogPopUp.Escrever(TitulosComuns.ErroSimples, stringErros.ToString());
            }
        }

        private async void ImportarDadoBase()
        {
            var resultado = await new ImportarDadoBase(TipoBásicoSelecionado).ImportarAsync();
            if (resultado.Count == 0)
            {
                LogPopUp.Escrever(TitulosComuns.Sucesso, "As informações base foram importadas com sucesso.");
            }
            else
            {
                StringBuilder stringErros = new StringBuilder();
                stringErros.AppendLine("Os seguintes dados base não foram reconhecidos por terem a tag raiz diferente do esperado.");
                resultado.ForEach(y =>
                {
                    var x = y as XmlNaoReconhecido;
                    stringErros.AppendLine($"Nome arquivo: {x.NomeArquivo}; Tag raiz encontrada: {x.TagRaiz}; Tags raiz esperadas: {x.TagsEsperadas[0]} ou {x.TagsEsperadas[1]}");
                });
                LogPopUp.Escrever(TitulosComuns.ErroSimples, stringErros.ToString());
            }
        }

        public IEnumerable<TiposDadoBasico> TiposBásicos => BibliotecaCentral.Extensoes.ObterItens<TiposDadoBasico>();
        public TiposDadoBasico TipoBásicoSelecionado { get; set; }

        #endregion
    }
}
