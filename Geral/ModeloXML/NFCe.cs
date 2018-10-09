using BaseGeral.ModeloXML.PartesAssinatura;
using System;
using System.Text;
using System.Xml.Serialization;

namespace BaseGeral.ModeloXML
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
            var nVersao = "2";
            var tpAmb = Informacoes.identificacao.TipoAmbiente;
            byte[] cHashQRCode;
            string[] stringsConcatenacao;
            using (var hash = System.Security.Cryptography.SHA1.Create())
            {
                stringsConcatenacao = new string[5] { chNFe, nVersao, tpAmb.ToString(), cIdToken, null };
                var concatenacao = ConcatenarStrings(stringsConcatenacao) + CSC;
                var bytes = Encoding.ASCII.GetBytes(concatenacao);
                cHashQRCode = hash.ComputeHash(bytes, 0, bytes.Length);
            }
            var hashCalculado = BitConverter.ToString(cHashQRCode).Replace("-", "");
            stringsConcatenacao[5] = hashCalculado;

            var enderecos = AmbienteTestes ? UrlsQR.Homologacao : UrlsQR.Producao;
            var ufEmitente = Informacoes.Emitente.Endereco.SiglaUF;
            var enderecoConsultaQR = enderecos[ufEmitente];
            if (enderecoConsultaQR[enderecoConsultaQR.Length - 1] != '&')
            {
                enderecoConsultaQR += '?';
            }
            InfoSuplementares = new InformacoesSuplementaresNFCe()
            {
                Uri = enderecoConsultaQR + ConcatenarStrings(stringsConcatenacao),
                UriChave = tpAmb == 1
                    ? UrlsChaveAcesso.Producao[ufEmitente]
                    : UrlsChaveAcesso.Homologacao[ufEmitente]
            };
        }

        static string ConcatenarStrings(string[] strings)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < strings.Length; i++)
            {
                string valor = strings[i];
                if (string.IsNullOrEmpty(valor)) continue;
                builder.Append(valor);
                if (i < strings.Length - 1)
                    builder.Append('|');
            }
            return builder.ToString();
        }
    }
}
