using System.Xml.Serialization;

namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTransporte
{
    public class ICMSTransporte
    {
        /// <summary>
        /// Valor do Serviço.
        /// </summary>
        [XmlElement("vServ", Order = 0)]
        public double VServ { get; set; }

        /// <summary>
        /// BC da Retenção do ICMS.
        /// </summary>
        [XmlElement("vBCRet", Order = 1)]
        public double VBCRet { get; set; }

        /// <summary>
        /// Alíquota da Retenção.
        /// </summary>
        [XmlElement("pICMSRet", Order = 2)]
        public double PICMSRet { get; set; }

        /// <summary>
        /// Valor do ICMS Retido.
        /// </summary>
        [XmlElement("vICMSRet", Order = 3)]
        public double VICMSRet { get; set; }

        [XmlElement("CFOP", Order = 4)]
        public long CFOP { get; set; }

        /// <summary>
        /// Código do município de ocorrência do fato gerador do ICMS do transporte.
        /// </summary>
        [XmlElement("cMunFG", Order = 5)]
        public int CMunFG { get; set; }
    }
}
