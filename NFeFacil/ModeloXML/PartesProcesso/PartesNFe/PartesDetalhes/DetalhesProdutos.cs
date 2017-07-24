using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes
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

        /// <summary>
        /// (Opcional)
        /// Informações Adicionais do DadosProduto.
        /// </summary>
        [XmlElement("infAdProd", Order = 2)]
        public string InfAdProd { get; set; }
    }
}
