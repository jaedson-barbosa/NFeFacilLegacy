using NFeFacil.Certificacao;
using NFeFacil.ModeloXML.PartesAssinatura;
using System;
using System.Collections.Generic;
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
                var concatenacao = ConcatenarStrings(stringsConcatenacao) + CSC;
                var bytes = Encoding.ASCII.GetBytes(concatenacao);
                cHashQRCode = hash.ComputeHash(bytes, 0, bytes.Length);
            }
            var hashCalculado = BitConverter.ToString(cHashQRCode).Replace("-", "");
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
                Uri = enderecoConsultaQR + ConcatenarStrings(stringsConcatenacao)
            };

            string ToHex(string str) => BitConverter.ToString(Encoding.ASCII.GetBytes(str)).Replace("-", "");
        }

        static string ConcatenarStrings(string[,] strings)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < strings.GetLength(0); i++)
            {
                string titulo = strings[i, 0];
                string valor = strings[i, 1];

                if (string.IsNullOrEmpty(valor)) continue;
                else if (i > 0)
                {
                    builder.Append('&');
                }
                builder.Append($"{titulo}={valor}");
            }
            return builder.ToString();
        }
    }

    class UrlsQR
    {
        public static readonly Dictionary<string, string> Producao = new Dictionary<string, string>
        {
            { "AC", "http://www.sefaznet.ac.gov.br/nfce/qrcode" },
            { "AL", "http://nfce.sefaz.al.gov.br/QRCode/consultarNFCe.jsp" },
            { "AM", "http://sistemas.sefaz.am.gov.br/nfceweb/consultarNFCe.jsp" },
            { "AP", "https://www.sefaz.ap.gov.br/nfce/nfcep.php" },
            { "BA", "http://nfe.sefaz.ba.gov.br/servicos/nfce/modulos/geral/NFCEC_consulta_chave_acesso.aspx" },
            { "CE", "http://nfce.sefaz.ce.gov.br/pages/ShowNFCe.html" },
            { "DF", "http://dec.fazenda.df.gov.br/ConsultarNFCe.aspx" },
            { "ES", "http://app.sefaz.es.gov.br/ConsultaNFCe/qrcode.aspx" },
            { "GO", "http://nfe.sefaz.go.gov.br/nfeweb/sites/nfce/danfeNFCe" },
            { "MA", "http://www.nfce.sefaz.ma.gov.br/portal/consultaNFe.do?method=preFilterCupom&" },
            { "MG", "" },
            { "MS", "http://www.dfe.ms.gov.br/nfce/qrcode" },
            { "MT", "http://www.sefaz.mt.gov.br/nfce/consultanfce" },
            { "PA", "https://appnfc.sefa.pa.gov.br/portal/view/consultas/nfce/nfceForm.seam" },
            { "PB", "http://www.receita.pb.gov.br/nfce" },
            { "PE", "http://nfce.sefaz.pe.gov.br/nfce-web/consultarNFCe" },
            { "PI", "http://webas.sefaz.pi.gov.br/nfceweb/consultarNFCe.jsf" },
            { "PR", "http://www.dfeportal.fazenda.pr.gov.br/dfe-portal/rest/servico/consultaNFCe" },
            { "RJ", "http://www4.fazenda.rj.gov.br/consultaNFCe/QRCode" },
            { "RN", "http://nfce.set.rn.gov.br/consultarNFCe.aspx" },
            { "RO", "http://www.nfce.sefin.ro.gov.br/consultanfce/consulta.jsp" },
            { "RR", "https://www.sefaz.rr.gov.br/nfce/servlet/qrcode" },
            { "RS", "https://www.sefaz.rs.gov.br/NFCE/NFCE-COM.aspx" },
            { "SC", "" },
            { "SE", "http://www.nfce.se.gov.br/portal/consultarNFCe.jsp" },
            { "SP", "https://www.nfce.fazenda.sp.gov.br/NFCeConsultaPublica/Paginas/ConsultaQRCode.aspx" },
            { "TO", "http://apps.sefaz.to.gov.br/portal-nfce/qrcodeNFCe" }
        };

        public static readonly Dictionary<string, string> Homologacao = new Dictionary<string, string>
        {
            { "AC", "http://www.hml.sefaznet.ac.gov.br/nfce/qrcode" },
            { "AL", "http://nfce.sefaz.al.gov.br/QRCode/consultarNFCe.jsp" },
            { "AM", "http://homnfce.sefaz.am.gov.br/nfceweb/consultarNFCe.jsp" },
            { "AP", "https://www.sefaz.ap.gov.br/nfcehml/nfce.php" },
            { "BA", "http://hnfe.sefaz.ba.gov.br/servicos/nfce/modulos/geral/NFCEC_consulta_chave_acesso.aspx" },
            { "CE", "http://nfceh.sefaz.ce.gov.br/pages/ShowNFCe.html" },
            { "DF", "http://dec.fazenda.df.gov.br/ConsultarNFCe.aspx" },
            { "ES", "http://homologacao.sefaz.es.gov.br/ConsultaNFCe/qrcode.aspx" },
            { "GO", "http://homolog.sefaz.go.gov.br/nfeweb/sites/nfce/danfeNFCe" },
            { "MA", "http://www.hom.nfce.sefaz.ma.gov.br/portal/consultarNFCe.jsp" },
            { "MG", "" },
            { "MS", "http://www.dfe.ms.gov.br/nfce/qrcode" },
            { "MT", "http://homologacao.sefaz.mt.gov.br/nfce/consultanfce" },
            { "PA", "https://appnfc.sefa.pa.gov.br/portal-homologacao/view/consultas/nfce/nfceForm.seam" },
            { "PB", "http://www.receita.pb.gov.br/nfcehom" },
            { "PE", "http://nfcehomolog.sefaz.pe.gov.br/nfce-web/consultarNFCe" },
            { "PI", "http://webas.sefaz.pi.gov.br/nfceweb-homologacao/consultarNFCe.jsf" },
            { "PR", "http://www.dfeportal.fazenda.pr.gov.br/dfe-portal/rest/servico/consultaNFCe" },
            { "RJ", "http://www4.fazenda.rj.gov.br/consultaNFCe/QRCode" },
            { "RN", "http://hom.nfce.set.rn.gov.br/consultarNFCe.aspx" },
            { "RO", "http://www.nfce.sefin.ro.gov.br/consultanfce/consulta.jsp" },
            { "RR", "http://200.174.88.103:8080/nfce/servlet/qrcode" },
            { "RS", "https://www.sefaz.rs.gov.br/NFCE/NFCE-COM.aspx" },
            { "SC", "" },
            { "SE", "http://www.hom.nfe.se.gov.br/portal/consultarNFCe.jsp" },
            { "SP", "https://www.homologacao.nfce.fazenda.sp.gov.br/NFCeConsultaPublica/Paginas/ConsultaQRCode.aspx" },
            { "TO", "http://apps.sefaz.to.gov.br/portal-nfce-homologacao/qrcodeNFCe" }
        };
    }
}
