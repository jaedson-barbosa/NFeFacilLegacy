using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace BibliotecaCentral.ImportacaoParaBanco
{
    public abstract class Importacao
    {
        private string[] Extensão;

        protected Importacao(params string[] extensão)
        {
            Extensão = extensão;
        }

        protected async Task<StorageFile> ImportarArquivo()
        {
            var importar = CriarImportador();
            return await importar.PickSingleFileAsync();
        }

        protected async Task<IReadOnlyList<StorageFile>> ImportarArquivos()
        {
            var importar = CriarImportador();
            return await importar.PickMultipleFilesAsync();
        }

        private FileOpenPicker CriarImportador()
        {
            var importar = new FileOpenPicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };
            foreach (var item in Extensão) importar.FileTypeFilter.Add(item);
            return importar;
        }
    }
}
