using System;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace BibliotecaCentral.Assinatura
{
    internal static class AssinaXML
    {
        public static XmlElement AssinarXML(XmlDocument xml, X509Certificate2 certificado)
        {
            var elemento = xml.GetElementsByTagName("infNFe")[0] as XmlElement;
            var id = elemento.Attributes.GetNamedItem("Id").Value;
            var signedXml = new SignedXml(elemento)
            {
                SigningKey = certificado.GetRSAPrivateKey()
            };

            Reference reference = new Reference($"#{id}");
            reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
            reference.AddTransform(new XmlDsigC14NTransform());
            signedXml.AddReference(reference);

            KeyInfo keyInfo = new KeyInfo();
            keyInfo.AddClause(new KeyInfoX509Data(certificado));
            signedXml.KeyInfo = keyInfo;

            signedXml.ComputeSignature();

            XmlElement xmlSignature = xml.CreateElement("Signature", "http://www.w3.org/2000/09/xmldsig#");
            XmlElement xmlSignedInfo = signedXml.SignedInfo.GetXml();
            XmlElement xmlKeyInfo = signedXml.KeyInfo.GetXml();

            XmlElement xmlSignatureValue = xml.CreateElement("SignatureValue", xmlSignature.NamespaceURI);
            string signBase64 = Convert.ToBase64String(signedXml.Signature.SignatureValue);
            XmlText text = xml.CreateTextNode(signBase64);
            xmlSignatureValue.AppendChild(text);

            xmlSignature.AppendChild(xml.ImportNode(xmlSignedInfo, true));
            xmlSignature.AppendChild(xmlSignatureValue);
            xmlSignature.AppendChild(xml.ImportNode(xmlKeyInfo, true));

            return xmlSignature;
        }
    }
}
