using BaseGeral.Log;
using BaseGeral.View;
using System;
using System.IO;
using System.Reflection;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.Certificacao
{
    [DetalhePagina(Symbol.Permissions, "Certificação")]
    public sealed partial class ConfiguracoesServidorCertificacao : Page
    {
        public ConfiguracoesServidorCertificacao()
        {
            InitializeComponent();
        }

        void InstalarServidor(object sender, RoutedEventArgs e)
        {
            SalvarArquivoInterno("Servidor de certificação", "BaseGeral.Certificacao.LAN.ServidorCertificacao.zip");
        }

        void RepararProblemas(object sender, RoutedEventArgs e)
        {
            SalvarArquivoInterno("Gerenciador de Looopback", "BaseGeral.Certificacao.LAN.WindowsLoopbackManager.zip");
        }

        async void SalvarArquivoInterno(string nomeSugerido, string caminho)
        {
            var salvador = new FileSavePicker()
            {
                SuggestedFileName = nomeSugerido,
                DefaultFileExtension = ".zip"
            };
            salvador.FileTypeChoices.Add("Arquivo comprimido", new string[1] { ".zip" });
            var arquivo = await salvador.PickSaveFileAsync();
            if (arquivo != null)
            {
                using (var stream = await arquivo.OpenStreamForWriteAsync())
                {
                    var assembly = GetType().GetTypeInfo().Assembly;
                    var recurso = assembly.GetManifestResourceStream(caminho);
                    recurso.CopyTo(stream);
                }
                Popup.Current.Escrever(TitulosComuns.Sucesso, "Arquivo salvo com sucesso.\r\n" +
                    "Extraia os arquivos e inicie a instalação");
            }
        }
    }
}
