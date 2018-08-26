using BaseGeral.Certificacao;
using System;
using System.Collections.ObjectModel;
using System.Security.Cryptography.X509Certificates;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using BaseGeral;
using BaseGeral.View;
using Fiscal.Certificacao.LAN.Primitivos;
using Fiscal.Certificacao;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Fiscal.Certificacao
{
    [DetalhePagina(Symbol.Permissions, "Certificação")]
    public sealed partial class ConfiguracoesCertificadoImportado : Page
    {
        ObservableCollection<CertificadoExibicao> ListaCertificados { get; set; }

        public ConfiguracoesCertificadoImportado()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var certs = await Certificados.ObterCertificadosAsync(OrigemCertificado.Importado);
            ListaCertificados = certs.GerarObs();
        }

        async void ImportarCertificado(object sender, RoutedEventArgs e)
        {
            FileOpenPicker abrir = new FileOpenPicker
            {
                SuggestedStartLocation = PickerLocationId.Downloads,
            };
            abrir.FileTypeFilter.Add(".pfx");
            var arq = await abrir.PickSingleFileAsync();
            if (arq != null)
            {
                var pasta = ApplicationData.Current.TemporaryFolder;
                var novoArq = await arq.CopyAsync(pasta, arq.Name, NameCollisionOption.ReplaceExisting);
                var entrada = new DefinirSenhaCertificado();
                if (await entrada.ShowAsync() == ContentDialogResult.Primary)
                {
                    var senha = entrada.Senha;
                    X509Certificate2 cert;
                    if (string.IsNullOrEmpty(senha))
                    {
                        cert = new X509Certificate2(novoArq.Path);
                    }
                    else
                    {
                        cert = new X509Certificate2(novoArq.Path, senha, X509KeyStorageFlags.PersistKeySet);
                    }
                    using (var loja = new X509Store())
                    {
                        loja.Open(OpenFlags.ReadWrite);
                        loja.Add(cert);
                    }
                    ListaCertificados.Add(new CertificadoExibicao()
                    {
                        Subject = cert.Subject,
                        SerialNumber = cert.SerialNumber
                    });
                }
            }
        }

        private void RemoverCertificado(object sender, RoutedEventArgs e)
        {
            var contexto = ((FrameworkElement)sender).DataContext;
            var item = (CertificadoExibicao)contexto;
            using (var loja = new X509Store(StoreName.My, StoreLocation.CurrentUser))
            {
                loja.Open(OpenFlags.ReadWrite);
                var cert = loja.Certificates.Find(X509FindType.FindBySerialNumber, item.SerialNumber, false)[0];
                loja.Remove(cert);
            }
            ListaCertificados.Remove(item);
        }
    }
}
