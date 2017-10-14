using Newtonsoft.Json;
using NFeFacil.Log;
using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace NFeFacil.Backup
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class Backup : Page
    {
        public Backup()
        {
            InitializeComponent();
        }

        async void SalvarBackup(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            var objeto = new ConjuntoBanco();
            objeto.AtualizarPadrao();
            var json = JsonConvert.SerializeObject(objeto);
            await CriarZip.Zipar(json);

            var caixa = new FileSavePicker();
            caixa.FileTypeChoices.Add("Arquivo comprimido", new string[] { ".zip" });
            var novo = await caixa.PickSaveFileAsync();
            if (novo != null)
            {
                var original = await CriarZip.RetornarArquivo();
                await original.CopyAndReplaceAsync(novo);
                await CriarZip.ExcluirArquivo();
            }
        }

        async void RestaurarBackup(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            var caixa = new FileOpenPicker();
            caixa.FileTypeFilter.Add(".zip");
            var arq = await caixa.PickSingleFileAsync();
            if (arq != null)
            {
                var pasta = ApplicationData.Current.TemporaryFolder;
                pasta = await pasta.CreateFolderAsync("Temp", CreationCollisionOption.ReplaceExisting);
                arq = await arq.CopyAsync(pasta, "Backup.zip", NameCollisionOption.ReplaceExisting);
                var aberto = await Task.Run(() => ZipFile.Open(arq.Path, ZipArchiveMode.Read));
                pasta = ApplicationData.Current.TemporaryFolder;
                foreach (var item in await pasta.GetFilesAsync())
                {
                    await item.DeleteAsync();
                }
                aberto.ExtractToDirectory(pasta.Path);
                arq = await pasta.GetFileAsync("Backup.json");
                if (arq != null)
                {
                    using (var leitor = new StreamReader(await arq.OpenStreamForReadAsync()))
                    {
                        try
                        {
                            var texto = await leitor.ReadToEndAsync();
                            var conjunto = JsonConvert.DeserializeObject<ConjuntoBanco>(texto);
                            try
                            {
                                conjunto.AnalisarESalvar();
                                Popup.Current.Escrever(TitulosComuns.Sucesso, "Backup restaurado com sucesso.");
                            }
                            catch (Exception)
                            {
                                Popup.Current.Escrever(TitulosComuns.Erro, "Erro ao salvar os itens do backup, aparentemente já existem dados cadastrados neste aplicativo.");
                            }
                        }
                        catch (Exception)
                        {
                            Popup.Current.Escrever(TitulosComuns.Erro, "Este não é um arquivo de backup válido.");
                        }
                    }
                }
            }
        }
    }
}
