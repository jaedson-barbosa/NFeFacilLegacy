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
                var assinatura = AssinarXML(cert.XML, x509);
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

        public async Task<string> EnviarRequisicaoAsync(Stream stream, RequisicaoEnvioDTO req)
        {
            using (var proxy = new HttpClient(new HttpClientHandler()
            {
                ClientCertificateOptions = ClientCertificateOption.Automatic,
                UseDefaultCredentials = true
            }, true))
            {
                proxy.DefaultRequestHeaders.Add(req.Cabecalho.Nome, req.Cabecalho.Valor);
                var resposta = await proxy.PostAsync(req.Uri,
                    new StringContent(req.Conteudo.ToString(SaveOptions.DisableFormatting), Encoding.UTF8, "text/xml"));
                var str = await resposta.Content.ReadAsStringAsync();
                var xmlPrimario = XElement.Load(await resposta.Content.ReadAsStreamAsync());
                var xml = ObterConteudoCorpo(xmlPrimario);
                return xml.ToString(SaveOptions.DisableFormatting);
            }

            XNode ObterConteudoCorpo(XElement soap)
            {
                var nome = XName.Get("Body", "http://schemas.xmlsoap.org/soap/envelope/");
                var item = soap.Element(nome);
                if (item == null)
                {
                    nome = XName.Get("Body", "http://www.w3.org/2003/05/soap-envelope");
                    item = soap.Element(nome);
                }
                var casca = (XElement)item.FirstNode;
                return casca.FirstNode;
            }
        }

        public string AssinarXML(string xml, X509Certificate2 certificado)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNodeList ListInfNFe = doc.GetElementsByTagName("infNFe");
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