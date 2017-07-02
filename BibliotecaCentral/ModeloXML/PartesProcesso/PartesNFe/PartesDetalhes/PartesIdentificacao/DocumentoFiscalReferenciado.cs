using System.Xml.Serialization;

namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesIdentificacao
{
    /// <summary>
    /// Apenas um desses "filhos" pode ser diferente de null.
    /// </summary>
    public sealed class DocumentoFiscalReferenciado
    {
        /// <summary>
        /// Referencia uma NF-e (modelo 55) emitida anteriormente, vinculada a NF-e atual, ou uma NFC-e (modelo 65).
        /// </summary>
        [XmlElement("refNFe", Order = 0)]
        public string RefNFe { get; set; }

        [XmlElement("refNF", Order = 1)]
        public NF1AReferenciada RefNF { get; set; }

        [XmlElement("refNFP", Order = 2)]
        public NFProdutorRuralReferenciada RefNFP { get; set; }
    }
}
