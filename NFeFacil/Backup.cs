using Newtonsoft.Json;
using NFeFacil.Log;
using NFeFacil.PacotesBanco;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage.Pickers;

namespace NFeFacil
{
    public static class Backup
    {
        public async static void SalvarBackup()
        {
            var objeto = new ConjuntoBanco();
            objeto.AtualizarPadrao();
            var json = JsonConvert.SerializeObject(objeto);

            var caixa = new FileSavePicker();
            caixa.FileTypeChoices.Add("Arquivo JSON", new string[] { ".json" });
            var arq = await caixa.PickSaveFileAsync();
            if (arq != null)
            {
                var stream = await arq.OpenStreamForWriteAsync();
                using (StreamWriter escritor = new StreamWriter(stream))
                {
                    await escritor.WriteAsync(json);
                    await escritor.FlushAsync();
                }
            }
        }

        public async static Task<bool> RestaurarBackup()
        {
            var caixa = new FileOpenPicker();
            caixa.FileTypeFilter.Add(".json");
            var arq = await caixa.PickSingleFileAsync();
            if (arq != null)
            {
                var stream = await arq.OpenStreamForReadAsync();
                using (var leitor = new StreamReader(stream))
                {
                    try
                    {
                        var texto = await leitor.ReadToEndAsync();
                        var conjunto = JsonConvert.DeserializeObject<ConjuntoBanco>(texto);
                        try
                        {
                            conjunto.AnalisarESalvar();
                            Popup.Current.Escrever(TitulosComuns.Sucesso, "Backup restaurado com sucesso.");
                            return true;
                        }
                        catch (Exception e)
                        {
                            e.ManipularErro();
                        }
                    }
                    catch (Exception e)
                    {
                        e.ManipularErro();
                    }
                }
            }
            return false;
        }
    }
}
