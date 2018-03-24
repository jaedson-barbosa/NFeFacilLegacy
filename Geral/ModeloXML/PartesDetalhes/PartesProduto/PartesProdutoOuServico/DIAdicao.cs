using BaseGeral.View;
using System.Xml.Serialization;

namespace BaseGeral.ModeloXML.PartesDetalhes.PartesProduto.PartesProdutoOuServico
{
    public sealed class DIAdicao
    {
        [XmlElement("nAdicao", Order = 0), DescricaoPropriedade("Número da Adição")]
        public int NAdicao { get; set; }

        [XmlElement("nSeqAdic", Order = 1), DescricaoPropriedade("Número sequencial do item dentro da Adição")]
        public int NSeqAdic { get; set; }

        [XmlElement("cFabricante", Order = 2), DescricaoPropriedade("Código do fabricante estrangeiro")]
        public string CFabricante { get; set; }

        [XmlElement("vDescDI", Order = 3), DescricaoPropriedade("Valor do desconto do item da DI – Adição")]
        public string VDescDI { get; set; }

        [XmlElement("nDraw", Order = 4), DescricaoPropriedade("O número do Ato Concessório")]
        public string NDraw { get; set; }
    }
}
