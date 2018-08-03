using ServidorCertificacao.Pacotes;
using System;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml;
using System.Security.Cryptography.Xml;
using System.ServiceModel;

namespace ServidorCertificacao
{
    class Metodos
    {
        public string ObterCertificados(Stream stream)
        {
            var retorno = new CertificadosExibicaoDTO();
            var xml = Serializar(retorno);
            return xml.ToString(SaveOptions.DisableFormatting);
        }

        public string AssinarRemotamente(Stream stream, CertificadoAssinaturaDTO cert)
        {
            using (var loja = new X509Store())
            {
                loja.Open(OpenFlags.ReadOnly);
                var x509 = loja.Certificates.Find(X509FindType.FindBySerialNumber, cert.Serial, true)[0];
                var assinatura = AssinarXML(cert.XML, x509, cert.Tag);
                var xml = Serializar(assinatura);
                return assinatura;
            }
        }

        static XElement Serializar<T>(T objeto)
        {
            using (var memoryStream = new MemoryStream())
            {
                var name = new XmlSerializerNamespaces();
                name.Add(string.Empty, string.Empty);

                var xmlSerializer = new XmlSerializer(typeof(T));
                xmlSerializer.Serialize(memoryStream, objeto, name);
                memoryStream.Position = 0;
                return XElement.Load(memoryStream);
            }
        }

        public async Task<string> EnviarRequisicaoAsync(Stream stream, RequisicaoEnvioDTO req, X509Certificate2 cert)
        {
            var bind = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
            bind.Security.Transport.ClientCredentialType = HttpClientCredentialType.Certificate;
            var client = new AutorizacaoSVRS.NFeAutorizacao4SoapClient(bind, new EndpointAddress(req.Uri));
            client.ClientCredentials.ClientCertificate.Certificate = cert;

            client.Open();
            var xml = new XmlDocument();
            xml.LoadXml(req.Conteudo.ToString(SaveOptions.DisableFormatting));
            var resp = await client.nfeAutorizacaoLoteAsync(xml);

            var retorno = resp.nfeResultMsg.OuterXml;
            return retorno;
        }

        public string AssinarXML(string xml, X509Certificate2 certificado, string tag)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNodeList ListInfNFe = doc.GetElementsByTagName(tag);
            XmlElement infNFe = (XmlElement)ListInfNFe[0];
            string id = infNFe.Attributes.GetNamedItem("Id").Value;
            var signedXml = new SignedXml(infNFe)
            {
                SigningKey = certificado.PrivateKey
            };

            // Transformações p/ DigestValue da Nota
            Reference reference = new Reference("#" + id);
            reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
            reference.AddTransform(new XmlDsigC14NTransform());
            signedXml.AddReference(reference);

            KeyInfo keyInfo = new KeyInfo();
            keyInfo.AddClause(new KeyInfoX509Data(certificado));
            signedXml.KeyInfo = keyInfo;

            signedXml.ComputeSignature();

            XmlElement xmlSignedInfo = signedXml.SignedInfo.GetXml();
            XmlElement xmlKeyInfo = signedXml.KeyInfo.GetXml();

            XmlElement xmlSignatureValue = doc.CreateElement("SignatureValue");
            string signBase64 = Convert.ToBase64String(signedXml.Signature.SignatureValue);
            XmlText text = doc.CreateTextNode(signBase64);
            xmlSignatureValue.AppendChild(text);

            XElement xmlSignature = new XElement("Assinatura");
            var xSignedInfo = XElement.Parse(xmlSignedInfo.OuterXml);
            var xSignatureValue = XElement.Parse(xmlSignatureValue.OuterXml);
            var xKeyInfo = XElement.Parse(xmlKeyInfo.OuterXml);

            xmlSignature.Add(xSignedInfo);
            xmlSignature.Add(xSignatureValue);
            xmlSignature.Add(xKeyInfo);
            var str = xmlSignature.ToString(SaveOptions.DisableFormatting);
            return str.Replace("xmlns=\"http://www.w3.org/2000/09/xmldsig#\"", string.Empty);
        }
    }
}