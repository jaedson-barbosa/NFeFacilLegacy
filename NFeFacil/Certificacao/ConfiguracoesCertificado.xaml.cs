using Comum.Primitivos;
using NFeFacil.Log;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Windows.Storage.Pickers;
using Windows.System.Profile;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.Certificacao
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class ConfiguracoesCertificado : Page
    {
        public ConfiguracoesCertificado()
        {
            InitializeComponent();
            ListaCertificados = new ObservableCollection<CertificadoExibicao>();
            AttLista();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            MainPage.Current.SeAtualizar(Symbol.Permissions, "Certificação");
        }

        ObservableCollection<CertificadoExibicao> ListaCertificados { get; }
        bool InstalacaoLiberada => AnalyticsInfo.VersionInfo.DeviceFamily.Contains("Desktop");

        async void ImportarCertificado(object sender, TappedRoutedEventArgs e)
        {
            if (await new ImportarCertificado().ImportarEAdicionarAsync())
            {
                AttLista();
            }
        }

        async void ConectarServidor(object sender, TappedRoutedEventArgs e)
        {
            await LAN.InformacoesConexao.Cadastrar();
        }

        void EsquecerServidor(object sender, TappedRoutedEventArgs e)
        {
            LAN.InformacoesConexao.Esquecer();
        }

        async void InstalarServidor(object sender, TappedRoutedEventArgs e)
        {
            var salvador = new FileSavePicker()
            {
                SuggestedFileName = "Servidor de certificação",
                DefaultFileExtension = ".zip"
            };
            salvador.FileTypeChoices.Add("Arquivo comprimido", new string[1] { ".zip" });
            var arquivo = await salvador.PickSaveFileAsync();
            if (arquivo != null)
            {
                using (var stream = await arquivo.OpenStreamForWriteAsync())
                {
                    var recurso = Extensoes.Retornar(this, $"NFeFacil.Certificacao.LAN.ServidorCertificacao.zip");
                    recurso.CopyTo(stream);
                }
                Popup.Current.Escrever(TitulosComuns.Sucesso, "Arquivo salvo com sucesso.\r\n" +
                    "Extraia os arquivos e inicie o repositório remoto com o Iniciar.bat");
            }
        }

        private void RemoverCertificado(object sender, RoutedEventArgs e)
        {
            var contexto = ((FrameworkElement)sender).DataContext;
            var serial = ((CertificadoExibicao)contexto).SerialNumber;
            using (var loja = new X509Store(StoreName.My, StoreLocation.CurrentUser))
            {
                loja.Open(OpenFlags.ReadWrite);
                var cert = loja.Certificates.Find(X509FindType.FindBySerialNumber, serial, false)[0];
                loja.Remove(cert);
            }
            AttLista();
        }

        async void AttLista()
        {
            try
            {
                ListaCertificados.Clear();
                foreach (var item in await Certificados.ObterCertificadosAsync(OrigemCertificado.Importado))
                {
                    ListaCertificados.Add(item);
                }
            }
            catch (Exception e)
            {
                e.ManipularErro();
            }
        }
    }
}
