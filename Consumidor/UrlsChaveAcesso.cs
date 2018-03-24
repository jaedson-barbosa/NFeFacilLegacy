using System.Collections.Generic;

namespace NFeFacil.Fiscal.ViewNFCe
{
    public static class UrlsChaveAcesso
    {
        public static readonly Dictionary<string, string> Producao = new Dictionary<string, string>
        {
            { "AC", "http://www.sefaznet.ac.gov.br/nfce/consulta" },
            { "AL", "http://nfce.sefaz.al.gov.br/consultaNFCe.htm" },
            { "AM", "http://sistemas.sefaz.am.gov.br/nfceweb/formConsulta.do" },
            { "AP", "https://www.sefaz.ap.gov.br/sate/seg/SEGf_AcessarFuncao.jsp?cdFuncao=FIS_1261" },
            { "BA", "http://nfe.sefaz.ba.gov.br/servicos/nfce/default.aspx" },
            { "CE", "http://nfce.sefaz.ce.gov.br/pages/consultaNota.jsf" },
            { "DF", "http://dec.fazenda.df.gov.br/NFCE/" },
            { "ES", "http://app.sefaz.es.gov.br/ConsultaNFCe" },
            { "GO", "http://www.nfce.go.gov.br/post/ver/214278/consumid" },
            { "MA", "http://www.nfce.sefaz.ma.gov.br/portal/consultaNFe.do?method=preFilterCupom&" },
            { "MG", "" },
            { "MS", "http://www.dfe.ms.gov.br/nfce" },
            { "MT", "http://www.sefaz.mt.gov.br/nfce/consultanfce" },
            { "PA", "https://appnfc.sefa.pa.gov.br/portal/view/consultas/nfce/consultanfce.seam" },
            { "PB", "http://www.receita.pb.gov.br/nfce" },
            { "PE", "http://nfce.sefaz.pe.gov.br/nfce-web/entradaConsNfce" },
            { "PI", "http://webas.sefaz.pi.gov.br/nfceweb/consultarNFCe.jsf" },
            { "PR", "http://www.fazenda.pr.gov.br" },
            { "RJ", "http://www.nfce.fazenda.rj.gov.br/consulta" },
            { "RN", "http://nfce.set.rn.gov.br/consultarNFCe.aspx" },
            { "RO", "http://www.nfce.sefin.ro.gov.br" },
            { "RR", "https://www.sefaz.rr.gov.br/nfce/servlet/wp_consulta_nfce" },
            { "RS", "https://www.sefaz.rs.gov.br/NFCE/NFCE-COM.aspx" },
            { "SC", "" },
            { "SE", "http://www.nfce.se.gov.br/portal/portalNoticias.jsp" },
            { "SP", "https://www.nfce.fazenda.sp.gov.br/NFCeConsultaPublica" },
            { "TO", "http://apps.sefaz.to.gov.br/portal-nfce/consultarNFCe.jsf" }
        };

        public static readonly Dictionary<string, string> Homologacao = new Dictionary<string, string>
        {
            { "AC", "http://hml.sefaznet.ac.gov.br/nfce/consulta" },
            { "AL", "http://nfce.sefaz.al.gov.br/consultaNFCe.htm" },
            { "AM", "http://homnfce.sefaz.am.gov.br/nfceweb/formConsulta.do" },
            { "AP", "https://www.sefaz.ap.gov.br/sate1/seg/SEGf_AcessarFuncao.jsp?cdFuncao=FIS_1261" },
            { "BA", "http://hnfe.sefaz.ba.gov.br/servicos/nfce/default.aspx " },
            { "CE", "http://nfceh.sefaz.ce.gov.br/pages/consultaNota.jsf" },
            { "DF", "http://dec.fazenda.df.gov.br/NFCE/" },
            { "ES", "http://homologacao.sefaz.es.gov.br/ConsultaNFCe" },
            { "GO", "http://www.nfce.go.gov.br/post/ver/214413/consulta-nfc-e-homologacao" },
            { "MA", "http://www.hom.nfce.sefaz.ma.gov.br/portal/consultarNFCe.jsp" },
            { "MG", "" },
            { "MS", "http://www.dfe.ms.gov.br/nfce" },
            { "MT", "http://homologacao.sefaz.mt.gov.br/nfce/consultanfce" },
            { "PA", "https://appnfc.sefa.pa.gov.br/portal-homologacao/view/consultas/nfce/consultanfce.seam" },
            { "PB", "http://www.receita.pb.gov.br/nfcehom" },
            { "PE", "http://nfcehomolog.sefaz.pe.gov.br/nfce-web/entradaConsNfce" },
            { "PI", "http://webas.sefaz.pi.gov.br/nfceweb-homologacao/consultarNFCe.jsf" },
            { "PR", "http://www.fazenda.pr.gov.br" },
            { "RJ", "http://www.nfce.fazenda.rj.gov.br/consulta" },
            { "RN", "http://nfce.set.rn.gov.br/consultarNFCe.aspx" },
            { "RO", "http://www.nfce.sefin.ro.gov.br" },
            { "RR", "http://200.174.88.103:8080/nfce/servlet/wp_consulta_nfce" },
            { "RS", "https://www.sefaz.rs.gov.br/NFCE/NFCE-COM.aspx" },
            { "SC", "" },
            { "SE", "http://www.hom.nfe.se.gov.br/portal/portalNoticias.jsp" },
            { "SP", "https://www.homologacao.nfce.fazenda.sp.gov.br/NFCeConsultaPublica" },
            { "TO", "http://apps.sefaz.to.gov.br/portal-nfce-homologacao/consultarNFCe.jsf" }
        };
    }
}
