using NFeFacil.View;
using NFeFacil.ModeloXML.PartesDetalhes;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace NFeFacil.ModeloXML
{
    [XmlRoot("infNFe", Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public sealed class InformacoesNFCe : InformacoesBase
    {
        [DescricaoPropriedade("Produtos")]
        [XmlElement(ElementName = "det", Namespace = "http://www.portalfiscal.inf.br/nfe", Order = 5)]
        public List<DetalhesProdutos> produtos { get; set; }

        [DescricaoPropriedade("Total")]
        [XmlElement(Order = 6)]
        public Total total { get; set; }

        [DescricaoPropriedade("Transporte")]
        [XmlElement(Order = 7)]
        public Transporte transp { get; set; }

        [DescricaoPropriedade("Informações Adicionais")]
        [XmlElement(Order = 8)]
        public InformacoesAdicionais infAdic { get; set; }
    }
}
