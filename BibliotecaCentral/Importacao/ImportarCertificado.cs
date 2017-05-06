using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace BibliotecaCentral.Importacao
{
    public sealed class ImportarCertificado : Importacao
    {
        private StorageFolder PastaArquivos = ApplicationData.Current.LocalFolder;

        public ImportarCertificado() : base(".pfx") { }

        public override async Task<List<Exception>> ImportarAsync()
        {
            var retorno = new List<Exception>();
            FileOpenPicker abrir = new FileOpenPicker
            {
                SuggestedStartLocation = PickerLocationId.Downloads,
            };
            abrir.FileTypeFilter.Add(".pfx");
            var arq = await abrir.PickSingleFileAsync();
            if (arq == null)
            {
                retorno.Add(new Exception("Nenhum arquivo foi selecionado."));
            }
            else
            {
                await arq.CopyAsync(PastaArquivos);
            }
            return retorno;
        }
    }
}
