using BibliotecaCentral.Log;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage.Pickers;

namespace BibliotecaCentral.Certificacao.LAN
{
    public struct Exportacao
    {
        ILog log;

        public Exportacao(ILog log)
        {
            this.log = log;
        }

        public async Task Exportar(string nome, string nomeFormato, string extensao)
        {
            var salvador = new FileSavePicker()
            {
                SuggestedFileName = "Repositorio remoto de certificados",
                DefaultFileExtension = '.' + extensao
            };
            salvador.FileTypeChoices.Add(nomeFormato, new string[1] { ".zip" });
            var arquivo = await salvador.PickSaveFileAsync();
            if (arquivo != null)
            {
                using (var stream = await arquivo.OpenStreamForWriteAsync())
                {
                    var recurso = new RecursoInserido().Retornar($"BibliotecaCentral.Certificacao.LAN.Arquivos.{nome}.{extensao}");
                    recurso.CopyTo(stream);
                }
                log.Escrever(TitulosComuns.Sucesso, "Arquivo salvo com sucesso, inicie o repositório remoto com o Iniciar.bat");
            }
        }
    }
}
