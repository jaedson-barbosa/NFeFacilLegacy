using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml.Linq;

namespace ServidorCertificacaoConsole
{
    class Metodos
    {
        X509Store Loja { get; }

        public Metodos()
        {
            Loja = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            Loja.Open(OpenFlags.ReadOnly);
        }

        public void ObterCertificados(Stream stream)
        {
            X509Certificate2[] array = new X509Certificate2[Loja.Certificates.Count];
            Loja.Certificates.CopyTo(array, 0);
            XElement xml = new XElement("Certificados",
                array.Select(
                    x => new XElement("Certificado",
                    new XElement("Subject", x.Subject),
                    new XElement("SerialNumber", x.SerialNumber))
                )
            );
            var conteudo = xml.ToString();
            var data = Encoding.UTF8.GetBytes($"HTTP/1.1 200 OK\r\nContent-Type:text/xml\r\nContent-Length: {conteudo.Length}\r\nConnection: close\r\n\r\n{conteudo}");
            stream.Write(data, 0, data.Length);
        }

        public void ObterCertificado(Stream stream, string serial)
        {
            var cert = Loja.Certificates.Find(X509FindType.FindBySerialNumber, serial, true)[0];
            var data = cert.RawData;

            var cabecalho = Encoding.UTF8.GetBytes($"HTTP/1.1 200 OK\r\nContent-Length: {data.Length}\r\nConnection: close\r\n\r\n");
            stream.Write(cabecalho, 0, cabecalho.Length);

            stream.Write(data, 0, data.Length);
        }
    }
}