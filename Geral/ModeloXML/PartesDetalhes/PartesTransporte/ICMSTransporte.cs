using NFeFacil.View;
using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesDetalhes.PartesTransporte
{
    public class ICMSTransporte
    {
        [XmlElement("vServ", Order = 0), DescricaoPropriedade("Valor do serviço")]
        public double VServ { get; set; }

        [XmlElement("vBCRet", Order = 1), DescricaoPropriedade("BC da retenção do ICMS")]
        public double VBCRet { get; set; }

        [XmlElement("pICMSRet", Order = 2), DescricaoPropriedade("Alíquota da retenção")]
        public double PICMSRet { get; set; }

        [XmlElement("vICMSRet", Order = 3), DescricaoPropriedade("Valor do ICMS retido")]
        public double VICMSRet { get; set; }

        [XmlElement("CFOP", Order = 4)]
        public long CFOP { get; set; }

        [XmlElement("cMunFG", Order = 5), PropriedadeExtensivel("Município", MetodosObtencao.Municipio)]
        public int CMunFG { get; set; }
    }
}
