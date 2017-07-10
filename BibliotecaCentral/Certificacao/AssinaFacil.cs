using System;
using System.Threading.Tasks;
using System.Xml;
using Windows.UI.Xaml.Controls;

namespace BibliotecaCentral.Certificacao
{
    public struct AssinaFacil
    {
        private ISignature Nota;

        public AssinaFacil(ISignature nfe)
        {
            Nota = nfe;
        }

        public async Task Assinar<T>(string id, string tag)
        {
            var xml = new XmlDocument();
            using (var reader = Nota.ToXElement<T>().CreateReader())
            {
                xml.Load(reader);
                var caixa = new CaixasDialogo.SelecaoCertificado();
                if (await caixa.ShowAsync() == ContentDialogResult.Primary)
                {
                    var serial = caixa.CertificadoEscolhido;
                    var origem = ConfiguracoesCertificacao.Origem;
                    var cert = await Certificados.ObterCertificadoEscolhidoAsync(serial, origem);
                    Nota.Signature = new AssinaturaXML(xml, tag, id).AssinarXML(cert);
                }
            }
        }
    }
}
