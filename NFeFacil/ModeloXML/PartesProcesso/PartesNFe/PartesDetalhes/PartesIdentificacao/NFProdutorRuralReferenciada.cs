using NFeFacil.View;
using System.Xml.Serialization;

namespace NFeFacil.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesIdentificacao
{
    public sealed class NFProdutorRuralReferenciada
    {
        /// <summary>
        /// Informar a IE do emitente da NF de Produtor ou o literal “ISENTO”.
        /// </summary>
        [DescricaoPropriedade("IE do emitente da NF de produtor")]
        public string IE { get; set; }

        /// <summary>
        /// Utilizar esta TAG para referenciar um CT-e emitido anteriormente, vinculada a NF-e atual.
        /// </summary>
        [XmlElement("refCTe"), DescricaoPropriedade("CT-e emitido anteriormente")]
        public string refCTe { get; set; }
    }
}
