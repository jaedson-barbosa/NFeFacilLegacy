using BaseGeral.View;
using BaseGeral.ModeloXML.PartesDetalhes;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace BaseGeral.ModeloXML
{
    [XmlRoot("infNFe", Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public sealed class InformacoesNFe : InformacoesBase
    {
        [DescricaoPropriedade("Local de retirada")]
        [XmlElement("retirada", Order = 3)]
        public RetiradaOuEntrega Retirada { get; set; }

        [DescricaoPropriedade("Local de entrega")]
        [XmlElement("entrega", Order = 4)]
        public RetiradaOuEntrega Entrega { get; set; }

        [DescricaoPropriedade("Produtos")]
        [XmlElement(ElementName = "det", Namespace = "http://www.portalfiscal.inf.br/nfe", Order = 5)]
        public List<DetalhesProdutos> produtos { get; set; }

        [DescricaoPropriedade("Total")]
        [XmlElement(Order = 6)]
        public Total total { get; set; }

        [DescricaoPropriedade("Transporte")]
        [XmlElement(Order = 7)]
        public Transporte transp { get; set; }

        [DescricaoPropriedade("Cobrança")]
        [XmlElement(Order = 8)]
        public Cobranca cobr { get; set; }

        [DescricaoPropriedade("Informações Adicionais")]
        [XmlElement(Order = 9)]
        public InformacoesAdicionais infAdic { get; set; }

        [DescricaoPropriedade("Exportação")]
        [XmlElement(Order = 10)]
        public Exportacao exporta { get; set; }

        [DescricaoPropriedade("Compra")]
        [XmlElement(Order = 11)]
        public Compra compra { get; set; }

        [DescricaoPropriedade("Cana de açúcar")]
        [XmlElement(Order = 12)]
        public RegistroAquisicaoCana cana { get; set; }
    }
}
