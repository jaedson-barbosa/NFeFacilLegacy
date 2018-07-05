using BaseGeral.View;
using System.Xml.Serialization;

namespace BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesProdutoOuServico
{
    public sealed class Combustivel
    {
        [XmlElement("cProdANP", Order = 0), DescricaoPropriedade("Código de Produto da ANP")]
        public int CProdANP { get; set; }

        [XmlElement("pMixGN", Order = 1), DescricaoPropriedade("Percentual de Gás Natural para o Produto GLP")]
        public string PMixGN { get; set; }

        [XmlElement(Order = 2), DescricaoPropriedade("Sistema de Controle do Diferimento do Imposto nas Operações com AEAC")]
        public string CODIF { get; set; }

        [XmlElement("qTemp", Order = 3), DescricaoPropriedade("Quantidade de combustível faturada à temperatura ambiente")]
        public string QTemp { get; set; }

        [XmlElement(Order = 4), DescricaoPropriedade("Informar a UF de consumo")]
        public string UFCons { get; set; }

        [XmlElement(Order = 5), DescricaoPropriedade("Grupo de informações da CIDE")]
        public CIDE CIDE { get; set; }
    }
}
