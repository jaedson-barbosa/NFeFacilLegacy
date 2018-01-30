using NFeFacil.View;
using NFeFacil.ModeloXML.PartesDetalhes;
using System.Xml.Serialization;
using System.Collections.Generic;
using static NFeFacil.ExtensoesPrincipal;

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

        [DescricaoPropriedade("Formas de pagamento")]
        [XmlElement("pag", Namespace = "http://www.portalfiscal.inf.br/nfe", Order = 8)]
        public List<Pagamento> FormasPagamento { get; set; }

        [DescricaoPropriedade("Informações Adicionais")]
        [XmlElement(Order = 9)]
        public InformacoesAdicionais infAdic { get; set; }
    }

    public sealed class Pagamento
    {
        [XmlElement("tPag", Order = 0)]
        public string Forma { get; set; }

        [XmlIgnore]
        public double vPag { get; set; }
        [XmlElement("vPag", Order = 1)]
        public string VPag { get => ToStr(vPag); set => vPag = Parse(value); }

        [XmlElement("card", Order = 2)]
        public Cartao Cartao { get; set; }
    }

    public sealed class Cartao
    {
        [XmlElement(Order = 0)]
        public string CNPJ { get; set; }

        [XmlElement("tBand", Order = 1)]
        public string Bandeira { get; set; }

        [XmlElement("cAut", Order = 2)]
        public string Autorizacao { get; set; }
    }
}
