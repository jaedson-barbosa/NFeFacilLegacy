using NFeFacil.Certificacao;
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
            var vNF = ToStr(Informacoes.total.ICMSTot.vNF, 13, 2);
            var vICMS = ToStr(Informacoes.total.ICMSTot.vICMS, 13, 2);
            var digVal = ToHex(Signature.SignedInfo.Reference.DigestValue);
            string cHashQRCode;
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
                    { nameof(CSC), CSC }
                };
                var concatenacao = ConcatenarStrings(stringsConcatenacao);
                var bytes = Encoding.UTF8.GetBytes(concatenacao);
                cHashQRCode = Encoding.UTF8.GetString(hash.ComputeHash(bytes, 0, bytes.Length));
            }
            cHashQRCode = ToHex(cHashQRCode);
            stringsConcatenacao[9, 0] = nameof(cHashQRCode);
            stringsConcatenacao[9, 1] = cHashQRCode;

            InfoSuplementares = new InformacoesSuplementaresNFCe()
            {
                Uri = ConcatenarStrings(stringsConcatenacao)
            };

            string ToHex(string str) => BitConverter.ToString(Encoding.ASCII.GetBytes(str));
            string ToStr(double num, int parteInteira, int parteDecimal)
            {
                var tamInt = new string('0', parteInteira);
                var tamDec = new string('0', parteDecimal);
                return ExtensoesPrincipal.ToStr(num, $"{tamInt},{tamDec}");
            }
        }

        static string ConcatenarStrings(string[,] strings)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < strings.GetLength(0); i++)
            {
                if (i > 0)
                {
                    builder.Append('&');
                }
                builder.Append($"{strings[i,0]}={strings[i,1]}");
            }
            return builder.ToString();
        }
    }
}
