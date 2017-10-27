using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace NFeFacil.Importacao
{
    public abstract class Importacao
    {
        private string[] Extensão;

        protected Importacao(params string[] extensão)
        {
            Extensão = extensão;
        }

        protected async Task<IReadOnlyList<StorageFile>> ImportarArquivos()
        {
            var importar = new FileOpenPicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };
            foreach (var item in Extensão) importar.FileTypeFilter.Add(item);
            return await importar.PickMultipleFilesAsync();
        }
    }
}
