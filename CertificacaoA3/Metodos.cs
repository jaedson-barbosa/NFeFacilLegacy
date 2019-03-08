using CertificacaoA3.Pacotes;
using System;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace CertificacaoA3
{
    class Metodos
    {
        public XElement ObterCertificados()
        {
            var retorno = new CertificadosExibicaoDTO();
            var xml = Serializar(retorno);
            return xml;
        }

        public XElement AssinarRemotamente(CertificadoAssinaturaDTO cert)
        {
            using (var loja = new X509Store())
            {
                loja.Open(OpenFlags.ReadOnly);
                for (int i = 0; i < loja.Certificates.Count; i++)
                {
                    var temp = loja.Certificates[i];
                    if (temp.SerialNumber == cert.Serial)
                    {
                        var assinatura = AssinarXML(cert.XML, temp, cert.Tag);
                        return XElement.Parse(assinatura);
                    }
                }
            }
            return null;
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

        public async Task<XElement> EnviarRequisicaoAsync(RequisicaoEnvioDTO req)
        {
            using (var proxy = new HttpClient(new HttpClientHandler()
            {
                ClientCertificateOptions = ClientCertificateOption.Automatic,
                UseDefaultCredentials = true
            }, true))
            {
                proxy.DefaultRequestHeaders.Add(req.Cabecalho.Nome, req.Cabecalho.Valor);
                var resposta = await proxy.PostAsync(req.Uri,
                    new StringContent(req.Conteudo.ToString(SaveOptions.DisableFormatting), Encoding.UTF8, req.TipoConteudo));
                var str = await resposta.Content.ReadAsStringAsync();
                var xmlPrimario = XElement.Load(await resposta.Content.ReadAsStreamAsync());
                var xml = ObterConteudoCorpo(xmlPrimario);
                return xml;
            }

            XElement ObterConteudoCorpo(XElement soap)
            {
                var nome = XName.Get("Body", "http://schemas.xmlsoap.org/soap/envelope/");
                var item = soap.Element(nome);
                if (item == null)
                {
                    nome = XName.Get("Body", "http://www.w3.org/2003/05/soap-envelope");
                    item = soap.Element(nome);
                }
                var casca = (XElement)item.FirstNode;
                return (XElement)casca.FirstNode;
            }
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
