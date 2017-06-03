using BibliotecaCentral.Log;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Serialization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.View.CaixasDialogo
{
    public sealed partial class ImportarCertificadoLAN : ContentDialog
    {
        const string ObterCertificados = "ObterCertificados";
        const string ObterCertificado = "ObterCertificado";
        ILog log = new Popup();

        public ImportarCertificadoLAN()
        {
            InitializeComponent();
        }

        private async void Localizar(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var cliente = new HttpClient())
                {
                    var resposta = await cliente.GetAsync(new Uri($"http://{txtIP.Text}:{txtPorta.Text}/{ObterCertificados}"));
                    using (var stream = await resposta.Content.ReadAsStreamAsync())
                    {
                        var conjunto = new XmlSerializer(typeof(Certificados)).Deserialize(stream) as Certificados;
                        lstCertificados.ItemsSource = new ObservableCollection<Certificados.CertificadoPrimitivo>(conjunto.Certificado);
                    }
                }
            }
            catch (Exception erro)
            {
                log.Escrever(TitulosComuns.ErroSimples, erro.Message);
            }
        }

        private async void Importar(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            try
            {
                var serial = (string)lstCertificados.SelectedValue;
                using (var cliente = new HttpClient())
                {
                    var resposta = await cliente.GetAsync(new Uri($"http://{txtIP.Text}:{txtPorta.Text}/{ObterCertificado}/{serial}"));
                    var bytes = await resposta.Content.ReadAsByteArrayAsync();
                    var cert = new X509Certificate2(bytes);
                    using (var loja = new X509Store())
                    {
                        loja.Open(OpenFlags.ReadWrite);
                        loja.Add(cert);
                    }
                }
            }
            catch (Exception erro)
            {
                args.Cancel = true;
                log.Escrever(TitulosComuns.ErroSimples, erro.Message);
            }
        }

        public sealed class Certificados
        {
            [XmlElement("Certificado")]
            public List<CertificadoPrimitivo> Certificado { get; set; }

            public struct CertificadoPrimitivo
            {
                public string Nome { get; set; }
                public string NumeroSerial { get; set; }
            }
        }
    }
}
