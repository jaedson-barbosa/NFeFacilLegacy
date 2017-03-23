using NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto;
using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes
{
    [XmlRoot(ElementName = "det", Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class DetalhesProdutos
    {
        private static string RemoverPonto(string str) => str.Contains(".") ? str.Substring(0, str.IndexOf('.')) : str;

        [XmlAttribute(AttributeName = "nItem")]
        public int número;

        [XmlElement(ElementName = "prod")]
        public ProdutoOuServico produto { get; set; } = new ProdutoOuServico();

        [XmlElement(ElementName = "imposto", Namespace = "http://www.portalfiscal.inf.br/nfe")]
        public Impostos impostos { get; set; } = new Impostos();

        /// <summary>
        /// (Opcional)
        /// Informações Adicionais do DadosProduto.
        /// </summary>
        public string infAdProd { get; set; }
    }
}
