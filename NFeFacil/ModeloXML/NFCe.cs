using NFeFacil.Certificacao;
using NFeFacil.Fiscal.ViewNFCe;
using NFeFacil.ModeloXML.PartesAssinatura;
using System;
using System.Text;
using System.Xml.Serialization;

namespace NFeFacil.ModeloXML
{
    [XmlRoot(nameof(NFe), Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public sealed class NFCe : ISignature
    {
        [XmlElement("infNFe", Order = 0)]
        public InformacoesNFCe Informacoes { get; set; }

        [XmlElement("infNFeSupl", Order = 1)]
        public InformacoesSuplementaresNFCe InfoSuplementares { get; set; }

        [XmlElement("Signature", Namespace = "http://www.w3.org/2000/09/xmldsig#", Order = 2)]
        public Assinatura Signature { get; set; }

        [XmlIgnore]
        public bool AmbienteTestes => Informacoes.identificacao.TipoAmbiente == 2;

        public void PrepararInformacoesSuplementares(string cIdToken, string CSC)
        {
            var chNFe = Informacoes.ChaveAcesso;
            var nVersao = "100";
            var tpAmb = Informacoes.identificacao.TipoAmbiente;
            var cDest = Informacoes.destinatário?.Documento;
            cDest = string.IsNullOrEmpty(cDest) ? null : cDest;
            var dhEmi = ToHex(Informacoes.identificacao.DataHoraEmissão);
            var vNF = ExtensoesPrincipal.ToStr(Informacoes.total.ICMSTot.vNF);
            var vICMS = ExtensoesPrincipal.ToStr(Informacoes.total.ICMSTot.vICMS);
            var digVal = ToHex(Signature.SignedInfo.Reference.DigestValue);
            byte[] cHashQRCode;
            string[,] stringsConcatenacao;
            using (var hash = System.Security.Cryptography.SHA1.Create())
            {
                stringsConcatenacao = new string[10, 2]
                {
                    { nameof(chNFe), chNFe },
                    { nameof(nVersao), nVersao },
                    { nameof(tpAmb), tpAmb.ToString() },
                    { nameof(cDest), cDest },
                    { nameof(dhEmi), dhEmi },
                    { nameof(vNF), vNF },
                    { nameof(vICMS), vICMS },
                    { nameof(digVal), digVal },
                    { nameof(cIdToken), cIdToken },
                    { null, null }
                };
                var concatenacao = ConcatenarStrings(stringsConcatenacao, true);
                var bytes = Encoding.ASCII.GetBytes(concatenacao + CSC);
                cHashQRCode = hash.ComputeHash(bytes, 0, bytes.Length);
            }
            var hashCalculado = BitConverter.ToString(cHashQRCode).Replace("-", "").ToLower();
            stringsConcatenacao[9, 0] = nameof(cHashQRCode);
            stringsConcatenacao[9, 1] = hashCalculado;

            var enderecos = AmbienteTestes ? UrlsQR.Homologacao : UrlsQR.Producao;
            var ufEmitente = Informacoes.Emitente.Endereco.SiglaUF;
            var enderecoConsultaQR = enderecos[ufEmitente];
            if (enderecoConsultaQR[enderecoConsultaQR.Length - 1] != '&')
            {
                enderecoConsultaQR += '?';
            }
            InfoSuplementares = new InformacoesSuplementaresNFCe()
            {
                Uri = enderecoConsultaQR + ConcatenarStrings(stringsConcatenacao, false)
            };

            string ToHex(string str) => BitConverter.ToString(Encoding.ASCII.GetBytes(str)).Replace("-", "").ToLower();
        }

        static string ConcatenarStrings(string[,] strings, bool envolveCalculoHash)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < strings.GetLength(0); i++)
            {
                string titulo = strings[i, 0];
                string valor = strings[i, 1];

                if (string.IsNullOrEmpty(valor) &&
                    (!envolveCalculoHash || string.IsNullOrEmpty(titulo))) continue;
                else if (i > 0)
                {
                    builder.Append('&');
                }
                builder.Append($"{titulo}={valor}");
            }
            return builder.ToString();
        }
    }
}
