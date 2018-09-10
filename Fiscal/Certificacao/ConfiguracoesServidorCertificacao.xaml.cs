using BaseGeral;
using BaseGeral.Log;
using BaseGeral.View;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace Fiscal.Certificacao
{
    [DetalhePagina(Symbol.Permissions, "Certificação")]
    public sealed partial class ConfiguracoesServidorCertificacao : Page
    {
        public ConfiguracoesServidorCertificacao()
        {
            InitializeComponent();
        }

        async void InstalarServidor(object sender, RoutedEventArgs e)
        {
            var log = Popup.Current;
            var pasta = ApplicationData.Current.LocalFolder;
            pasta = await pasta.CreateFolderAsync("ConjuntoA3", CreationCollisionOption.ReplaceExisting);
            var zip = await pasta.CreateFileAsync("ConjuntoA3.zip");
            using (Stream original = GetFileStream("Fiscal.Certificacao.LAN.ConjuntoA3.zip"),
                novo = await zip.OpenStreamForWriteAsync())
                original.CopyTo(novo);
            ZipFile.ExtractToDirectory(zip.Path, pasta.Path);

            var salvador = new FolderPicker()
            {
                SuggestedStartLocation = PickerLocationId.ComputerFolder,
                CommitButtonText = "Instalar"
            };
            salvador.FileTypeFilter.Add("*");
            var novaPasta = await salvador.PickSingleFolderAsync();
            foreach (var item in await pasta.GetFilesAsync()) 
                await item.CopyAsync(novaPasta, item.Name, NameCollisionOption.ReplaceExisting);

            log.Escrever(TitulosComuns.Sucesso, "Arquivo salvo com sucesso, agora execute-o para que o serviço de certificação esteja ativo, se quiser, pode salvar um atalho na área de trabalho ou registrar ele para iniciar automaticamente, se lembre que para registrar ele, é necessário que ele esteja aberto.\nO executável correto é o ServidorCertificacao.");
        }

        async void RegistrarInicioAutomatico(object sender, RoutedEventArgs e)
        {
            try
            {
                var log = Popup.Current;
                await new LAN.OperacoesServidor().RegistrarInicioAutomatico();
                log.Escrever(TitulosComuns.Sucesso, "Servidor de certificação registrado para iniciar automaticamente junto ao Windows.");
            }
            catch (Exception erro)
            {
                erro.ManipularErro();
            }
        }

        async void RepararProblemas(object sender, RoutedEventArgs e)
        {
            var salvador = new FileSavePicker()
            {
                SuggestedFileName = "Gerenciador de Looopback",
                DefaultFileExtension = ".zip"
            };
            salvador.FileTypeChoices.Add("Arquivo comprimido", new string[1] { ".zip" });
            var arquivo = await salvador.PickSaveFileAsync();
            if (arquivo != null)
            {
                using (var stream = await arquivo.OpenStreamForWriteAsync())
                {
                    var recurso = GetFileStream("Fiscal.Certificacao.LAN.WindowsLoopbackManager.zip");
                    recurso.CopyTo(stream);
                }
            }

            Popup.Current.Escrever(TitulosComuns.Sucesso, "Arquivo salvo com sucesso.\r\n" +
                "Extraia os arquivos e inicie a instalação");
        }

        async Task SalvarArquivo(string nomeSugerido, string save)
        {
            var salvador = new FileSavePicker()
            {
                SuggestedFileName = nomeSugerido,
                DefaultFileExtension = ".bat"
            };
            salvador.FileTypeChoices.Add("Arquivo em lotes do Windows", new string[1] { ".bat" });
            var arquivo = await salvador.PickSaveFileAsync();
            if (arquivo != null)
            {
                var stream = await arquivo.OpenStreamForWriteAsync();
                using (var escritor = new StreamWriter(stream))
                {
                    escritor.Write(save);
                    escritor.Flush();
                }
            }
        }

        Stream GetFileStream(string caminho)
        {
            var assembly = GetType().GetTypeInfo().Assembly;
            var recurso = assembly.GetManifestResourceStream(caminho);
            return recurso;
        }
    }
}
