using NFeFacil.View;
using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesTransporte
{
    public sealed class Motorista
    {
        [XmlElement(Order = 0)]
        public string CPF { get; set; }

        [XmlElement(Order = 1)]
        public string CNPJ { get; set; }

        [XmlElement(ElementName = "xNome", Order = 2)]
        public string Nome { get; set; }

        [XmlElement(ElementName = "IE", Order = 3), DescricaoPropriedade("Inscrição estadual")]
        public string InscricaoEstadual { get; set; }

        [XmlElement("xEnder", Order = 4), DescricaoPropriedade("Endereço")]
        public string XEnder { get; set; }

        [XmlElement("xMun", Order = 5), DescricaoPropriedade("Nome do município")]
        public string XMun { get; set; }

        [XmlElement(Order = 6), PropriedadeExtensivel("Estado", MetodosObtencao.Estado)]
        public string UF { get; set; }

        [XmlIgnore]
        public string Documento => CNPJ ?? CPF;
    }
}
