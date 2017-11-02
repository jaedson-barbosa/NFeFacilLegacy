using Comum.Pacotes;
using ServidorCertificacaoConsole.PartesAssinatura;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using ServidorCertificacaoConsole.CodigoNet;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml;
using Comum.Primitivos;

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
            
            var retorno = new CertificadosExibicaoDTO(Loja.Certificates.Count);
            foreach (var item in Loja.Certificates)
            {
                retorno.Registro.Add(new CertificadoExibicao
                {
                    SerialNumber = item.SerialNumber,
                    Subject = item.Subject
                });
            }
            var xml = Serializar(retorno);

            var data = Encoding.UTF8.GetBytes(xml.ToString());
            EscreverCabecalho(stream, data.Length);
            stream.Write(data, 0, data.Length);
        }

        public void ObterChaveCertificado(Stream stream, string serial)
        {
            var cert = Loja.Certificates.Find(X509FindType.FindBySerialNumber, serial, true)[0];
            var rsa = cert.GetRSAPrivateKey();
            var paramRSA = rsa.ExportParameters(true);
            var obj = new CertificadoAssinaturaDTO()
            {
                ParametrosChavePrivada = paramRSA,
                RawData = cert.RawData
            };
            var xml = Serializar(obj);

            var data = Encoding.UTF8.GetBytes(xml.ToString());
            EscreverCabecalho(stream, data.Length);
            stream.Write(data, 0, data.Length);
        }

        public async Task EnviarRequisicaoAsync(Stream stream, RequisicaoEnvioDTO req)
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

                var data = Encoding.UTF8.GetBytes(xml.ToString());
                EscreverCabecalho(stream, data.Length);
                stream.Write(data, 0, data.Length);
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

        static void EscreverCabecalho(Stream stream, int tamanho)
        {
            var cabecalho = Encoding.UTF8.GetBytes($"HTTP/1.1 200 OK\r\nContent-Length: {tamanho}\r\nConnection: close\r\n\r\n");
            stream.Write(cabecalho, 0, cabecalho.Length);
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