using System;
using System.Threading.Tasks;
using System.Xml;
using Windows.UI.Xaml.Controls;
using NFeFacil.Primitivos;
using System.Linq;
using System.Security.Cryptography.Xml;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesAssinatura;
using System.Security.Cryptography.X509Certificates;
using NFeFacil.Pacotes;

namespace NFeFacil.Certificacao
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
                var caixa = new SelecaoCertificado();
                if (await caixa.ShowAsync() == ContentDialogResult.Primary)
                {
                    var serial = caixa.CertificadoEscolhido;

                    if (ConfiguracoesCertificacao.Origem == OrigemCertificado.Importado)
                    {
                        using (var loja = new X509Store())
                        {
                            loja.Open(OpenFlags.ReadOnly);
                            var temp = loja.Certificates.Find(X509FindType.FindBySerialNumber, serial, true)[0];
                            Nota.Signature = AssinarXML(new CertificadoAssinatura
                            {
                                ChavePrivada = temp.GetRSAPrivateKey(),
                                RawData = temp.RawData,
                                Id = id,
                                Tag = tag,
                                XML = xml.OuterXml
                            });
                        }
                    }
                    else
                    {
                        var operacoes = new LAN.OperacoesServidor();
                        Nota.Signature = await operacoes.AssinarRemotamente(new CertificadoAssinaturaDTO
                        {
                            Id = id,
                            Tag = tag,
                            XML = xml.OuterXml,
                            Serial = serial
                        });
                    }
                }
            }
        }

        public Assinatura AssinarXML(CertificadoAssinatura certificado)
        {
            var doc = new XmlDocument();
            doc.LoadXml(certificado.XML);
            var signedXml = new SignedXml(doc)
            {
                Key = certificado.ChavePrivada
            };

            Reference reference = new Reference($"#{certificado.Id}", certificado.Tag, signedXml);
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
