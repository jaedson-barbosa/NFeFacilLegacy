using BibliotecaCentral.ModeloXML.PartesProcesso;
using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace BibliotecaCentral.Certificacao
{
    public sealed class AssinaNFe
    {
        private NFe Nota;
        public AssinaNFe(NFe nfe)
        {
            Nota = nfe;
        }

        public void Assinar()
        {
            var xml = new XmlDocument();
            xml.Load(Nota.ToXElement<NFe>().CreateReader());
            var loja = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            loja.Open(OpenFlags.ReadOnly);
            var cert = Certificados.ObterCertificadoEscolhido();
            Nota.Signature = AssinarXML(xml, cert);
        }

        private Assinatura AssinarXML(XmlDocument xml, X509Certificate2 certificado)
        {
            var elemento = xml.GetElementsByTagName("infNFe")[0] as XmlElement;
            var id = elemento.Attributes.GetNamedItem("Id").Value;
            var signedXml = new SignedXml(elemento)
            {
                SigningKey = certificado.GetRSAPrivateKey(),
                KeyInfo = certificado
            };

            Reference reference = new Reference($"#{id}", signedXml);
            reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
            reference.AddTransform(new XmlDsigC14NTransform());
            signedXml.AddReference(reference);

            signedXml.ComputeSignature();

            return new Assinatura()
            {
                SignatureValue = Convert.ToBase64String(signedXml.Signature.SignatureValue),
                KeyInfo = new DetalhesChave
                {
                    X509Data = new DadosChave
                    {
                        X509Certificate = Convert.ToBase64String(signedXml.KeyInfo.RawData)
                    }
                },
                SignedInfo = new ModeloXML.PartesProcesso.SignedInfo
                {
                    CanonicalizationMethod = new Algoritmo
                    {
                        Algorithm = signedXml.SignedInfo.CanonicalizationMethod
                    },
                    SignatureMethod = new Algoritmo
                    {
                        Algorithm = signedXml.SignedInfo.SignatureMethod
                    },
                    Reference = new Referencia
                    {
                        DigestMethod = new Algoritmo
                        {
                            Algorithm = signedXml.SignedInfo.Reference.DigestMethod
                        },
                        DigestValue = Convert.ToBase64String(signedXml.SignedInfo.Reference.DigestValue),
                        URI = signedXml.SignedInfo.Reference.Uri,
                        Transforms = (from t in signedXml.SignedInfo.Reference.TransformChain
                                     select new Algoritmo
                                     {
                                         Algorithm = t.Algorithm
                                     }).ToArray()
                    }
                }
            };
        }
    }
}
