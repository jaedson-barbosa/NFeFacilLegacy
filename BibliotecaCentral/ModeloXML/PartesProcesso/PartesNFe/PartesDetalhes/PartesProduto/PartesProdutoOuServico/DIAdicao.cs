using System.Xml.Serialization;

namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesProduto.PartesProdutoOuServico
{
    public sealed class DIAdicao
    {
        /// <summary>
        /// Numero da Adição.
        /// </summary>
        [XmlElement("nAdicao")]
        public int NAdicao { get; set; }

        /// <summary>
        /// Numero sequencial do item dentro da Adição.
        /// </summary>
        [XmlElement("nSeqAdic")]
        public int NSeqAdic { get; set; }

        /// <summary>
        /// Código do fabricante estrangeiro, usado nos sistemas internos de informação do emitente da NF-e.
        /// </summary>
        [XmlElement("cFabricante")]
        public string CFabricante { get; set; }

        /// <summary>
        /// (Campo opcional) Valor do desconto do item da DI – Adição.
        /// </summary>
        [XmlElement("vDescDI")]
        public string VDescDI { get; set; }

        /// <summary>
        ///  O número do Ato Concessório de Suspensão deve ser preenchido com 11 dígitos (AAAANNNNNND).
        ///  O número do Ato Concessório de Drawback Isenção deve ser preenchido com 9 dígitos (AANNNNNND).
        /// </summary>
        [XmlElement("nDraw")]
        public string NDraw { get; set; }
    }
}
