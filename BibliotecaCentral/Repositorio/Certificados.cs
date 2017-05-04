using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace BibliotecaCentral.Repositorio
{
    public sealed class Certificados
    {
        private ApplicationDataContainer pasta = ApplicationData.Current.LocalSettings;

        public IEnumerable<X509Certificate2> Registro
        {
            get
            {
                var loja = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                loja.Open(OpenFlags.ReadOnly);
                return loja.Certificates.Cast<X509Certificate2>();
            }
        }

        public string Escolhido
        {
            get { return pasta.Values[nameof(Escolhido)] as string; }
            set { pasta.Values[nameof(Escolhido)] = value; }
        }

        public async System.Threading.Tasks.Task<X509Certificate2> CertificadoEscolhidoAsync()
        {
            //FileOpenPicker abrir = new FileOpenPicker
            //{
            //    SuggestedStartLocation = PickerLocationId.Downloads,
            //};
            //abrir.FileTypeFilter.Add(".pfx");
            //var arq = await abrir.PickSingleFileAsync();
            return new X509Certificate2(@"C:\Users\jaeds\AppData\Local\Packages\f85d4d3f-0a9a-44a0-8c0c-582b27cac00a_jnfgjghanf488\LocalState\SEVERINO ALVES.pfx", "12345678");
        }
    }
}
