using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTotal
{
    public sealed class RetTrib
    {
        [XmlElement("vRetPIS", Order = 0), DescricaoPropriedade("Valor do PIS retido")]
        public double VRetPIS { get; set; }

        [XmlElement("vRetCOFINS", Order = 1), DescricaoPropriedade("Valor do COFINS retido")]
        public double VRetCOFINS { get; set; }

        [XmlElement("vRetCSLL", Order = 2), DescricaoPropriedade("Valor do CSLL retido")]
        public double VRetCSLL { get; set; }

        [XmlElement("vBCIRRF", Order = 3), DescricaoPropriedade("Valor do BC IRRF retido")]
        public double VBCIRRF { get; set; }

        [XmlElement("vIRRF", Order = 4), DescricaoPropriedade("Valor do IRRF retido")]
        public double VIRRF { get; set; }

        [XmlElement("vBCRetPrev", Order = 5), DescricaoPropriedade("Valor da BC da retenção da Previdência retido")]
        public double VBCRetPrev { get; set; }

        [XmlElement("vRetPrev", Order = 6), DescricaoPropriedade("Valor da retenção da Previdência retido")]
        public double VRetPrev { get; set; }
    }
}
