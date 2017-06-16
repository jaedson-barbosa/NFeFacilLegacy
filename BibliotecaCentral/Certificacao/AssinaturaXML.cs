using BibliotecaCentral.ModeloXML.PartesProcesso;
using Comum.Primitivos;
using System;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace BibliotecaCentral.Certificacao
{
    public sealed class AssinaturaXML
    {
        XmlDocument xml;
        string tag;
        string id;

        public AssinaturaXML(XmlDocument xml, string tag, string id)
        {
            this.xml = xml;
            this.tag = tag;
            this.id = id;
        }

        public Assinatura AssinarXML(CertificadoAssinatura certificado)
        {
            var signedXml = new SignedXml(xml.DocumentElement)
            {
                Key = certificado.ChavePrivada
            };

            Reference reference = new Reference($"#{id}", tag, signedXml);
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
                        X509Certificate = Convert.ToBase64String(certificado.RawData)
                    }
                },
                SignedInfo = new SignedInfo
                {
                    CanonicalizationMethod = new Algoritmo
                    {
                        Algorithm = signedXml.Signature.CanonicalizationMethod
                    },
                    SignatureMethod = new Algoritmo
                    {
                        Algorithm = signedXml.Signature.SignatureMethod
                    },
                    Reference = new Referencia
                    {
                        DigestMethod = new Algoritmo
                        {
                            Algorithm = signedXml.Signature.Reference.DigestMethod
                        },
                        DigestValue = Convert.ToBase64String(signedXml.Signature.Reference.DigestValue),
                        URI = signedXml.Signature.Reference.Uri,
                        Transforms = (from t in signedXml.Signature.Reference.TransformChain
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
