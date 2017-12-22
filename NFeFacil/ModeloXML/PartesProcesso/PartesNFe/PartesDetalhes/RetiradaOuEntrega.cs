using NFeFacil.AtributosVisualizacao;
using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes
{
    public sealed class RetiradaOuEntrega
    {
        [XmlElement(Order = 0)]
        public string CNPJ { get; set; }

        [XmlElement(Order = 1)]
        public string CPF { get; set; }

        [XmlElement("xLgr", Order = 2)]
        public string Logradouro { get; set; }

        [XmlElement("nro", Order = 3), DescricaoPropriedade("Número")]
        public string Numero { get; set; }

        [XmlElement("xCpl", Order = 4)]
        public string Complemento { get; set; }

        [XmlElement("xBairro", Order = 5)]
        public string Bairro { get; set; }

        [XmlElement("cMun", Order = 6), DescricaoPropriedade("Código do município")]
        public int CodigoMunicipio { get; set; }

        [XmlElement("xMun", Order = 7)]
        public string NomeMunicipio { get; set; }

        [XmlElement("UF", Order = 8), PropriedadeExtensivel("Estado", MetodosObtencao.Estado)]
        public string SiglaUF { get; set; }
    }
}
