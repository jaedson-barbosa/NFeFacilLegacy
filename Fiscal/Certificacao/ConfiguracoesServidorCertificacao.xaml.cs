using BaseGeral.Log;
using BaseGeral.View;
using System;
using System.IO;
using System.IO.Compression;
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
            var caixaSenha = new SenhaMicrosoft();
            if (await caixaSenha.ShowAsync() == ContentDialogResult.Secondary)
                return;
            var senha = caixaSenha.Senha;

            var log = Popup.Current;
            var pasta = ApplicationData.Current.LocalFolder;
            var pastas = await pasta.GetFoldersAsync();
            if (pastas.Count > 0)
            {
                var msg = new MessageDialog("O servidor já foi instalado. Se esta é uma atualização, certifique-se que o servidor já tenha sido desinstalado. O servidor já foi desinstalado?");
                msg.Commands.Add(new UICommand("Sim"));
                msg.Commands.Add(new UICommand("Não"));
                var rst = await msg.ShowAsync();
                if (rst.Label == "Não") return;
            }
            pasta = await pasta.CreateFolderAsync("ConexaoA3", CreationCollisionOption.ReplaceExisting);
            var zip = await pasta.CreateFileAsync("ConexaoA3.zip");
            using (Stream original = GetFileStream("Fiscal.Certificacao.LAN.ConexaoA3.zip"),
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

            string file;
            var stream = GetFileStream("Fiscal.Certificacao.LAN.RotinaInstalacao.bat");
            using (var leitor = new StreamReader(stream))
                file = leitor.ReadToEnd();
            file = string.Format(file, novaPasta.Path, senha);
            await SalvarArquivo("Rotina de instalação", file);
            log.Escrever(TitulosComuns.Sucesso, "Arquivo salvo com sucesso, agora execute-o para que o serviço seja instalado.");
        }

        async void DesinstalarServidor(object sender, RoutedEventArgs e)
        {
            var log = Popup.Current;
            var pasta = ApplicationData.Current.LocalFolder;
            var pastas = await pasta.GetFoldersAsync();
            if (pastas.Count > 0)
            {
                string file;
                var stream = GetFileStream("Fiscal.Certificacao.LAN.RotinaDesinstalacao.bat");
                using (var leitor = new StreamReader(stream))
                    file = string.Format(leitor.ReadToEnd(), pastas[0].Path);
                await SalvarArquivo("Rotina de desinstalação", file);
                log.Escrever(TitulosComuns.Sucesso, "Arquivo salvo com sucesso, agora execute-o para que o serviço seja instalado.");
            }
            else
                log.Escrever(TitulosComuns.Atenção, "Não foi encontrada nenhuma pasta com arquivo de instalação.");
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
