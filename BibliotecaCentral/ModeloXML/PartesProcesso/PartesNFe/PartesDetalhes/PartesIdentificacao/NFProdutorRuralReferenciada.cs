using System.Xml.Serialization;

namespace BibliotecaCentral.ModeloXML.PartesProcesso.PartesNFe.PartesDetalhes.PartesIdentificacao
{
    public sealed class NFProdutorRuralReferenciada
    {
        /// <summary>
        /// Informar a IE do emitente da NF de Produtor ou o literal “ISENTO”.
        /// </summary>
        public string IE { get; set; }

        /// <summary>
        /// Utilizar esta TAG para referenciar um CT-e emitido anteriormente, vinculada a NF-e atual.
        /// </summary>
        [XmlElement("refCTe")]
        public string refCTe { get; set; }
    }
}
