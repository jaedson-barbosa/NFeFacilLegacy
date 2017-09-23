using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTransporte
{
    public class ICMSTransporte
    {
        /// <summary>
        /// Valor do Serviço.
        /// </summary>
        [XmlElement("vServ", Order = 0), DescricaoPropriedade("Valor do serviço")]
        public double VServ { get; set; }

        /// <summary>
        /// BC da Retenção do ICMS.
        /// </summary>
        [XmlElement("vBCRet", Order = 1), DescricaoPropriedade("Base de cálculo da retenção do ICMS")]
        public double VBCRet { get; set; }

        /// <summary>
        /// Alíquota da Retenção.
        /// </summary>
        [XmlElement("pICMSRet", Order = 2), DescricaoPropriedade("Alíquota da retenção")]
        public double PICMSRet { get; set; }

        /// <summary>
        /// Valor do ICMS Retido.
        /// </summary>
        [XmlElement("vICMSRet", Order = 3), DescricaoPropriedade("Valor do ICMS retido")]
        public double VICMSRet { get; set; }

        [XmlElement("CFOP", Order = 4)]
        public long CFOP { get; set; }

        /// <summary>
        /// Código do município de ocorrência do fato gerador do ICMS do transporte.
        /// </summary>
        [XmlElement("cMunFG", Order = 5), PropriedadeExtensivel("Município", MetodosObtencao.Municipio)]
        public int CMunFG { get; set; }
    }
}
