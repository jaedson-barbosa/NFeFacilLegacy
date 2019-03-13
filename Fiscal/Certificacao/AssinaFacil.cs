using System;
using System.Threading.Tasks;
using System.Xml;
using System.Linq;
using System.Security.Cryptography.Xml;
using BaseGeral.ModeloXML.PartesAssinatura;
using System.Security.Cryptography.X509Certificates;
using BaseGeral;
using System.Security.Cryptography;

namespace Fiscal.Certificacao
{
    public sealed class AssinaFacil
    {
        public ISignature Nota { get; set; }

        public static readonly string[] Etapas = new string[2]
        {
            "Obter informações da assinatura",
            "Processar assinatura"
        };

        public AssinaFacil()
        {
            using (var loja = new X509Store(StoreName.My, StoreLocation.CurrentUser))
            {
                loja.Open(OpenFlags.ReadOnly);
                CertificadosDisponiveis = loja.Certificates
                    .Cast<X509Certificate2>().ToArray();
            }
        }

        public readonly X509Certificate2[] CertificadosDisponiveis;

        public event ProgressChangedEventHandler ProgressChanged;
        async Task OnProgressChanged(int conc)
        {
            if (ProgressChanged != null) await ProgressChanged(this, conc);
        }

        public async Task<(bool, string)> Assinar<T>(X509Certificate2 cert, string id, string tag)
        {
            var xml = new XmlDocument();
            using (var reader = Nota.ToXElement<T>().CreateReader())
            {
                xml.Load(reader);

                if (cert == null)
                {
                    return (false, "Selecione um certificado.");
                }
                try
                {
                    await OnProgressChanged(1);
                    Nota.Signature = AssinarXML(cert.GetRSAPrivateKey(), cert.RawData, id, tag, xml.OuterXml);
                    await OnProgressChanged(2);
                    return (true, "Documento assinado com sucesso.");
                }
                catch (Exception e)
                {
                    return (false, e.Message);
                }
            }
        }

        public Assinatura AssinarXML(RSA ChavePrivada, byte[] RawData, string Id, string Tag, string XML)
        {
            var doc = new XmlDocument();
            doc.LoadXml(XML);
            var signedXml = new SignedXml(doc)
            {
                Key = ChavePrivada
            };

            Reference reference = new Reference($"#{Id}", Tag, signedXml);
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
                        X509Certificate = Convert.ToBase64String(RawData)
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

    public delegate Task ProgressChangedEventHandler(object sender, int Concluidos);
}
