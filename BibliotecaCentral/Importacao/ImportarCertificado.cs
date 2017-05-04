using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace BibliotecaCentral.Importacao
{
    public sealed class ImportarCertificado : Importacao
    {
        private StorageFolder PastaArquivos = ApplicationData.Current.LocalFolder;

        public ImportarCertificado() : base(".pfx") { }

        public override async Task<RelatorioImportacao> ImportarAsync()
        {
            var retorno = new RelatorioImportacao();
            FileOpenPicker abrir = new FileOpenPicker
            {
                SuggestedStartLocation = PickerLocationId.Downloads,
            };
            abrir.FileTypeFilter.Add(".pfx");
            var arq = await abrir.PickSingleFileAsync();
            if (arq == null)
            {
                retorno.Erros.Add(new Exception("Nenhum arquivo foi selecionado."));
            }
            else
            {
                await arq.CopyAsync(PastaArquivos);
            }
            return retorno;
        }
    }
}
