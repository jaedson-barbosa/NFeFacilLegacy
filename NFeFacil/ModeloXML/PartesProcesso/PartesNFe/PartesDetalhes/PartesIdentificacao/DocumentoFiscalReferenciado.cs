using NFeFacil.AtributosVisualizacao;
using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesIdentificacao
{
    /// <summary>
    /// Apenas um desses "filhos" pode ser diferente de null.
    /// </summary>
    public sealed class DocumentoFiscalReferenciado
    {
        /// <summary>
        /// Referencia uma NF-e (modelo 55) emitida anteriormente, vinculada a NF-e atual, ou uma NFC-e (modelo 65).
        /// </summary>
        [XmlElement("refNFe", Order = 0), DescricaoPropriedade("Referência a NF-e ou NFC-e")]
        public string RefNFe { get; set; }

        [XmlElement("refNF", Order = 1), DescricaoPropriedade("Referência a NF 1A")]
        public NF1AReferenciada RefNF { get; set; }

        [XmlElement("refNFP", Order = 2), DescricaoPropriedade("Referência a nota de produtor rural")]
        public NFProdutorRuralReferenciada RefNFP { get; set; }
    }
}
