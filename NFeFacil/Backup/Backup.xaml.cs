using Newtonsoft.Json;
using System;
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
            var original = await CriarZip.RetornarArquivo();
            caixa.FileTypeChoices.Add("Arquivo comprimido", new string[] { ".zip" });
            var novo = await caixa.PickSaveFileAsync();
            await original.CopyAndReplaceAsync(novo);
            await CriarZip.ExcluirArquivo();
        }
    }
}
