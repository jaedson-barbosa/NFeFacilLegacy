using System;
using System.Threading.Tasks;
using System.Xml;
using NFeFacil.Certificacao.LAN.Primitivos;
using System.Linq;
using System.Security.Cryptography.Xml;
using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesAssinatura;
using System.Security.Cryptography.X509Certificates;
using NFeFacil.Certificacao.LAN.Pacotes;
using NFeFacil.View;
using System.Security.Cryptography;

namespace NFeFacil.Certificacao
{
    public sealed class AssinaFacil
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

                var listaCertificados = await Certificados.ObterCertificadosAsync(ConfiguracoesCertificacao.Origem);
                Progresso progresso = null;
                progresso = new Progresso(async x =>
                {
                    if (x == null)
                    {
                        return (false, "Selecione um certificado.");
                    }
                    try
                    {
                        var cert = (CertificadoExibicao)x;
                        var serial = cert.SerialNumber;
                        await progresso.Update(1);

                        if (ConfiguracoesCertificacao.Origem == OrigemCertificado.Importado)
                        {
                            using (var loja = new X509Store())
                            {
                                loja.Open(OpenFlags.ReadOnly);
                                var temp = loja.Certificates.Find(X509FindType.FindBySerialNumber, serial, true)[0];
                                Nota.Signature = AssinarXML(temp.GetRSAPrivateKey(), temp.RawData, id, tag, xml.OuterXml);
                                await progresso.Update(2);

                                return (true, "Documento assinado com sucesso.");
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
                            await progresso.Update(2);

                            return (true, "Documento assinado com sucesso.");
                        }
                    }
                    catch (Exception e)
                    {
                        return (false, e.Message);
                    }
                }, listaCertificados, "Subject",
                "Obter informações da assinatura",
                "Processar assinatura");
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
}
