using BibliotecaCentral.Log;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage.Pickers;

namespace BibliotecaCentral.Certificacao
{
    public static class ExportarRepositorioRemoto
    {
        public async static Task Exportar(ILog log)
        {
            var salvador = new FileSavePicker()
            {
                SuggestedFileName = "Repositorio remoto de certificados",
                DefaultFileExtension = ".zip"
            };
            salvador.FileTypeChoices.Add("Arquivo comprimido", new string[1] { ".zip" });
            var arquivo = await salvador.PickSaveFileAsync();
            if (arquivo != null)
            {
                using (var stream = await arquivo.OpenStreamForWriteAsync())
                {
                    var recurso = new RecursoInserido().Retornar("BibliotecaCentral.Certificacao.RepositorioRemoto.zip");
                    recurso.CopyTo(stream);
                }
                log.Escrever(TitulosComuns.Sucesso, "Arquivo salvo com sucesso, inicie o repositório remoto com o Iniciar.bat");
            }
        }
    }
}
