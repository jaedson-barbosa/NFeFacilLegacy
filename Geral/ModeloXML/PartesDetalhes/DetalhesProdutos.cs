using BaseGeral.View;
using BaseGeral.ModeloXML.PartesDetalhes.PartesProduto;
using System.Xml.Serialization;

namespace BaseGeral.ModeloXML.PartesDetalhes
{
    [XmlRoot(ElementName = "det", Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class DetalhesProdutos
    {
        [XmlAttribute(AttributeName = "nItem")]
        public int Número { get; set; }

        [XmlElement("prod", Order = 0)]
        public ProdutoOuServico Produto { get; set; } = new ProdutoOuServico();

        [XmlElement("imposto", Order = 1)]
        public Impostos Impostos { get; set; } = new Impostos();

        [XmlElement("impostoDevol", Order = 2), DescricaoPropriedade("Imposto devolvido")]
        public ImpostoDevol ImpostoDevol { get; set; }

        /// <summary>
        /// (Opcional)
        /// Informações Adicionais do DadosProduto.
        /// </summary>
        [XmlElement("infAdProd", Order = 3), DescricaoPropriedade("Informações adicionais")]
        public string InfAdProd { get; set; }
    }
}
